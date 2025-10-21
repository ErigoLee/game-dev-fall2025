using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoordinateManager : MonoBehaviour
{
    [SerializeField] private GameObject questExclamation;
    [SerializeField] private GameObject puzzleObj;
    [SerializeField] private GameObject arrow;
    public static event Action<bool> IsCompleted;
    private int clearedProblemCount = 0;
    private int maxProblemCount = 8;
    private bool isFinished = false;

    void Start()
    {
        
    }

    void Update()
    {
        if (isFinished)
            return;

        if(clearedProblemCount>=maxProblemCount)
        {
            isFinished = true;
            questExclamation.SetActive(false);
            arrow.SetActive(false);
            puzzleObj.SetActive(true);
            IsCompleted?.Invoke(isFinished);
        }
    }

    private void OnEnable()
    {
        CoordinateColliderController.OnCleared += HandleProblemCleared;
    }

    private void OnDisable()
    {
        CoordinateColliderController.OnCleared -= HandleProblemCleared;
    }

    private void HandleProblemCleared(UnityEngine.Object o)
    {
        clearedProblemCount++;
    }
}
