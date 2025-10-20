using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus.Interaction;
using Oculus.Interaction.Input;

public class EquationManager : MonoBehaviour
{
    [SerializeField] private GameObject successPanel;
    [SerializeField] private GameObject failurePanel;
    [SerializeField] private float disableDelay = 2f;
    private int tokenCount;
    private int answerTokenCount = 6;
    private bool isFinished = false;



    // Start is called before the first frame update
    void Start()
    {
        tokenCount = 0;

    }

    private void OnEnable()
    {
        TokenCounter.ConveyTokenCount += UpdateTokenCount;
    }

    private void OnDisable()
    {
        TokenCounter.ConveyTokenCount -= UpdateTokenCount;
    }

    private void UpdateTokenCount(int _tokenCount)
    {
       tokenCount = _tokenCount;
    }
    
    public void OnSelectedButton()
    {
        if (!isFinished)
        {
            if (tokenCount == answerTokenCount)
            {
                isFinished = true;
                successPanel.SetActive(true);
            }
            else
            {
                failurePanel.SetActive(true);
                StartCoroutine(DeactiveAfterDelay());
            }
        }
    }

    IEnumerator DeactiveAfterDelay()
    {
        yield return new WaitForSeconds(disableDelay);
        failurePanel.SetActive(false);
        Debug.Log("failurePanel is disactive");
    }




}
