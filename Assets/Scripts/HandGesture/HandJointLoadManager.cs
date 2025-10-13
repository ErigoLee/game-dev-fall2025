using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


/// <summary>
/// Manages the loading of hand gesture data from JSON files.
/// This class loads predefined gesture data for left and right hands at startup
/// and notifies subscribers via static events.
/// </summary>
/// <remarks>
/// Assumes JSON files are located at "Application.dataPath/Scripts/leftdata.json" and "rightdata.json".
/// Uses JsonHelper for deserialization. Ensure JsonHelper is implemented correctly.
/// </remarks>
public class HandJointLoadManager : MonoBehaviour
{
    /// <summary>
    /// Event triggered when left hand gesture data is loaded.
    /// Subscribers can listen to this event to receive the loaded gesture data.
    /// </summary>
    public static event Action<List<HandGestureData>> LoadLeftHandGestureData;
    /// <summary>
    /// Event triggered when right hand gesture data is loaded.
    /// Subscribers can listen to this event to receive the loaded gesture data.
    /// </summary>
    public static event Action<List<HandGestureData>> LoadRightHandGestureData;

    /// <summary>
    /// Called when the script instance is being loaded.
    /// Initializes the loading process for gesture data.
    /// </summary>
    void Start()
    {
        LoadData();
    }

    /// <summary>
    /// Loads gesture data from JSON files and invokes the corresponding events.
    /// Reads data for both left and right hands, deserializes it, and notifies subscribers.
    /// </summary>
    /// <remarks>
    /// If a file is missing or deserialization fails, an error is logged, and the event may not be invoked.
    /// Ensure HandGestureData class is properly serializable.
    /// </remarks>
    private void LoadData()
    {
        // Initialize lists for gesture data
        List<HandGestureData> leftHandGesture = new List<HandGestureData>();
        List<HandGestureData> rightHandGesture = new List<HandGestureData>();


        try
        {

            // Load and deserialize left hand gesture data
            //string leftHandGestureDataJson = File.ReadAllText(Application.dataPath + "/Scripts/leftdata.json");
            //string leftHandGestureDataJson = File.ReadAllText(Application.dataPath + "/Resources/leftdata.json");
            //leftHandGesture = JsonHelper.FromJson<HandGestureData>(leftHandGestureDataJson);

            // Load and deserialize right hand gesture data
            //string rightHandGestureDataJson = File.ReadAllText(Application.dataPath + "/Scripts/rightdata.json");
            //string rightHandGestureDataJson = File.ReadAllText(Application.dataPath + "/Resources/rightdata.json");
            //rightHandGesture = JsonHelper.FromJson<HandGestureData>(rightHandGestureDataJson);

            TextAsset jsonFileLeft = Resources.Load<TextAsset>("leftdata");
            if(jsonFileLeft != null)
            {
                string leftHandGestureDataJson = jsonFileLeft.text;
                leftHandGesture = JsonHelper.FromJson<HandGestureData>(leftHandGestureDataJson);
                // Invoke events to notify subscribers
                LoadLeftHandGestureData?.Invoke(leftHandGesture);
            }


            TextAsset jsonFileRight = Resources.Load<TextAsset>("rightdata");
            if(jsonFileRight != null)
            {
                string rightHandGestureDataJson = jsonFileRight.text;
                rightHandGesture = JsonHelper.FromJson<HandGestureData>(rightHandGestureDataJson);
                // Invoke events to notify subscribers
                LoadRightHandGestureData?.Invoke(rightHandGesture);
            }

            
            

            Debug.Log($"Successfully loaded {leftHandGesture.Count} left hand gestures and {rightHandGesture.Count} right hand gestures.");
        }
        catch (FileNotFoundException ex)
        {
            Debug.LogError($"Gesture data file not found: {ex.FileName}. Ensure JSON files exist in the Scripts folder.");
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to load gesture data: {ex.Message}. Check JSON format and file paths.");
        }
    }
}
