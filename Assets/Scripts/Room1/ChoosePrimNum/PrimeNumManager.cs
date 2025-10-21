using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PrimeNumManager : MonoBehaviour
{
    [SerializeField] private GameObject questExclamation;
    [SerializeField] private GameObject puzzleObj;
    private bool isFinished = false;
    private Dictionary<int,bool> clickedPrimeNum = new Dictionary<int, bool> ();
    void Start()
    {
        clickedPrimeNum.Clear ();
        clickedPrimeNum.Add(2, false);
        clickedPrimeNum.Add(3, false);
        clickedPrimeNum.Add(7, false);
        clickedPrimeNum.Add(11, false);
    }

    
    void Update()
    {
        if (!isFinished)
        {
            bool allTrue = clickedPrimeNum.Values.All(v => v);
            if (allTrue)
            {
                isFinished = true;
                questExclamation.SetActive(false);
                puzzleObj.SetActive(true);
            }
        }
    }

    public void PressedPrimBalloonNum(int _num)
    {
        if (!clickedPrimeNum[_num])
        {
            clickedPrimeNum[_num] = true;
        }
    }
}
