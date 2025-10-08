using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandGestureAction : MonoBehaviour
{
    [SerializeField] private GameObject lookcam;
    [SerializeField] private GameObject player;
    private Vector3 forwardDir;



    private float speed = 1f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ExecuteGestureAction(GestureType gestureType)
    {
        if(gestureType == GestureType.Scissors)
        {
            
            forwardDir = lookcam.transform.forward;
            forwardDir.y = 0;
            forwardDir.Normalize();

            player.transform.Translate(forwardDir * speed * Time.deltaTime);
        }
    }


    public void PerformGestureAction(HandGestureData _handGesture, bool isLeft)
    {
        if (isLeft)
        {
            Debug.Log("LeftHandGestureData: " + _handGesture.name);
        }
        else
        {
            Debug.Log("RightHandGestureData: " + _handGesture.name);
        }
        

    }

    public void ExecuteGoingAction(HandGestureData leftGesture, HandGestureData rightGesture)
    {
        if (leftGesture == null || rightGesture == null) { return; }

        if (string.Equals(leftGesture.name, "One") && string.Equals(rightGesture.name, "One"))
        {
            forwardDir = lookcam.transform.forward;
            forwardDir.y = 0;
            forwardDir.Normalize();

            player.transform.Translate(forwardDir * speed * Time.deltaTime);
        }
    }
}
