using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Oculus.Interaction;
using Oculus.Interaction.Input;

public class PrimeChecker : MonoBehaviour
{
    [SerializeField] private int balloonNum = 0;
    [SerializeField] private GameObject particles;
    [SerializeField] private float disableDelay = 2f;
    private HandGestureData leftHandGesture;
    private HandGestureData rightHandGesture;

    public UnityEvent<int> EnterHandBalloon;
    public UnityEvent<int> ExitHandBalloon;

    private void Start()
    {
        leftHandGesture = new HandGestureData();
        rightHandGesture = new HandGestureData();
    }
    private void OnEnable()
    {
        HandGestureRecognizer.ConveyHandGestures += ObtainHandGesture;
    }

    private void OnDisable()
    {
        HandGestureRecognizer.ConveyHandGestures -= ObtainHandGesture;
    }

    public void ObtainHandGesture(HandGestureData _leftHandGesture, HandGestureData _rightHandGesture)
    {
        leftHandGesture = _leftHandGesture;
        rightHandGesture = _rightHandGesture;
    }

    private void OnTriggerEnter(Collider other)
    {
        HandTransformScaler hand = other.GetComponentInChildren<HandTransformScaler>();
        if (hand != null)
        {
            if (string.Equals(leftHandGesture.name, "One") || string.Equals(rightHandGesture.name, "One"))
            {
                EnterHandBalloon?.Invoke(balloonNum);
                if (balloonNum == 2 || balloonNum == 3 || balloonNum == 7 || balloonNum == 11)
                {
                    if(particles != null)
                    {
                        particles.SetActive(true);
                        StartCoroutine(DeactivateAfterDelay());
                    }
                    
                }
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        HandTransformScaler hand = other.GetComponentInChildren<HandTransformScaler>();

        if (hand != null)
        {
            ExitHandBalloon?.Invoke(balloonNum);
        }
    }

    IEnumerator DeactivateAfterDelay()
    {
        yield return new WaitForSeconds(disableDelay);
        particles.SetActive(false);
        Debug.Log("Object deactivated after delay!");
    }



}
