using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus.Interaction.Input;
using Oculus.Interaction;

public class HandJointSave : MonoBehaviour
{
    [SerializeField] private Hand leftHand;

    private List<Vector3> pos = new List<Vector3>();

    private List<Vector3> checkingpos = new List<Vector3>();

    private float threshold = 0.05f;

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (leftHand.IsTrackedDataValid)
            {
                pos.Clear();
                foreach (HandJointId joint in System.Enum.GetValues(typeof(HandJointId)))
                {
                    try
                    {
                        if (leftHand.GetJointPose(joint, out Pose pose))
                        {
                            Vector3 localPosition = leftHand.transform.InverseTransformPoint(pose.position);
                            pos.Add(localPosition);
                            Debug.Log($"HandJointId: {joint.ToString()}");

                        }
                        else
                        {
                            Debug.Log($"Failed to get pose for joint: {joint.ToString()}");
                        }
                    }
                    catch (System.IndexOutOfRangeException ex)
                    {
                        Debug.LogError($"IndexOutOfRangeException for joint {joint.ToString()} : {ex.Message}. Skipping this joint.");
                        continue;
                    }
                    
                }

                ReadingPose();
            }
        }

        if(Input.GetKeyDown(KeyCode.T))
        {
            if (leftHand.IsTrackedDataValid)
            {
                checkingpos.Clear();
                foreach(HandJointId joint in System.Enum.GetValues(typeof(HandJointId)))
                {
                    try
                    {
                        if (leftHand.GetJointPose(joint, out Pose pose))
                        {
                            Vector3 localPosition = leftHand.transform.InverseTransformPoint(pose.position);
                            checkingpos.Add(localPosition);
                            Debug.Log($"HandJointId: {joint.ToString()}");

                        }
                        else
                        {
                            Debug.Log($"Failed to get pose for joint: {joint.ToString()}");
                        }
                    }
                    catch (System.IndexOutOfRangeException ex)
                    {
                        Debug.LogError($"IndexOutOfRangeException for joint {joint.ToString()} : {ex.Message}. Skipping this joint.");
                        continue;
                    }

                }

                string result = ComparerPose();
                Debug.Log("result: "+result);
            }
        }
        
    }

    public void ReadingPose()
    {
        int i = 0;
        foreach(Vector3 _pos in pos)
        {
            Debug.Log(i+": x-"+_pos.x+" y-"+_pos.y+" z-"+_pos.z);
            i++;
        }

    }

    public string ComparerPose()
    {
        float threshold = 0.05f;

        if (pos.Count == checkingpos.Count)
        {
            for(int i = 0;i<pos.Count;i++)
            {
                Vector3 _pos = pos[i];
                Vector3 _checkingpose = checkingpos[i];

                float distance = Vector3.Distance(_checkingpose, _pos);
                if(distance > threshold)
                {
                    return "fail";
                }

            }
            return "success";
        }
        else
        {
            return "fail";
        }
       
       
    }


}
