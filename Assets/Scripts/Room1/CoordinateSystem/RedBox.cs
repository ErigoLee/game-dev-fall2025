using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents a red box that inherits from BaseBox.
/// This class handles the lifecycle of the box, including resetting its position and releasing it back to the object pool when it falls below a certain height.
/// </summary>
[RequireComponent(typeof(PooledObject))]
public class RedBox : BaseBox
{
    /// <summary>
    /// The name of the box, set to "Red".
    /// </summary>
    [SerializeField]
    private string m_BoxName = "Red";

    /// <summary>
    /// Overrides the BoxName property from BaseBox.
    /// </summary>
    public override string BoxName { get => m_BoxName; set => m_BoxName = value; }

    // Reference to the PooledObject component so we can return to the pool
    private PooledObject pooledObject;

    /// <summary>
    /// Initializes the pooledObject reference on awake.
    /// </summary>
    private void Awake()
    {
        pooledObject = GetComponent<PooledObject>();
    }

    /// <summary>
    /// Sets the initial position and rotation of the box.
    /// </summary>
    private void Start()
    {
        InitPos = transform.position;
        InitRot = transform.rotation;
    }

    /// <summary>
    /// Updates every frame to check if the box has fallen below the threshold height.
    /// If so, releases it back to the pool.
    /// </summary>
    void Update()
    {
        // Get the current position of the box
        Vector3 position = transform.position;
        // Check if the y-position is below the threshold
        if (position.y <= 0.3f)
        {
            ReleaseObjectPool();
        }
    }

    /// <summary>
    /// Resets the box's position and rotation to initial values, stops its Rigidbody movement, and releases it back to the object pool.
    /// </summary>
    public override void ReleaseObjectPool()
    {
        transform.position = InitPos;
        transform.rotation = InitRot;

        // Reset the moving Rigidbody
        Rigidbody rBody = GetComponent<Rigidbody>();
        rBody.velocity = new Vector3(0f, 0f, 0f);
        rBody.angularVelocity = new Vector3(0f, 0f, 0f);
        // Release the redBox back to the pool
        pooledObject.Release();
    }
}
