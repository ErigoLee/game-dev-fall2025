using Oculus.Interaction.Input;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Recognizes hand gestures for both left and right hands using Oculus Interaction SDK.
/// Compares joint positions against predefined gesture data and invokes events when gestures are detected.
/// </summary>
public class HandGestureRecognizer2: MonoBehaviour
{
    /// <summary>
    /// The left hand component from Oculus Interaction for tracking joint positions.
    /// </summary>
    [SerializeField]
    [Tooltip("Assign the left Hand component from Oculus Interaction.")]
    private Hand leftHand;

    /// <summary>
    /// The right hand component from Oculus Interaction for tracking joint positions.
    /// </summary>
    [SerializeField]
    [Tooltip("Assign the right Hand component from Oculus Interaction.")]
    private Hand rightHand;

    /// <summary>
    /// List of predefined gesture data for the left hand.
    /// Loaded dynamically via HandJointLoadManager.
    /// </summary>
    private List<HandGestureData> leftHandGesture;

    /// <summary>
    /// List of predefined gesture data for the right hand.
    /// Loaded dynamically via HandJointLoadManager.
    /// </summary>
    private List<HandGestureData> rightHandGesture;

    /// <summary>
    /// Flag indicating if left hand gesture data has been loaded.
    /// Used to prevent gesture recognition before data is ready.
    /// </summary>
    private bool isLoadingLeft;

    /// <summary>
    /// Flag indicating if right hand gesture data has been loaded.
    /// Used to prevent gesture recognition before data is ready.
    /// </summary>
    private bool isLoadingRight;


    /// <summary>
    /// Stores the previous frame's left hand gesture data for comparison.
    /// Helps prevent repeated event invocations for sustained gestures.
    /// </summary>
    private HandGestureData preLeftHandGesture;

    /// <summary>
    /// Stores the current frame's left hand gesture data.
    /// Updated each frame with joint positions from the left hand.
    /// </summary>
    private HandGestureData currentLeftHandGesture;

    /// <summary>
    /// Stores the previous frame's right hand gesture data for comparison.
    /// Helps prevent repeated event invocations for sustained gestures.
    /// </summary>
    private HandGestureData preRightHandGesture;

    /// <summary>
    /// Stores the current frame's right hand gesture data.
    /// Updated each frame with joint positions from the right hand.
    /// </summary>
    private HandGestureData currentRightHandGesture;

    /// <summary>
    /// Event invoked when a left hand gesture is recognized.
    /// Passes the recognized HandGestureData and a boolean (true for left hand).
    /// </summary>
    [SerializeField]
    [Tooltip("Event triggered when a left hand gesture is detected. Add listeners in the Inspector.")]
    public UnityEvent<HandGestureData, bool> LeftHandGestureAction;


    /// <summary>
    /// Event invoked when a right hand gesture is recognized.
    /// Passes the recognized HandGestureData and a boolean (false for right hand).
    /// </summary>
    [SerializeField]
    [Tooltip("Event triggered when a right hand gesture is detected. Add listeners in the Inspector.")]
    public UnityEvent<HandGestureData, bool> RightHandGestureAction;

    /// <summary>
    /// Threshold for matching joint positions between current hand and predefined gestures.
    /// Lower values require more precise matching; higher values allow more tolerance.
    /// </summary>
    [SerializeField]
    [Tooltip("Distance threshold for gesture matching. Adjust based on hand tracking accuracy.")]
    [Range(0.01f, 0.2f)]
    private float threshold =0.05f;

    /// <summary>
    /// Initializes the gesture recognizer.
    /// Sets up lists and gesture data objects, and resets loading flags.
    /// </summary>
    void Start()
    {
        // Initialize collections
        leftHandGesture = new List<HandGestureData>();
        rightHandGesture = new List<HandGestureData>();

        // Assuming HandGestureData has a default constructor that initializes jointPositions as a new List<Vector3>()
        preLeftHandGesture = new HandGestureData();
        currentLeftHandGesture = new HandGestureData();
        preRightHandGesture = new HandGestureData();
        currentRightHandGesture = new HandGestureData();

        // Reset loading flags
        isLoadingLeft = false;
        isLoadingRight = false;

        // Optional: Validate serialized references
        if (leftHand == null)
        {
            Debug.LogWarning("Left Hand reference is not assigned. Left hand gesture recognition will be disabled.");
        }
        if (rightHand == null)
        {
            Debug.LogWarning("Right Hand reference is not assigned. Right hand gesture recognition will be disabled.");
        }
    }


    /// <summary>
    /// Subscribes to gesture data loading events when the component is enabled.
    /// </summary>
    private void OnEnable()
    {
        HandJointLoadManager.LoadLeftHandGestureData += LoadLeftHandData;
        HandJointLoadManager.LoadRightHandGestureData += LoadRightHandData;
    }

