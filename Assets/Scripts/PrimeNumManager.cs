using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrimeNumManager : MonoBehaviour
{
    [SerializeField] private GameObject successPanel;
    private bool isFinished = false;
    private Dictionary<int,bool> clickedNum = new Dictionary<int, bool> ();
    void Start()
    {
        clickedNum.Clear ();
        clickedNum.Add(2, false);
        clickedNum.Add(3, false);
        clickedNum.Add(7, false);
        clickedNum.Add(11, false);
    }

    
    void Update()
    {
        
    }
}
