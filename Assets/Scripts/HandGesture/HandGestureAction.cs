using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script handles gesture-based actions for controlling player movement.
/// It responds to different gesture types and hand gestures to move the player forward or backward.
/// </summary>
public class HandGestureAction : MonoBehaviour
{
    /// <summary>
    /// Reference to the camera that determines the forward direction.
    /// </summary>
    [SerializeField] private GameObject lookcam;

    /// <summary>
    /// Reference to the player GameObject that will be moved.
    /// </summary>
    [SerializeField] private GameObject player;

    /// <summary>
    /// Direction vector for forward movement, calculated from the camera's forward direction.
    /// </summary>
    private Vector3 forwardDir;

    /// <summary>
    /// Direction vector for backward movement, calculated as the negative of the camera's forward direction.
    /// </summary>
    private Vector3 backwardDir;

    /// <summary>
    /// Speed multiplier for movement.
    /// </summary>
    private float speed = 1f;

    /// <summary>
    /// Executes an action based on the recognized gesture type.
    /// Currently handles Scissors gesture for forward movement.
    /// </summary>
    /// <param name="gestureType">The type of gesture recognized.</param>
    public void ExecuteGestureAction(GestureType gestureType)
    {
        // Check if the gesture is Scissors
        if (gestureType == GestureType.Scissors)
        {
            // Calculate forward direction from camera, flatten to horizontal plane, and normalize
            forwardDir = lookcam.transform.forward;
            forwardDir.y = 0;
            forwardDir.Normalize();

            // Move the player forward
            player.transform.Translate(forwardDir * speed * Time.deltaTime);
        }
    }

    /// <summary>
    /// Performs an action based on hand gesture data, distinguishing between left and right hands.
    /// Currently logs the gesture name for debugging.
    /// </summary>
    /// <param name="_handGesture">The gesture data for the hand.</param>
    /// <param name="isLeft">True if it's the left hand, false for right.</param>
    public void PerformGestureAction(HandGestureData _handGesture, bool isLeft)
    {
        // Log the gesture based on which hand it is
        if (isLeft)
        {
            Debug.Log("LeftHandGestureData: " + _handGesture.name);
        }
        else
        {
            Debug.Log("RightHandGestureData: " + _handGesture.name);
        }
        

    }

    /// <summary>
    /// Executes forward movement action if both hands show the "One" gesture.
    /// </summary>
    /// <param name="leftGesture">Gesture data for the left hand.</param>
    /// <param name="rightGesture">Gesture data for the right hand.</param>
    public void ExecuteGoingAction(HandGestureData leftGesture, HandGestureData rightGesture)
    {
        // Return early if either gesture is null
        if (leftGesture == null || rightGesture == null) { return; }

        // Check if both hands are showing the "One" gesture
        if (string.Equals(leftGesture.name, "One") && string.Equals(rightGesture.name, "One"))
        {
            // Calculate forward direction from camera, flatten to horizontal plane, and normalize
            forwardDir = lookcam.transform.forward;
            forwardDir.y = 0;
            forwardDir.Normalize();

            // Move the player forward
            player.transform.Translate(forwardDir * speed * Time.deltaTime);
        }
    }

    /// <summary>
    /// Executes backward movement action if both hands show the "Thumb" gesture.
    /// </summary>
    /// <param name="leftGesture">Gesture data for the left hand.</param>
    /// <param name="rightGesture">Gesture data for the right hand.</param>
    public void ExecuteBackGoingAction(HandGestureData leftGesture, HandGestureData rightGesture)
    {
        // Return early if either gesture is null
        if (leftGesture == null || rightGesture == null) { return; }

        // Check if both hands are showing the "Thumb" gesture
        if (string.Equals(leftGesture.name, "Thumb") && string.Equals(rightGesture.name, "Thumb"))
        {
            // Calculate backward direction as negative forward, flatten to horizontal plane, and normalize
            backwardDir = -lookcam.transform.forward;
            backwardDir.y = 0;
            backwardDir.Normalize();

            // Move the player backward
            player.transform.Translate(backwardDir * speed * Time.deltaTime);
        }
    }
}
