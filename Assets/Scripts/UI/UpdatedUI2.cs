using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpdatedUI2 : MonoBehaviour
{

    void UpdateConText(object c)
    {
        GetComponent<TextMeshProUGUI>().text = "Congrats! An item was put into the container.";
    }
    
}
