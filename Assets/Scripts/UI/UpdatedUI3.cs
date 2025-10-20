using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpdatedUI3 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
       
    }

    public void GestureRecognized(GestureType gestureType){

        GetComponent<TextMeshProUGUI>().text = "Gesture recognized as: " + gestureType;

    }
    
}
