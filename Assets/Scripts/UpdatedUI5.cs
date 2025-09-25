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
    
}
