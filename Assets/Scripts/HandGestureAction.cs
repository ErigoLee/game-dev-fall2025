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
}
