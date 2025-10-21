using System.Collections.Generic;
using UnityEngine;
using Oculus.Interaction.Input;
using Oculus.Interaction;
using UnityEngine.Events;
using System.Linq;
using System; // For cleaner enum filtering

/// <summary>
/// This script records hand joint positions using Oculus Interaction components.
/// It allows recording for either the left or right hand exclusively and saves the data for gesture recognition.
/// </summary>
[RequireComponent(typeof(HandJointSaveManager))]
public class HandJointRecord : MonoBehaviour
{
    /// <summary>
    /// Reference to the left hand tracking component.
    /// </summary>
    [Tooltip("Reference to the left hand tracking component.")]
    [SerializeField] private Hand _leftHand;

    /// <summary>
    /// Reference to the right hand tracking component.
    /// </summary>
    [Tooltip("Reference to the right hand tracking component.")]
    [SerializeField] private Hand _rightHand;

    /// <summary>
    /// If true, joint data will be recorded for the left hand.
    /// </summary>
    [Tooltip("If true, joint data will be recorded for the left hand.")]
    [SerializeField] private bool _isLeftHandActive = false;

    /// <summary>
    /// If true, joint data will be recorded for the right hand.
    /// </summary>
    [Tooltip("If true, joint data will be recorded for the right hand.")]
    [SerializeField] private bool _isRightHandActive = false;

    // Stores the latest recorded joint positions for the active hand.
    // If you need to store multiple snapshots, this would need to be a List<List<Vector3>> or similar.
    private List<Vector3> _recordedJointPositions = new List<Vector3>();

    // After gesture data is obtained, it will be passed to GestureJointSaveManger.
    public static event Action<HandGestureData, bool> ConveyGestureData;

    /// <summary>
    /// Ensures mutual exclusivity between left and right hand activation in the Inspector.
    /// </summary>
    private void OnValidate()
    {
        // Enforce mutual exclusivity in the Inspector
        if (_isLeftHandActive && _isRightHandActive)
        {
#if UNITY_EDITOR
            // This approach is more robust for OnValidate than checking prefabOverride.
            // It simply ensures only one can be active at a time, preferring the right hand if both are set.
            // Or you could add a timestamp logic if you wanted to keep the *last* one set.
            // For simplicity, let's say if both are checked, the right hand takes precedence.
            // Alternatively, you could just reset _isRightHandActive to false if _isLeftHandActive was set.
            Debug.LogWarning("Both Left and Right Hand Active checkboxes were checked. Deactivating Left Hand to maintain exclusivity.");
            _isLeftHandActive = false; // Deactivate one to ensure exclusivity
#endif
        }
    }

    /// <summary>
    /// Gets or sets whether the right hand is active, ensuring mutual exclusivity.
    /// </summary>
    public bool IsRightHandActive
    {
        get => _isRightHandActive;
        set
        {
            _isRightHandActive = value;
            if (value) _isLeftHandActive = false; // Ensure mutual exclusivity
        }
    }

    /// <summary>
    /// Gets or sets whether the left hand is active, ensuring mutual exclusivity.
    /// </summary>
    public bool IsLeftHandActive
    {
        get => _isLeftHandActive;
        set
        {
            _isLeftHandActive = value;
            if (value) _isRightHandActive = false; // Ensure mutual exclusivity
        }
    }

    /// <summary>
    /// Updates the script each frame, checking for space key input to record joint data.
    /// </summary>
    void Update()
    {
        // Only allow recording if one hand is explicitly active
        if (_isRightHandActive || _isLeftHandActive)
        {
            // Check for space key press to initiate recording
            if (Input.GetKeyDown(KeyCode.Space))
            {
                // Call the unified storage method based on which hand is active
                if (_isRightHandActive)
                {
                    StoreHandJoints(_rightHand);
                }
                else // _isLeftHandActive must be true here due to the outer if condition
                {
                    StoreHandJoints(_leftHand);
                }
            }
        }
    }

