using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CollectionNotifier : MonoBehaviour
{
    public static event Action<CollectionNotifier> OnColleted;

    private void OnTriggerEnter(Collider other)
    {

        if (OnColleted != null) // check if anyone subscribed to this event
        {
            OnColleted(this); // run the event on all subscribers
        }
    }
}
