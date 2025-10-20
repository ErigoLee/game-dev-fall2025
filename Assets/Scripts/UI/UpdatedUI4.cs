using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpdatedUI4 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
       
    }

    public void HandePosePos(Vector3 posepos, Quaternion poserot){

        GetComponent<TextMeshProUGUI>().text = "Hand Pose position: " + posepos+", "+poserot;

    }
    
}
