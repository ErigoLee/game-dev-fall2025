using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpdatedUI : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
       Cube.cubeEventAct+=UpdateBoxText;
       NumberCube.numCubEventAct+=UpdateNumBoxText;
    }
    void OnDestroy()
    {
       Cube.cubeEventAct-=UpdateBoxText;
       NumberCube.numCubEventAct-=UpdateNumBoxText;
    }
    
    // this method will be called when cube notifies us that it was collected
    void UpdateNumBoxText(object c)
    {
        GetComponent<TextMeshProUGUI>().text = "Congrats on grabbing the numberbox!";
    }

    void UpdateBoxText(object c)
    {
        GetComponent<TextMeshProUGUI>().text = "Congrats on grabbing the box!";
    }

    
    public void GestureRecognized(GestureType gestureType){

        GetComponent<TextMeshProUGUI>().text = "Gesture recognized as: " + gestureType;

    }

    public void PerformGestureNotification(HandGestureData _handGesture, bool isLeft)
    {
        if (isLeft)
        {
            GetComponent<TextMeshProUGUI>().text = "LeftHandGestureData: " + _handGesture.name;
            //Debug.Log("LeftHandGestureData: " + _handGesture.name);
        }
        else
        {
            GetComponent<TextMeshProUGUI>().text = "RightHandGestureData: " + _handGesture.name;
            Debug.Log("RightHandGestureData: " + _handGesture.name);
        }


    }


}