    /// <summary>
    /// Unsubscribes from gesture data loading events when the component is disabled.
    /// Prevents memory leaks and unnecessary event handling.
    /// </summary>
    private void OnDisable()
    {
        HandJointLoadManager.LoadLeftHandGestureData -= LoadLeftHandData;
        HandJointLoadManager.LoadRightHandGestureData -= LoadRightHandData;
    }

    /// <summary>
    /// Loads predefined gesture data for the left hand.
    /// Called by HandJointLoadManager when data is available.
    /// </summary>
    /// <param name="_leftHandGesture">List of HandGestureData for the left hand. If null, logs an error and disables loading.</param>
    private void LoadLeftHandData(List<HandGestureData> _leftHandGesture)
    {
        if(_leftHandGesture != null)
        {
            leftHandGesture = _leftHandGesture;
            isLoadingLeft = true;
        }
        else
        {
            Debug.LogError("Loaded left hand gesture data was null.");
            isLoadingLeft = false; // Or handle as an error state
        }

       
    }

    /// <summary>
    /// Loads predefined gesture data for the right hand.
    /// Called by HandJointLoadManager when data is available.
    /// </summary>
    /// <param name="_rightHandGesture">List of HandGestureData for the right hand. If null, logs an error and disables loading.</param>
    private void LoadRightHandData(List<HandGestureData> _rightHandGesture)
    {
        if (_rightHandGesture != null)
        {
            rightHandGesture = _rightHandGesture;
            isLoadingRight = true;
        }
        else
        {
            Debug.LogError("Loaded left hand gesture data was null.");
            isLoadingRight = false; // Or handle as an error state
        }

    }

    /// <summary>
    /// Updates the gesture recognition process each frame.
    /// Only processes if both left and right hand gesture data have been loaded.
    /// Retrieves current joint data and updates previous gesture states for comparison.
    /// </summary>
    void Update()
    {
        if (isLoadingRight && isLoadingLeft)
        {
            GetLeftGestureData();
            GetRightGestureData();


            // Store current gestures as previous for the next frame
            // Ensure deep copy to avoid reference issues
            if (currentLeftHandGesture != null)
            {
                preLeftHandGesture.name = currentLeftHandGesture.name;
                preLeftHandGesture.jointPositions = new List<Vector3>(currentLeftHandGesture.jointPositions); // Deep copy
            }
            if (currentRightHandGesture != null)
            {
                preRightHandGesture.name = currentRightHandGesture.name;
                preRightHandGesture.jointPositions = new List<Vector3>(currentRightHandGesture.jointPositions); // Deep copy
            }
        }
        
    }

