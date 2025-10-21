using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Oculus.Interaction.Input;

public class HandPosePosition : MonoBehaviour
{
    
    [System.Serializable] 
    public class PoseEvent : UnityEvent<Vector3, Quaternion> {};
    [Header("Events")]
    public PoseEvent HandPoseChanged;

    public Hand hand;

    void Update()
    {

        if (hand == null) return;


        if (hand.IsTrackedDataValid && hand.GetRootPose(out Pose rootPose))
        {
            Vector3 pos = rootPose.position;
            Quaternion rot = rootPose.rotation;
            HandPoseChanged?.Invoke(pos, rot);
        }

    }
}
