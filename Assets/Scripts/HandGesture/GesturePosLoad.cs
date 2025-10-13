using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GesturePosLoad : MonoBehaviour
{
    [SerializeField] private GameObject leftHand;
    [SerializeField] private GameObject rightHand;
    private OVRSkeleton leftSkeleton;
    private OVRSkeleton rightSkeleton;
    private List<OVRBone> leftBones;
    private List<OVRBone> rightBones;
    
    // Start is called before the first frame update
    void Start()
    {
        leftSkeleton = leftHand.GetComponent<OVRSkeleton>();
        rightSkeleton = rightHand.GetComponent<OVRSkeleton>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void rightBonePos()
    {
        rightBones = new List<OVRBone>(rightSkeleton.Bones);
        List<Vector3> pos = new List<Vector3>();
        foreach (OVRBone bone in rightBones)
        {
            //Convert the position pos to the rightHand object’s local coordinate space.
            pos.Add(leftHand.transform.InverseTransformPoint(bone.Transform.position));
        }
        print(pos);
    }

}
