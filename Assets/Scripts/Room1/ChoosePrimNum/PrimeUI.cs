using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PrimeUI : MonoBehaviour
{
    public void PressedBalloonNum(int num)
    {
        if(num == 2 || num == 3 || num == 7 || num == 11)
        {
            GetComponent<TextMeshProUGUI>().text = "You select a prime number: "+num;
        }
        else
        {
            GetComponent<TextMeshProUGUI>().text = "You select a composite number: "+num;
        }
    }

    public void UnPressedBalloonNum(int num)
    {
        GetComponent<TextMeshProUGUI>().text = "You did not select anything";
    }
     
}
