using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FingerImageSwitcher : MonoBehaviour
{
    // Reference to the UI Image component that will display the hand sprite
    [SerializeField] private Image image;

    // Array of sprites representing different hand gestures (e.g., rock, scissors, paper)
    [SerializeField] private Sprite[] handSprites;

    // Time interval (in seconds) before switching to the next sprite
    private float interval = 1.0f;

    // Index of the current sprite in the array
    private int idx = 0;

    // Flag to prevent multiple coroutine calls at once
    private bool flag = false;


    // Update is called once per frame
    void Update()
    {
        // If the coroutine is not currently running,
        // and the Image component exists,
        // and the sprite array is not empty
        if (!flag && image != null && handSprites.Length>=0)
        {
            // Display the current sprite
            image.sprite = handSprites[idx];
            // Set the flag to true to prevent starting multiple coroutines
            flag = true;

            // Start the coroutine that waits for 'interval' seconds
            // before switching to the next sprite
            StartCoroutine(Run());
        }
    }

    // Coroutine that waits for a specified interval before switching to the next sprite
    private IEnumerator Run()
    {
        // Wait for the given number of seconds
        yield return new WaitForSeconds(interval);

        // Allow next update cycle to trigger a new change
        flag = false;

        // Move to the next sprite index
        idx++;

        // If the index exceeds the array length, loop back to the first sprite
        if (idx >= handSprites.Length)
            idx = 0;
    }
}
