using Oculus.Interaction.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Recognizes hand gestures for both left and right hands using Oculus Interaction SDK.
/// Compares joint positions against predefined gesture data and invokes events when gestures are detected.
/// Requires a HandJointLoadManager component on the same GameObject to function properly.
/// </summary>
[RequireComponent(typeof(HandJointLoadManager))]
public class HandGestureRecognizer : MonoBehaviour
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
    private List<HandGestureData> leftHandGestures;

    /// <summary>
    /// List of predefined gesture data for the right hand.
    /// Loaded dynamically via HandJointLoadManager.
    /// </summary>
    private List<HandGestureData> rightHandGestures;

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
    /// A Unity event that conveys hand gesture data for both hands.
    /// Allows custom actions to be assigned in the Unity editor.
    /// </summary>
    [SerializeField]
    public UnityEvent<HandGestureData, HandGestureData> BothHandGestureAction;

    /// <summary>
    /// Threshold for matching joint positions between current hand and predefined gestures.
    /// Lower values require more precise matching; higher values allow more tolerance.
    /// </summary>
    [SerializeField]
    [Tooltip("Distance threshold for gesture matching. Adjust based on hand tracking accuracy.")]
    [Range(0.01f, 0.2f)]
    private float threshold = 0.05f;

    /// <summary>
    /// Static event that conveys hand gesture data for both hands.
    /// The first parameter represents the **left hand** data, and the second represents the **right hand** data.
    /// Subscribed methods will receive current gesture data for processing.
    /// </summary>
    public static event Action<HandGestureData, HandGestureData> ConveyHandGestures;

    /// <summary>
    /// Static event that conveys hand gesture data specifically for the right hand.
    /// The first parameter represents the **current right-hand** gesture data,
    /// and the second parameter represents the **previous frame's right-hand** gesture data.
    /// Subscribed methods will receive gesture data for the right hand.
    /// </summary>
    public static event Action<HandGestureData, HandGestureData> ConveyRightHandGesture;

    /// <summary>
    /// Initializes the gesture recognizer.
    /// Sets up lists and gesture data objects, and resets loading flags.
    /// </summary>
    void Start()
    {
        // Initialize collections
        leftHandGestures = new List<HandGestureData>();
        rightHandGestures = new List<HandGestureData>();

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
    /// <param name="_leftHandGestures">List of HandGestureData for the left hand. If null, logs an error and disables loading.</param>
    private void LoadLeftHandData(List<HandGestureData> _leftHandGestures)
    {
        if (_leftHandGestures != null)
        {
            leftHandGestures = _leftHandGestures;
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
    /// <param name="_rightHandGestures">List of HandGestureData for the right hand. If null, logs an error and disables loading.</param>
    private void LoadRightHandData(List<HandGestureData> _rightHandGestures)
    {
        if (_rightHandGestures != null)
        {
            rightHandGestures = _rightHandGestures;
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
        // Check if both left and right hand data are loaded before proceeding
        if (isLoadingRight && isLoadingLeft)
        {
            // Initialize new instances for current gesture data
            currentLeftHandGesture = new HandGestureData();
            currentRightHandGesture = new HandGestureData();

            // Retrieve gesture data for both hands
            GetGestureData(leftHand);
            GetGestureData(rightHand);

            // Trigger any dual-hand gestures based on the current data
            TriggerDualGesture();

            // Invoke static events to convey gesture data to subscribers
            ConveyHandGestures?.Invoke(currentLeftHandGesture, currentRightHandGesture);
            ConveyRightHandGesture?.Invoke(currentRightHandGesture, preRightHandGesture);

            // Store current gestures as previous for the next frame
            // Ensure deep copy to avoid reference issues
            if (currentLeftHandGesture != null)
            {
                // Copy the name and perform a deep copy of joint positions for left hand
                preLeftHandGesture.name = currentLeftHandGesture.name;
                preLeftHandGesture.jointPositions = new List<Vector3>(currentLeftHandGesture.jointPositions); // Deep copy
            }
            if (currentRightHandGesture != null)
            {
                // Copy the name and perform a deep copy of joint positions for right hand
                preRightHandGesture.name = currentRightHandGesture.name;
                preRightHandGesture.jointPositions = new List<Vector3>(currentRightHandGesture.jointPositions); // Deep copy
            }
        }

    }

    /// <summary>
    /// Retrieves and processes joint position data for the given hand.
    /// Transforms joint positions to local space relative to the hand's root pose.
    /// Updates the appropriate current gesture data and attempts recognition.
    /// </summary>
    /// <param name="hand">The Hand component to process (left or right).</param>
    private void GetGestureData(Hand hand)
    {
       

        if (hand == null)
        {
            Debug.LogError("Attempted to store left hand joints from a null Hand reference.");
            return;
        }

        // Optional: Debug tracking validity (can be commented out for performance)
        //Debug.Log($"LeftHand IsTrackedDataValid: {leftHand.IsTrackedDataValid}, Hand name: {leftHand.gameObject.name}");
        if (hand.IsTrackedDataValid)
        {
            // Get the root pose for local space transformation
            if (!hand.GetRootPose(out Pose rootPose))
            {
                Debug.LogWarning("Failed to get root pose!");
                return;
            }

            // Initialize a new list to collect data each frame
            List<Vector3> collectedData = new List<Vector3>();

            // Filter out joints that are not actual tracking points or are placeholders.
            // Use direct enum comparison for better performance and type safety.
            IEnumerable<HandJointId> allJointIds = System.Enum.GetValues(typeof(HandJointId))
                                                     .Cast<HandJointId>()
                                                     .Where(joint => System.Enum.IsDefined(typeof(HandJointId), joint));
            
            // Iterate through all defined HandJointId values
            foreach (HandJointId joint in allJointIds)
            {

                // Filter out joints that are not actual tracking points or are placeholders
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
                    if (hand.GetJointPose(joint, out Pose pose))
                    {
                        // Transform to local space relative to root pose
                        Vector3 relativePos = pose.position - rootPose.position;
                        Vector3 localPos = Quaternion.Inverse(rootPose.rotation) * relativePos;
                        collectedData.Add(localPos);

                    }
                    else
                    {
                        // Log a warning if a pose couldn't be retrieved for a valid joint
                        Debug.LogWarning($"Failed to get pose for joint: {joint.ToString()}");
                    }
                }
                catch (System.IndexOutOfRangeException ex)
                {
                    Debug.LogError($"IndexOutOfRangeException for joint {joint.ToString()} : {ex.Message}. Skipping this joint.");
                    continue;
                }

            }

            // Assign to the correct current gesture and run detection
            if (hand == leftHand)
            {
                currentLeftHandGesture = new HandGestureData(collectedData);
                HandGestureDetector(hand, currentLeftHandGesture, leftHandGestures);
            }
            else if (hand == rightHand)
            {
                currentRightHandGesture = new HandGestureData(collectedData);
                HandGestureDetector(hand, currentRightHandGesture, rightHandGestures);
            }
            else
            {
                Debug.LogWarning($"Unrecognized Hand reference '{hand?.gameObject.name ?? "null"}' passed to HandGestureRecognizer. " + $"Expected '{leftHand?.gameObject.name}' or '{rightHand?.gameObject.name}'.");
            }

        }
        else
        {
            Debug.LogWarning($"Cannot store joint data for {leftHand.gameObject.name} because its tracking data is not valid.");
        }
    }

    /// <summary>
    /// Detects gestures for the given hand by comparing current joint positions against predefined gesture data.
    /// If a match is found within the threshold, invokes the appropriate event.
    /// </summary>
    /// <param name="hand">The Hand component being processed.</param>
    /// <param name="currentHandGesture">The current gesture data for this hand.</param>
    /// <param name="handGestures">List of predefined gesture data for this hand.</param>
    private void HandGestureDetector(Hand hand, HandGestureData currentHandGesture, List<HandGestureData> handGestures)
    {
        // Proceed only if the hand is not null
        if (hand != null)
        {
            bool isSuccessful = false; // Flag to track if a gesture match was found

            // Loop through each predefined gesture to find a match
            foreach (HandGestureData gesture in handGestures)
            {
                // Ensure the joint position counts match before comparing
                if (gesture.jointPositions.Count == currentHandGesture.jointPositions.Count)
                {
                    bool flag = true; // Flag for this gesture comparison

                    // Compare each joint position
                    for (int i = 0; i < gesture.jointPositions.Count; i++)
                    {
                        Vector3 _pos = gesture.jointPositions[i]; // Predefined joint position
                        Vector3 _pos2 = currentHandGesture.jointPositions[i]; // Current joint position
                        float distance = Vector3.Distance(_pos, _pos2); // Calculate distance

                        // If distance exceeds threshold, this gesture doesn't match
                        if (distance > threshold)
                        {
                            flag = false;
                            break; // Exit the inner loop early
                        }
                    }

                    // If all joints matched, mark as successful and set the gesture name
                    if (flag)
                    {
                        isSuccessful = true;
                        currentHandGesture.name = gesture.name;
                        break; // Exit the outer loop since a match was found
                    }

                }
            }

            // If a gesture was successfully detected, invoke the appropriate event
            if (isSuccessful)
            {
                // Invoke the correct event based on the hand
                if (currentHandGesture == currentLeftHandGesture)
                {
                    LeftHandGestureAction.Invoke(currentLeftHandGesture, true);
                }
                else if (currentHandGesture == currentRightHandGesture)
                {
                    RightHandGestureAction.Invoke(currentRightHandGesture, false);
                }
                else
                {
                    // Log a warning if the gesture doesn't match expected references
                    Debug.LogWarning($"[HandGestureDetector] currentHandGesture does not match left/right buffers " + $"(ref mismatch). Is it a new instance? leftRef={(currentLeftHandGesture != null)}, " + $"rightRef={(currentRightHandGesture != null)}, curCount={currentHandGesture?.jointPositions?.Count}");
                }

            }

        }
    }

    /// <summary>
    /// Triggers a dual-hand gesture event if both hands have recognized gestures.
    /// </summary>
    private void TriggerDualGesture()
    {
        // Return early if either hand gesture is empty (not recognized)
        if (string.Equals(currentLeftHandGesture, "") || string.Equals(currentRightHandGesture, ""))
            return;

        // Invoke the event for both-hand gestures
        BothHandGestureAction.Invoke(currentLeftHandGesture,currentRightHandGesture);
    }
}