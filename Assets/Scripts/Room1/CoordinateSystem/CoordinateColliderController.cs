using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// This script controls a collider that responds to interactions with BaseBox objects based on specific coordinate conditions.
/// When a BaseBox enters the trigger, it checks the box's name and the controller's position to determine if the answerCube should be activated.
/// After processing, it releases the box back to its object pool.
/// </summary>
public class CoordinateColliderController : MonoBehaviour
{
    /// <summary>
    /// The x-coordinate position for this collider, adjustable in the Unity editor with a range from -7 to 7.
    /// </summary>
    [Range(-7,7)]
    [SerializeField] private int xPos;

    /// <summary>
    /// The y-coordinate position for this collider, adjustable in the Unity editor with a range from -7 to 7.
    /// </summary>
    [Range(-7,7)]
    [SerializeField] private int yPos;

    /// <summary>
    /// The cube object that gets activated when the correct conditions are met.
    /// </summary>
    [SerializeField] private GameObject answerCube;

    /// <summary>
    /// Flag to track if the interaction has been completed.
    /// </summary>
    private bool isFinished = false;

    /// <summary>
    /// Event invoked when the interaction is cleared (isFinished becomes true).
    /// Allows other scripts to subscribe and react to the clear event.
    /// </summary>
    public static event Action<CoordinateColliderController> OnCleared; // UnityEvent for easy editor assignment
    
    /// <summary>
    /// Called when another collider enters this trigger collider.
    /// Checks if the entering object is a BaseBox and evaluates specific coordinate and name conditions.
    /// </summary>
    /// <param name="other">The collider that entered the trigger.</param>
    private void OnTriggerEnter(Collider other)
    {
        // Attempt to get the BaseBox component from the colliding object
        BaseBox box = other.GetComponent(typeof(BaseBox)) as BaseBox;
        if (box != null)
        {
            // Log the box's name for debugging
            Debug.Log("BaseBox: "+box.BoxName);

            // Release the box back to its object pool after processing
            box.ReleaseObjectPool();

            // Exit early if the process is already finished
            if (isFinished)
                return;

            // Check conditions for "LightBlue" box at specific positions
            if (((xPos == 1 && yPos == 1) || (xPos == 4 && yPos == 1) || (xPos == 1 && yPos==4))&&(string.Equals(box.BoxName, "LightBlue")))
            {
                answerCube.SetActive(true); // Activate the answer cube
                isFinished = true; // Mark as finished
                OnCleared?.Invoke(this); // Notify subscribers that clearing occurred
            }

            // Check conditions for "Orange" box at specific positions
            if (((xPos == -3 && yPos == -1) || (xPos == -6 && yPos == -4) || (xPos == -3 && yPos == -7)) && (string.Equals(box.BoxName, "Orange")))
            {
                answerCube.SetActive(true); // Activate the answer cube
                isFinished = true; // Mark as finished
                OnCleared?.Invoke(this); // Notify subscribers that clearing occurred
            }

            // Check conditions for "Red" box at specific positions
            if (((xPos == 2 && yPos == 2) || (xPos == -4 && yPos == -4) ) && (string.Equals(box.BoxName, "Red")))
            {
                answerCube.SetActive(true); // Activate the answer cube
                isFinished = true; // Mark as finished
                OnCleared?.Invoke(this); // Notify subscribers that clearing occurred
            }

            
        }
    }
}
