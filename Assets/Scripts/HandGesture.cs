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
        jointPositions = new List<Vector3>(_jointPositions);
    }

}

//For saving only: separted from enum (ideal for serialization/JSON/asset storage)
//Used when the gesture type does not yet have a name assigned.
[Serializable]
public class HandGestureData
{
    public string name;
    public List<Vector3> jointPositions;

    public HandGestureData(string _name, List<Vector3> _jointPositions)
    {
        name = _name;
        jointPositions = new List<Vector3>(_jointPositions);
    }

    public HandGestureData()
    {
        name = "";
        jointPositions = new List<Vector3>();
    }

    public HandGestureData(List<Vector3> _jointPositions)
    {
        name = "";
        jointPositions = new List<Vector3>(_jointPositions);
    }
}


/// <summary>
/// Change the JSON data into a List<T> object.
/// </summary>
/// <typeparam name="T">The type of objects in the list.</typeparam>
/// <param name="json">The JSON string to deserialize.</param>
/// <returns>A list of objects of type T, or an empty list if deserialization fails.</returns>
public static class JsonHelper
{
    /// Change the JSON data into a Wrapper<T> object
    public static List<T> FromJson<T> (string json)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.items;
    }

    /// Turn a List<T> into a JSON string
    public static string ToJson<T>(List<T> array)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.items = array;
        return JsonUtility.ToJson (wrapper);
    }

    /// Turn a List<T> into JSON, with pretty formatting (line breaks and tabs)
    public static string ToJson<T>(List<T> array, bool prettyPrint)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.items = array;
        return JsonUtility.ToJson(wrapper, prettyPrint);
    }

    /// <summary>
    /// A private generic wrapper class used internally.
    /// Unity's JsonUtilty cannot directly serialize or deserialize top-level lists, 
    /// so we wrap the list in a class with a single field "items".
    /// </summary>

    [System.Serializable]
    private class Wrapper<T>
    {
        public List<T> items;
    }
}