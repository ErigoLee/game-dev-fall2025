using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// HandJointSaveManager saves hand gesture data (left and right hand)
/// into JSON files whenever new gesture data is received.
/// </summary>
public class HandJointSaveManager : MonoBehaviour
{
    // Stores the latest received gesture
    private HandGestureData newHandGesture;

    // Stores all left-hand gestures
    private List<HandGestureData> saveLeftHandGesture;

    // Stores all right-hand gestures
    private List<HandGestureData> saveRightHandGesture;

    /// <summary>
    /// Initialize the left and right hand gesture lists
    /// </summary>
    private void Start()
    {
        saveLeftHandGesture = new List<HandGestureData>();
        saveRightHandGesture = new List<HandGestureData>();
    }

    /// <summary>
    /// Subscribe to the gesture data event when this object is enabled
    /// </summary>
    private void OnEnable()
    {
        HandJointRecord.ConveyGestureData += OnGestureDataReceived;
    }

    /// <summary>
    /// Unsubscribe from the gesture data event when this object is disabled
    /// (prevents memory leaks or unwanted calls)
    /// </summary>
    private void OnDisable()
    {
        HandJointRecord.ConveyGestureData -= OnGestureDataReceived;
    }

    /// <summary>
    /// Called whenever new gesture data is received.
    /// Saves the gesture into left or right hand lists based on isLeftActive.
    /// </summary>
    /// <param name="_newHandGesture">The new gesture data received</param>
    /// <param name="isLeftActive">True if the gesture belongs to the left hand, false if right hand</param>
    private void OnGestureDataReceived(HandGestureData _newHandGesture,bool isLeftActive)
    {
        newHandGesture = _newHandGesture;

        if (isLeftActive)
        {
            // Add to left hand list
            saveLeftHandGesture.Add(newHandGesture);
        }
        else
        {
            // Add to right hand list
            saveRightHandGesture.Add(newHandGesture);
        }

        // Save the current data to JSON files
        SaveDate();
       
    }

    /// <summary>
    /// Save the collected left and right hand gesture lists
    /// as JSON files inside the Scripts folder
    /// </summary>
    private void SaveDate()
    {
        // Convert left hand gesture list into JSON format
        string leftHandGestureDataJson = JsonHelper.ToJson(saveLeftHandGesture, prettyPrint: true);
        File.WriteAllText(Application.dataPath + "/Scripts/leftdata.json", leftHandGestureDataJson);

        // Convert right hand gesture list into JSON format
        string rightHandGestureDataJson = JsonHelper.ToJson(saveRightHandGesture, prettyPrint: true);
        File.WriteAllText(Application.dataPath + "/Scripts/rightdata.json", rightHandGestureDataJson);

    }
}
