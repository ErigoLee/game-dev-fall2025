using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpdatedUI5 : MonoBehaviour  
{
    private bool isSelecting;
    // Start is called before the first frame update
    void Start()
    {
        isSelecting = false;
    }

    public void PressedPrimeNumber()
    {
        if(!isSelecting)
        {
            GetComponent<TextMeshProUGUI>().text = "You selected a prime number ballon";
            isSelecting = true;
        }
        
    }

    public void UnPressedNumber()
    {
        if (isSelecting)
        {
            GetComponent<TextMeshProUGUI>().text = "You did not select anything";
            isSelecting = false;
        }
        
    }

    public void PressedCompositeNumber()
    {
        if (!isSelecting)
        {
            GetComponent<TextMeshProUGUI>().text = "You selected a composite number ballon";
            isSelecting = true;
        }
    }


    //checking
    public void number2()
    {
        Debug.Log("2 balloon poke");
    }

    public void number3()
    {
        Debug.Log("3 balloon poke");
    }
    public void number4()
    {
        Debug.Log("4 balloon poke");
    }
    public void number6()
    {
        Debug.Log("6 balloon poke");
    }
    public void number7()
    {
        Debug.Log("7 balloon poke");
    }

    public void number8()
    {
        Debug.Log("8 balloon poke");
    }

    public void number9()
    {
        Debug.Log("9 balloon poke");
    }

    public void number11()
    {
        Debug.Log("11 balloon poke");
    }

    public void number12()
    {
        Debug.Log("12 balloon poke");
    }
}
