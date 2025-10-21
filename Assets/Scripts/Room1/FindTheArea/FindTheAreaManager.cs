using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FindTheAreaManager : MonoBehaviour
{
    [SerializeField] private GameObject numberCubeArrow;
    [SerializeField] private GameObject questExclamation;
    [SerializeField] private GameObject puzzleObj;
    private Dictionary<int, bool> hasSolvedQustions = new Dictionary<int, bool>();
    private bool isFinished = false;

    void Start()
    {
        hasSolvedQustions.Clear();
        hasSolvedQustions.Add(1, false);
        hasSolvedQustions.Add(2, false);
    }

    
    void Update()
    {
        if(!isFinished)
        {
            bool allTrue = hasSolvedQustions.Values.All(v => v);
            if (allTrue)
            {
                isFinished = true;
                numberCubeArrow.SetActive(false);
                questExclamation.SetActive(false);
                puzzleObj.SetActive(true);
            }
        }
    }

    public void OnQuestionSolved(int question)
    {
        if (!hasSolvedQustions[question])
        {
            hasSolvedQustions[question] = true;
        }
    }
}
