using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetPositionOnGroundToken : MonoBehaviour
{
    private Vector3 initPos;
    private Quaternion initRot;
    private Vector3 parentPos;
    private Quaternion parentRot;
    void Start()
    {
        initPos = this.transform.position;
        initRot = this.transform.rotation;
        parentPos = this.transform.parent.position;
        parentRot = this.transform.parent.rotation;
    }

    void Update()
    {
        Vector3 currentPos = this.transform.position;

        if(currentPos.y < 0)
        {

            this.transform.position = parentPos;
            this.transform.rotation = parentRot;
            this.transform.parent.position = parentPos;
            this.transform.parent.rotation = parentRot;
        }
    }
}
