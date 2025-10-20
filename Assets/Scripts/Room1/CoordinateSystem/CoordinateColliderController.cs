using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoordinateColliderController : MonoBehaviour
{
    [Range(-7,7)]
    [SerializeField] private int xPos;
    [Range(-7,7)]
    [SerializeField] private int yPos;

    [SerializeField] private GameObject answerCube;
    private bool isFinished = false;

    private void OnTriggerEnter(Collider other)
    {
        BaseBox box = other.GetComponent(typeof(BaseBox)) as BaseBox;
        if (box != null)
        {
            if (((xPos == 1 && yPos == 1) || (xPos == 4 && yPos == 1) || (xPos == 1 && yPos==4))&&(string.Equals(box.BoxName, "LightBlue")))
            {
                answerCube.SetActive(true);
                isFinished = true;
            }

            if (((xPos == -3 && yPos == -1) || (xPos == -6 && yPos == -4) || (xPos == -3 && yPos == -7)) && (string.Equals(box.BoxName, "Orange")))
            {
                answerCube.SetActive(true);
                isFinished = true;
            }

            if (((xPos == 2 && yPos == 2) || (xPos == -4 && yPos == -4) ) && (string.Equals(box.BoxName, "Red")))
            {
                answerCube.SetActive(true);
                isFinished = true;
            }

            box.ReleaseObjectPool();
        }
    }
}
