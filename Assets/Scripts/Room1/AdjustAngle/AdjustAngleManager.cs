using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdjustAngleManager : MonoBehaviour
{
    [SerializeField] private GameObject playerObj;
    [SerializeField] private GameObject basePositionObj;
    [SerializeField] private GameObject successPanel;
    private Vector3 playerPos;
    Vector3 basePositionPos;
    private bool isFinished = false;
    float threshold = 1.5f;

    void Start()
    {
        basePositionPos = basePositionObj.transform.position;
        basePositionPos.y = 0f;
    }


    private void OnEnable()
    {
        HandGestureRecognizer.ConveyRightHandGesture += CheckAnswerGesture;
    }

    private void OnDisable()
    {
        HandGestureRecognizer.ConveyRightHandGesture -= CheckAnswerGesture;
    }

    void Update()
    {
        playerPos = playerObj.transform.position;
        playerPos.y = 0f;
        float distance = Vector3.Distance(playerPos, basePositionPos);
         
    }

    private void CheckAnswerGesture(HandGestureData currentRightGesture, HandGestureData prevRightGesture)
    {
        if (isFinished)
            return;

        playerPos = playerObj.transform.position;
        playerPos.y = 0f;
        float distance = Vector3.Distance(playerPos, basePositionPos);
        if (distance <= threshold && !string.Equals(currentRightGesture.name, prevRightGesture.name))
        {
            if(string.Equals(currentRightGesture.name,"Four"))
            {
                isFinished = true;
                successPanel.SetActive(true);
            }
        }
    }
}
