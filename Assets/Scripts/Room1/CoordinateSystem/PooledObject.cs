using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents an individual object managed by an ObjectPool.
/// Each PooledObject keeps a reference to the pool it belongs to,
/// allowing it to be returned to the pool when it is no longer needed.
/// </summary>
public class PooledObject : MonoBehaviour
{
    /// <summary>
    /// Reference to the ObjectPool that manages this object.
    /// </summary>
    private ObjectPool pool;

    /// <summary>
    /// Public property for accessing or assigning the pool reference.
    /// </summary>
    public ObjectPool Pool { get => pool; set => pool = value; }

    /// <summary>
    /// Releases this object back to its associated pool.
    /// Logs the current pool reference for debugging before returning it.
    /// </summary>
    public void Release()
    {
        Debug.Log("Pool is "+pool);
        pool.ReturnToPool(this);
    }
}
