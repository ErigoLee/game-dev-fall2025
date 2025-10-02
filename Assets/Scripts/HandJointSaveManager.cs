using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;

public class HandJointSaveManager : MonoBehaviour
{
    private HandGestureDate newHandGesture;
    private List<HandGestureDate> saveLeftHandGesture;
    private List<HandGestureDate> saveRightHandGesture;


    private void Start()
    {
        saveLeftHandGesture = new List<HandGestureDate>();
        saveRightHandGesture = new List<HandGestureDate>();
    }

    private void OnEnable()
    {
        HandJointRecord.ConveyGestureData += OnGestureDataReceived;
    }


    private void OnDisable()
    {
        HandJointRecord.ConveyGestureData -= OnGestureDataReceived;
    }

    private void OnGestureDataReceived(HandGestureDate _newHandGesture,bool isLeftActive)
    {
        newHandGesture = _newHandGesture;

        if (isLeftActive)
        {
            saveLeftHandGesture.Add(newHandGesture);
        }
        else
        {
            saveRightHandGesture.Add(newHandGesture);
        }

        SaveDate();
       
    }

    private void SaveDate()
    {
        List<Data> data = new List<Data>();
        foreach (HandGestureDate gestureData in saveLeftHandGesture)
        {
            Data _data = new Data();
            _data.name = gestureData.name;
            _data.jointPositions = gestureData.jointPositions;
            data.Add( _data );
        }

        string toJson = JsonHelper.ToJson(data, prettyPrint: true);
        File.WriteAllText(Application.dataPath + "/Scripts/leftdata.json", toJson);

        List<Data> data2 = new List<Data>();
        foreach (HandGestureDate gestureData in saveRightHandGesture)
        {
            Data _data2 = new Data();
            _data2.name = gestureData.name;
            _data2.jointPositions = gestureData.jointPositions;
            data2.Add(_data2);
        }

        string toJson2 = JsonHelper.ToJson(data2, prettyPrint: true);
        File.WriteAllText(Application.dataPath + "/Scripts/rightdata.json", toJson2);

    }
}
