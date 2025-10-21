using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script manages gesture recognition and spawns objects from object pools based on the recognized gesture type.
/// It checks if the player is within a distance limit from the spawn point before spawning.
/// </summary>
public class GestureDectectorObjectPool : MonoBehaviour
{
    /// <summary>
    /// Reference to the player GameObject, used to check the player's position.
    /// </summary>
    [SerializeField] private GameObject player;

    /// <summary>
    /// Reference to the spawn point GameObject, which determines the position and rotation for spawning objects.
    /// </summary>
    [SerializeField] private GameObject spawnPointObj;

    /// <summary>
    /// The position where objects will be spawned, derived from spawnPointObj.
    /// </summary>
    private Vector3 spawnPos;

    /// <summary>
    /// A modified spawn position with y-coordinate set to 0, used for distance calculations.
    /// </summary>
    private Vector3 spawnPos2;

    /// <summary>
    /// The rotation for spawned objects, derived from spawnPointObj.
    /// </summary>
    private Quaternion spawnRot;

    /// <summary>
    /// Object pool for light blue objects.
    /// </summary>
    [SerializeField] ObjectPool lightBlueObjectPool;

    /// <summary>
    /// Object pool for red objects.
    /// </summary>
    [SerializeField] ObjectPool redObjectPool;

    /// <summary>
    /// Object pool for orange objects.
    /// </summary>
    [SerializeField] ObjectPool orangeObjectPool;

    /// <summary>
    /// The maximum distance from the spawn point where the player must be to allow spawning.
    /// </summary>
    private float m_DistanceLimit = 1.25f;

    // Flag indicating whether the current process has been completed
    private bool isFinished = false;

    /// <summary>
    /// Initializes spawn positions and rotation based on the spawn point object.
    /// </summary>
    private void Start()
    {
        spawnPos = spawnPointObj.transform.position;
        spawnRot = spawnPointObj.transform.rotation;
        spawnPos2 = spawnPointObj.transform.position;
        spawnPos2.y = 0f; // Flatten y-coordinate for 2D distance calculation
    }

    private void OnEnable()
    {
        // Subscribe to the IsCompleted event from CoordinateManager
        // When the event is triggered, the IsFinished method will be called
        CoordinateManager.IsCompleted += IsFinished;
    }

    private void OnDisable()
    {
        // Unsubscribe from the IsCompleted event to prevent memory leaks
        CoordinateManager.IsCompleted -= IsFinished;
    }

    private void IsFinished(bool _isFinished)
    {
        // Update the local isFinished flag based on the event value
        isFinished = _isFinished;
    }


    /// <summary>
    /// Called when a gesture is recognized. Spawns an object from the appropriate pool if the player is close enough to the spawn point.
    /// </summary>
    /// <param name="gestureType">The type of gesture recognized (Rock, Paper, or Scissors).</param>
    public void GestureRecognized(GestureType gestureType)
    {
        // Exit early if the process has already finished
        if (isFinished)
            return;

        // Get the player's position and flatten y-coordinate for distance check
        Vector3 playerPos = player.transform.position;
        playerPos.y = 0f;

        // Calculate distance between player and spawn point (both flattened to y=0)
        float distance = Vector3.Distance(playerPos, spawnPos2);

        // Only proceed if player is within the distance limit
        if (distance <= m_DistanceLimit)
        {
            // Switch based on gesture type to spawn the corresponding object
            switch (gestureType)
            {
                case GestureType.Rock:
                    // Get an orange object from the pool
                    PooledObject orangePooledObject = orangeObjectPool.GetPooledObject();
                    if (orangePooledObject != null)
                    {
                        GameObject orangeObj = orangePooledObject.gameObject;
                        orangeObj.SetActive(true); // Activate the object
                        orangeObj.transform.SetPositionAndRotation(spawnPos, spawnRot); // Set position and rotation
                    }
                    break;
                case GestureType.Paper:
                    // Get a red object from the pool
                    PooledObject redPooledObject = redObjectPool.GetPooledObject();
                    if(redPooledObject != null)
                    {
                        GameObject redObj = redPooledObject.gameObject;
                        redObj.SetActive(true); // Activate the object
                        redObj.transform.SetPositionAndRotation(spawnPos, spawnRot); // Set position and rotation
                    }
                    break;
                case GestureType.Scissors:
                    // Get a light blue object from the pool
                    PooledObject lightBluePooledObject = lightBlueObjectPool.GetPooledObject();        
                    if(lightBluePooledObject != null)
                    {
                        GameObject lightBlueObj = lightBluePooledObject.gameObject;
                        lightBlueObj.SetActive(true); // Activate the object
                        lightBlueObj.transform.SetPositionAndRotation(spawnPos, spawnRot); // Set position and rotation
                    }
                    break;
            }
        }

        
    }
}
