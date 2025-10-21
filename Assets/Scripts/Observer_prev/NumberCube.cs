using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class NumberCube : MonoBehaviour 
{
    public static event Action<NumberCube> numCubEventAct;

    private void OnTriggerEnter(Collider other)
    {
        numCubEventAct?.Invoke(this);
    }
}
