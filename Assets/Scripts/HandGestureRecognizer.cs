using Oculus.Interaction.Input;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class HandGestureRecognizer : MonoBehaviour
{
    [SerializeField] private Hand leftHand;
    [SerializeField] private Hand rightHand;
    private List<HandGestureData> leftHandGesture;
    private List<HandGestureData> rightHandGesture;
    private bool isLoadingLeft;
    private bool isLoadingRight;

    private HandGestureData preLeftHandGesture;
    private HandGestureData currentLeftHandGesture;

    private HandGestureData preRightHandGesture;
    private HandGestureData currentRightHandGesture;

    public UnityEvent<HandGestureData, bool> LeftHandGestureAction;
    public UnityEvent<HandGestureData, bool> RightHandGestureAction;

    private float threshold = 0.05f;

    
    void Start()
    {
        leftHandGesture = new List<HandGestureData>();
        rightHandGesture = new List<HandGestureData>();

        preLeftHandGesture = new HandGestureData();
        currentLeftHandGesture = new HandGestureData();
        preRightHandGesture = new HandGestureData();
        currentRightHandGesture = new HandGestureData();

        isLoadingLeft = false;
        isLoadingRight = false;
    }

    private void OnEnable()
    {
        HandJointLoadManager.LoadLeftHandGestureData += LoadLeftHandData;
        HandJointLoadManager.LoadRightHandGestureData += LoadRightHandData;
    }

    private void OnDisable()
    {
        HandJointLoadManager.LoadLeftHandGestureData -= LoadLeftHandData;
        HandJointLoadManager.LoadRightHandGestureData -= LoadRightHandData;
    }

    private void LoadLeftHandData(List<HandGestureData> _leftHandGesture)
    {
        leftHandGesture.Clear();
        leftHandGesture = _leftHandGesture;
        isLoadingLeft = true;
    }

    private void LoadRightHandData(List<HandGestureData> _rightHandGesture)
    {
        rightHandGesture.Clear();
        rightHandGesture= _rightHandGesture;
        isLoadingRight = true;
    }

    void Update()
    {
        if (isLoadingRight && isLoadingLeft)
        {
            GetLeftGestureData();
            GetRightGestureData();
            preLeftHandGesture = currentLeftHandGesture;
            preRightHandGesture = currentRightHandGesture;
        }
        
    }


    private void GetLeftGestureData()
    {
        List<Vector3> collectedData = new List<Vector3>();
        if(leftHand == null)
        {
            Debug.LogError("Attempted to store left hand joints from a null Hand reference.");
            return;
        }
        Debug.Log($"LeftHand IsTrackedDataValid: {leftHand.IsTrackedDataValid}, Hand name: {leftHand.gameObject.name}");
        //if (leftHand.IsTrackedDataValid)
        //{
            collectedData.Clear();

            IEnumerable<HandJointId> allJointIds = System.Enum.GetValues(typeof(HandJointId))
                                                         .Cast<HandJointId>()
                                                         .Where(joint => System.Enum.IsDefined(typeof(HandJointId), joint));
            foreach (HandJointId joint in allJointIds)
            {
                //Filters out invalid (undefined) values
                if (string.Equals(joint.ToString(), "HandEnd") || string.Equals(joint.ToString(), "Invalid"))
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
                        collectedData.Add(localPosition);
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

            currentLeftHandGesture.jointPositions = collectedData;
            LeftHandGestureRecognizer();
        //}
        //else
        //{
            //Debug.LogWarning($"Cannot store joint data for {leftHand.gameObject.name} because its tracking data is not valid.");
        //W}

    }




    private void GetRightGestureData()
    {
        List<Vector3> collectedData = new List<Vector3>();
        if (rightHand == null)
        {
            Debug.LogError("Attempted to store right hand joints from a null Hand reference.");
            return;
        }
        Debug.Log($"LeftHand IsTrackedDataValid: {rightHand.IsTrackedDataValid}, Hand name: {rightHand.gameObject.name}");
        //if (rightHand.IsTrackedDataValid)
        //{
            collectedData.Clear();

            IEnumerable<HandJointId> allJointIds = System.Enum.GetValues(typeof(HandJointId))
                                                         .Cast<HandJointId>()
                                                         .Where(joint => System.Enum.IsDefined(typeof(HandJointId), joint));
            foreach (HandJointId joint in allJointIds)
            {
                //Filters out invalid (undefined) values
                if (string.Equals(joint.ToString(), "HandEnd") || string.Equals(joint.ToString(), "Invalid"))
                {
                    //skip if the value is invalid (or print a log message)
                    Debug.LogWarning($"Isvalid HandJointId value: {joint}");
                    continue;
                }
                try
                {
                    if (rightHand.GetJointPose(joint, out Pose pose))
                    {
                        Vector3 localPosition = leftHand.transform.InverseTransformPoint(pose.position);
                        collectedData.Add(localPosition);
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

            currentLeftHandGesture.jointPositions = collectedData;
            RightHandGestureRecognizer();
        //}
        //else
        //{
            //Debug.LogWarning($"Cannot store joint data for {rightHand.gameObject.name} because its tracking data is not valid.");
        //}


    }

    private void LeftHandGestureRecognizer()
    {
        if (leftHand != null)
        {
            bool isSuccessful = false;
            foreach (HandGestureData gesture in leftHandGesture)
            {
                
                if (gesture.jointPositions.Count == currentLeftHandGesture.jointPositions.Count)
                {
                    bool flag = true;
                    for (int i = 0; i < gesture.jointPositions.Count; i++)
                    {
                        Vector3 _pos = gesture.jointPositions[i];
                        Vector3 _pos2 = currentLeftHandGesture.jointPositions[i];
                        float distance = Vector3.Distance(_pos, _pos2);
                        if(distance > threshold)
                        {
                            flag = false;
                            break;
                        }    
                    }
                    if(flag)
                    {
                        isSuccessful = true;
                        currentLeftHandGesture.name = gesture.name;
                        break;
                    }
                }
            }
            if(isSuccessful)
            {
                if (string.Equals(preLeftHandGesture.name,currentLeftHandGesture.name))
                {
                    LeftHandGestureAction.Invoke(currentLeftHandGesture, true);
                }

            }
        }
    }

    private void RightHandGestureRecognizer()
    {
        if(rightHand != null)
        {
            bool isSuccessful = false;
            foreach (HandGestureData gesture in rightHandGesture)
            {

                if (gesture.jointPositions.Count == currentRightHandGesture.jointPositions.Count)
                {
                    bool flag = true;
                    for (int i = 0; i < gesture.jointPositions.Count; i++)
                    {
                        Vector3 _pos = gesture.jointPositions[i];
                        Vector3 _pos2 = currentRightHandGesture.jointPositions[i];
                        float distance = Vector3.Distance(_pos, _pos2);
                        if (distance > threshold)
                        {
                            flag = false;
                            break;
                        }
                    }
                    if (flag)
                    {
                        isSuccessful = true;
                        currentRightHandGesture.name = gesture.name;
                        break;
                    }
                }
            }
            if (isSuccessful)
            {
                if (string.Equals(preRightHandGesture.name, currentRightHandGesture.name))
                {
                    RightHandGestureAction.Invoke(currentRightHandGesture, false);
                }

            }
        }
    }



}
