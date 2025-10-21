using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus.Interaction.Input;
using Oculus.Interaction;

public class HandJointSave : MonoBehaviour
{
    [SerializeField] private Hand leftHand;
    [SerializeField] private Hand rightHand;
    [SerializeField] private bool isLeftHandActive = false;
    [SerializeField] private bool isRightHandActive = false;

    private List<Vector3> pos = new List<Vector3>();

    private List<Vector3> checkingpos = new List<Vector3>();

    private float threshold = 0.05f;


    //This runs automatically whenever values are changed in the Inspector
    private void OnValidate()
    {
        if(isRightHandActive && isLeftHandActive)
        {
#if UNITY_EDITOR
            UnityEditor.SerializedObject so = new UnityEditor.SerializedObject(this);
            UnityEditor.SerializedProperty rightProp = so.FindProperty("isRightHandActive");
            UnityEditor.SerializedProperty leftProp = so.FindProperty("isLeftHandActive");

            //Keep only the last modified property as true
            if (rightProp.prefabOverride)
            {
                isLeftHandActive = false; //Right hand was changed last
            }
            else if (leftProp.prefabOverride)
            {
                isRightHandActive = false;
            }
            so.ApplyModifiedProperties();
#endif
        }
    }

    //Runtime properties (same logic applies when changed in code
    public bool IsRightHandActive
    {
        get => isRightHandActive;
        set
        {
            isRightHandActive = value;
            if (value) isLeftHandActive = false;
        }
    }

    public bool IsLeftHandActive
    {
        get => isLeftHandActive;
        set
        {
            isLeftHandActive = value;
            if (value) isRightHandActive = false;
        }
    }


    void Update()
    {
        if (isRightHandActive)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                StorageRight();
            }
        }
        else if (isLeftHandActive)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                StorageLeft();
            }
        }

        

        if(Input.GetKeyDown(KeyCode.T))
        {
            if (leftHand.IsTrackedDataValid)
            {
                checkingpos.Clear();
                foreach(HandJointId joint in System.Enum.GetValues(typeof(HandJointId)))
                {
                    //Filters out invalid (undefined) values
                    if(!System.Enum.IsDefined(typeof(HandJointId), joint))
                    {
                        //skip if the value is invalid (or print a log message)
                        Debug.LogWarning($"Isvalid HandJointId value: {joint}");
                        continue;
                    }
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


    public void StorageLeft()
    {
        if (leftHand.IsTrackedDataValid)
        {
            pos.Clear();
            foreach (HandJointId joint in System.Enum.GetValues(typeof(HandJointId)))
            {
                //Filters out invalid (undefined) values
                if (!System.Enum.IsDefined(typeof(HandJointId), joint))
                {
                    //skip if the value is invalid (or print a log message)
                    Debug.LogWarning($"Isvalid HandJointId value: {joint}");
                    continue;
                }
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


    public void StorageRight()
    {
        if (rightHand.IsTrackedDataValid)
        {
            pos.Clear();
            foreach (HandJointId joint in System.Enum.GetValues(typeof(HandJointId)))
            {
                //Filters out invalid (undefined) values
                if (!System.Enum.IsDefined(typeof(HandJointId), joint))
                {
                    //skip if the value is invalid (or print a log message)
                    Debug.LogWarning($"Isvalid HandJointId value: {joint}");
                    continue;
                }
                try
                {
                    if (rightHand.GetJointPose(joint, out Pose pose))
                    {
                        Vector3 localPosition = rightHand.transform.InverseTransformPoint(pose.position);
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