    /// <summary>
    /// Retrieves and processes joint position data for the left hand.
    /// Transforms joint positions to local space relative to the hand's root pose.
    /// Updates the current left hand gesture data and attempts recognition.
    /// </summary>
    private void GetLeftGestureData()
    {
        // Initialize a new list to collect data each frame
        List<Vector3> collectedData = new List<Vector3>();

        if (leftHand == null)
        {
            Debug.LogError("Attempted to store left hand joints from a null Hand reference.");
            return;
        }

        // Optional: Debug tracking validity (can be commented out for performance)
        //Debug.Log($"LeftHand IsTrackedDataValid: {leftHand.IsTrackedDataValid}, Hand name: {leftHand.gameObject.name}");
        if (leftHand.IsTrackedDataValid)
        {
            // Get the root pose for local space transformation
            if (!leftHand.GetRootPose(out Pose rootPose))
            {
                Debug.LogWarning("Failed to get root pose!");
                return;
            }

            // Filter out joints that are not actual tracking points or are placeholders.
            // Use direct enum comparison for better performance and type safety.
            IEnumerable<HandJointId> allJointIds = System.Enum.GetValues(typeof(HandJointId))
                                                     .Cast<HandJointId>()
                                                     .Where(joint => System.Enum.IsDefined(typeof(HandJointId), joint));
            foreach (HandJointId joint in allJointIds)
            {
                
                //Filters out invalid (undefined) values
                if (string.Equals(joint.ToString(), "HandEnd") || string.Equals(joint.ToString(), "Invalid"))
                {
                    //skip if the value is invalid (or print a log message)
                    Debug.LogWarning($"Isvalid HandJointId value: {joint}");
                    continue;
                }

                // Try to get the pose for the current joint.
                // GetJointPose returns false if it fails; it typically does not throw IndexOutOfRangeException for invalid joints.
                try
                {
                    if (leftHand.GetJointPose(joint, out Pose pose))
                    {
                        // Transform to local space relative to root pose
                        Vector3 relativePos = pose.position - rootPose.position;
                        Vector3 localPos = Quaternion.Inverse(rootPose.rotation) * relativePos;
                        collectedData.Add(localPos);

                    }
                    else
                    {
                        // Log a warning if a pose couldn't be retrieved for a valid joint
                        Debug.Log($"Failed to get pose for joint: {joint.ToString()}");
                    }
                }
                catch (System.IndexOutOfRangeException ex)
                {
                    Debug.LogError($"IndexOutOfRangeException for joint {joint.ToString()} : {ex.Message}. Skipping this joint.");
                    continue;
                }

            }
            // Create a new HandGestureData object with the collected positions
            // Assuming HandGestureData constructor takes List<Vector3> for jointPositions
            currentLeftHandGesture = new HandGestureData(collectedData);
            LeftHandGestureRecognizer();
        }
        else
        {
            Debug.LogWarning($"Cannot store joint data for {leftHand.gameObject.name} because its tracking data is not valid.");
        }
    }
    /// <summary>
    /// Retrieves and processes joint position data for the right hand.
    /// Transforms joint positions to local space relative to the hand's root pose.
    /// Updates the current right hand gesture data and attempts recognition.
    /// </summary>
    private void GetRightGestureData()
    {
        // Initialize a new list to collect data each frame
        List<Vector3> collectedData = new List<Vector3>();
        if (rightHand == null)
        {
            Debug.LogError("Attempted to store right hand joints from a null Hand reference.");
            return;
        }

        // Optional: Debug tracking validity (can be commented out for performance)
        //Debug.Log($"rightHand IsTrackedDataValid: {rightHand.IsTrackedDataValid}, Hand name: {rightHand.gameObject.name}");
        
        if (rightHand.IsTrackedDataValid)
        {
            // Get the root pose for local space transformation
            if (!rightHand.GetRootPose(out Pose rootPose))
            {
                Debug.Log("Failed to get root pose!");
                return;
            }

            // Iterate through all defined HandJointId values
            // Filter out joints that are not actual tracking points or are placeholders.
            IEnumerable<HandJointId> allJointIds = System.Enum.GetValues(typeof(HandJointId))
                                                     .Cast<HandJointId>()
                                                     .Where(joint => System.Enum.IsDefined(typeof(HandJointId), joint));
            foreach (HandJointId joint in allJointIds)
            {
                //Filters out invalid (undefined) values
                if (string.Equals(joint.ToString(), "HandEnd") || string.Equals(joint.ToString(), "Invalid"))
                {
                    //skip if the value is invalid (or print a log message)
                    Debug.LogWarning($"Isvalid HandJointId value: {joint}");
                    continue;
                }
                try
                {
                    if (rightHand.GetJointPose(joint, out Pose pose))
                    {
                        Vector3 relativePos = pose.position - rootPose.position;
                        Vector3 localPos = Quaternion.Inverse(rootPose.rotation) * relativePos;
                        collectedData.Add(localPos);

                    }
                    else
                    {
                        Debug.Log($"Failed to get pose for joint: {joint.ToString()}");
                    }
                }
                catch (System.IndexOutOfRangeException ex)
                {
                    Debug.LogError($"IndexOutOfRangeException for joint {joint.ToString()} : {ex.Message}. Skipping this joint.");
                    continue;
                }

            }

            currentRightHandGesture = new HandGestureData(collectedData);
            RightHandGestureRecognizer();
        }
        else
        {
            Debug.LogWarning($"Cannot store joint data for {rightHand.gameObject.name} because its tracking data is not valid.");
        }


    }

    private void LeftHandGestureRecognizer()
    {
        if (leftHand != null)
        {
            bool isSuccessful = false;
            foreach (HandGestureData gesture in leftHandGesture)
            {
                if (gesture.jointPositions.Count == currentLeftHandGesture.jointPositions.Count)
                {
                    bool flag = true;
                    for (int i = 0; i < gesture.jointPositions.Count; i++)
                    {
                        Vector3 _pos = gesture.jointPositions[i];
                        Vector3 _pos2 = currentLeftHandGesture.jointPositions[i];
                        float distance = Vector3.Distance(_pos, _pos2);
                        if (distance > threshold)
                        {
                            flag = false;
                            break;
                        }
                    }
                    if (flag)
                    {
                        isSuccessful = true;
                        currentLeftHandGesture.name = gesture.name;
                        break;
                    }
                    
                }
            }
            if (isSuccessful)
            {
                LeftHandGestureAction.Invoke(currentLeftHandGesture, true);

            }
            
        }
    }

    private void RightHandGestureRecognizer()
    {
        if (rightHand != null)
        {
            bool isSuccessful = false;
            foreach (HandGestureData gesture in rightHandGesture)
            {

                if (gesture.jointPositions.Count == currentRightHandGesture.jointPositions.Count)
                {
                    bool flag = true;
                    for (int i = 0; i < gesture.jointPositions.Count; i++)
                    {
                        Vector3 _pos = gesture.jointPositions[i];
                        Vector3 _pos2 = currentRightHandGesture.jointPositions[i];
                        float distance = Vector3.Distance(_pos, _pos2);
                        if (distance > threshold)
                        {
                            flag = false;
                            break;
                        }
                    }
                    if (flag)
                    {
                        isSuccessful = true;
                        currentRightHandGesture.name = gesture.name;
                        break;
                    }
                }
            }
            if (isSuccessful)
            {
                RightHandGestureAction.Invoke(currentRightHandGesture, false);
            }
        }
    }
}
