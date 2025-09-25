using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumBalloon : MonoBehaviour
{
    [SerializeField] private int bollon_num = 1;
    // Start is called before the first frame update
    public int GetBallonNum()
    {
        return bollon_num; 
    }
}
