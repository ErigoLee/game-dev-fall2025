using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TokenCounter : MonoBehaviour
{
    private int tokenCount;
    public static event Action<int> ConveyTokenCount;
    private void Start()
    {
        tokenCount = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        ResetPositionOnGroundToken token = other.GetComponentInChildren<ResetPositionOnGroundToken>();
        if(token != null)
        {
            tokenCount++;
            ConveyTokenCount?.Invoke(tokenCount);
            Debug.Log("Token Count: "+tokenCount);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        ResetPositionOnGroundToken token = other.GetComponentInChildren<ResetPositionOnGroundToken>();
        if (token != null)
        {
            tokenCount--;
            if (tokenCount <= 0)
            {
                tokenCount = 0;
            }
            Debug.Log("Token Count: " + tokenCount);
            ConveyTokenCount?.Invoke(tokenCount);

        }
    }
}