    /// <summary>
    /// Stores the local positions of all valid joints for the given hand.
    /// </summary>
    /// <param name="handToStore">The Hand component from which to get joint data.</param>
    private void StoreHandJoints(Hand handToStore)
    {
        // Check if the hand reference is valid
        if (handToStore == null)
        {
            Debug.LogError("Attempted to store hand joints from a null Hand reference.");
            return;
        }

        // Proceed only if hand tracking data is valid
        if (handToStore.IsTrackedDataValid)
        {
            // Clear previous data to store the new snapshot
            _recordedJointPositions.Clear();

            // Attempt to get the root pose of the hand
            if (!handToStore.GetRootPose(out Pose rootPose))
            {
                Debug.Log("Failed to get root pose!");
                return;
            }

            // Cache the enum values once to avoid repeated allocations if this method were called frequently
            // For a Space key press, it's not strictly necessary, but good practice.
            // Using OfType<HandJointId>() filters out potential invalid casts from GetValues() if the enum has weird definitions.
            // While Enum.IsDefined handles this, LINQ can be a bit cleaner.
            IEnumerable<HandJointId> allJointIds = System.Enum.GetValues(typeof(HandJointId))
                                                         .Cast<HandJointId>()
                                                         .Where(joint => System.Enum.IsDefined(typeof(HandJointId), joint));

            // Iterate through each joint ID
            foreach (HandJointId joint in allJointIds)
            {
                // Skip invalid or undefined joint values
                if (string.Equals(joint.ToString(), "HandEnd") || string.Equals(joint.ToString(),"Invalid"))
                {
                    //skip if the value is invalid (or print a log message)
                    Debug.LogWarning($"Isvalid HandJointId value: {joint}");
                    continue;
                }
                try
                {
                    // Attempt to get the pose for this joint
                    if (handToStore.GetJointPose(joint, out Pose pose))
                    {
                        // Calculate position relative to the hand's root pose
                        Vector3 relativePos = pose.position - rootPose.position;
                        Vector3 localPos = Quaternion.Inverse(rootPose.rotation) * relativePos;
                        // Add the local position to the recorded list
                        _recordedJointPositions.Add(localPos);
                    }
                    else
                    {
                        Debug.LogWarning($"Failed to get pose for joint: {joint.ToString()} for hand {handToStore.gameObject.name}.");
                    }
                }
                catch (System.IndexOutOfRangeException ex)
                {
                    Debug.LogError($"IndexOutOfRangeException for joint {joint.ToString()} on hand {handToStore.gameObject.name}: {ex.Message}. Skipping this joint.");
                    continue;
                }
            }

            // Log a summary after recording all joints
            Debug.Log($"Successfully recorded {_recordedJointPositions.Count} joint positions for {handToStore.gameObject.name}.");
        }
        else
        {
            Debug.LogWarning($"Cannot store joint data for {handToStore.gameObject.name} because its tracking data is not valid.");
        }

        // Create a new HandGestureData object with the recorded positions
        HandGestureData newHandGesture = new HandGestureData("newGesture", _recordedJointPositions);
        // Invoke the event based on which hand is active
        if (IsLeftHandActive)
        {
            ConveyGestureData?.Invoke(newHandGesture, true);
        }
        else
        {
            ConveyGestureData?.Invoke(newHandGesture, false);
        }

       
        
    }

    /// <summary>
    /// Returns the last recorded list of hand joint positions.
    /// </summary>
    /// <returns>A read-only list of recorded joint positions.</returns>
    public IReadOnlyList<Vector3> GetRecordedJointPositions()
    {
        return _recordedJointPositions;
    }


    /// <summary>
    /// Debug method to log the recorded joint positions.
    /// </summary>
    public void ReadingPose()
    {
        int i = 0;
        // Iterate through each recorded position and log it
        foreach (Vector3 _pos in _recordedJointPositions)
        {
            Debug.Log(i + ": x-" + _pos.x + " y-" + _pos.y + " z-" + _pos.z);
            i++;
        }

    }
}
