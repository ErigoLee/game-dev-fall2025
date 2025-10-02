using Oculus.Interaction;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;


//Recognition / logic enum (still exists, but the save class does not reference it)
public enum GestureType 
{ 
    None, 
    Rock, 
    Paper, 
    Scissors
    // Add more gestuers as needed
} 

//For recognition / runtime logic
public class HandGesture 
{
    
    public GestureType gestureType; 
    
    private List<Vector3> jointPositions;

    public HandGesture(GestureType _gestureType, List<Vector3> _jointPositions)
    {
        gestureType = _gestureType;
        jointPositions = _jointPositions;
    }

}

//For saving only: separted from enum (ideal for serialization/JSON/asset storage)
[Serializable]
public class HandGestureDate
{
    public string name;
    public List<Vector3> jointPositions;

    public HandGestureDate(string _name, List<Vector3> _jointPositions)
    {
        name = _name;
        jointPositions = _jointPositions;
    }
}

[System.Serializable]
public class Data
{
    public string name;
    public List<Vector3> jointPositions;

    public Data()
    {
        jointPositions = new List<Vector3>();
    }

}


public static class JsonHelper
{
    public static List<T> FromJson<T> (string json)
    {
        Wrapper<T> wrapper = new Wrapper<T> ();
        return wrapper.items;
    }

    public static string ToJson<T>(List<T> array)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.items = array;
        return JsonUtility.ToJson (wrapper);
    }

    public static string ToJson<T>(List<T> array, bool prettyPrint)
    {
        Wrapper<T> wrapper =new Wrapper<T>();
        wrapper.items = array;
        return JsonUtility.ToJson (wrapper, prettyPrint);
    }

    [System.Serializable]
    private class Wrapper<T>
    {
        public List<T> items;
    }
}