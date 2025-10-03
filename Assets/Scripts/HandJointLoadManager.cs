using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class HandJointLoadManager : MonoBehaviour
{
    public static event Action<List<HandGestureData>> LoadLeftHandGestureData;
    public static event Action<List<HandGestureData>> LoadRightHandGestureData;

    // Start is called before the first frame update
    void Start()
    {
        LoadData();
    }

    private void LoadData()
    {
        List<HandGestureData> leftHandGesture = new List<HandGestureData>();
        List<HandGestureData> rightHandGesture = new List<HandGestureData>();
        string leftHandGestureDataJson = File.ReadAllText(Application.dataPath + "/Scripts/leftdata.json");
        //Debug.Log("leftHandGestureDataJson: " + leftHandGestureDataJson);
        leftHandGesture = JsonHelper.FromJson<HandGestureData>(leftHandGestureDataJson);

        string rightHandGestureDataJson = File.ReadAllText(Application.dataPath + "/Scripts/rightdata.json");
        //Debug.Log("rightHandGestureDataJson: " + rightHandGestureDataJson);
        rightHandGesture = JsonHelper.FromJson<HandGestureData>(rightHandGestureDataJson);


        /*
        Debug.Log("LeftHandGesture: " + leftHandGesture.Count);
        Debug.Log("RightHandGesture: " + rightHandGesture.Count);

        foreach(HandGestureData handGestureData in leftHandGesture)
        {
            Debug.Log("LeftHandGesture!!: " + handGestureData.name);
        }
        foreach(HandGestureData handGestureData1 in rightHandGesture)
        {
            Debug.Log("RightHandGesture!!: "+ handGestureData1.name);
        }
        */
        LoadLeftHandGestureData.Invoke(leftHandGesture);
        LoadRightHandGestureData.Invoke(rightHandGesture);
        /*
        TextAsset ta = Resources.Load<TextAsset>("Scripts/leftdata");
        if (ta != null)
        {
            Debug.LogError("leftdata.json not found!");
        }
        List<HandGestureData> leftHandGesture = JsonHelper.FromJson<HandGestureData>(ta.text);

        TextAsset ta2 = Resources.Load<TextAsset>("Scripts/rightdata");
        if (ta2 != null)
        {
            Debug.LogError("rightdata.json not found!");
        }
        List<HandGestureData> rightHandGesture = JsonHelper.FromJson<HandGestureData>(ta2.text);
        LoadLeftHandGestureData.Invoke(leftHandGesture);
        LoadRightHandGestureData.Invoke(rightHandGesture);
        */
    }
}
