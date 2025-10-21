using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NumCubeDetector : MonoBehaviour
{
    [Range(1,2)]
    [SerializeField] private int question = 1;
    [SerializeField] private GameObject particle;
    [SerializeField] private GameObject answerCube;
    [SerializeField] private GameObject questionPointer;
    [SerializeField] private float disableDelay = 2f;
    private int answer = 0;
    private bool isFinished = false;
    public UnityEvent<int> IsSolve = new UnityEvent<int>();
    

    void Start()
    {
        answer = (question == 1) ? 9 : 3;
    }

    private void OnTriggerEnter(Collider other)
    {
        NumBoxController numcube = other.GetComponentInChildren<NumBoxController>();
        if (numcube != null && !isFinished)
        {
            if ((question == 1 && numcube.GetNumBoxNumber == answer) || (question == 2 && numcube.GetNumBoxNumber == answer))
            {
                isFinished = true;
                IsSolve?.Invoke(question);
                particle.SetActive(true);
                answerCube.SetActive(true);
                questionPointer.SetActive(false);
                numcube.MoveInitalPlace();
                StartCoroutine(DeactiveAfterDelay());

            }
        }
    }

    IEnumerator DeactiveAfterDelay()
    {
        yield return new WaitForSeconds(disableDelay);
        particle.SetActive(false);
    }
}
