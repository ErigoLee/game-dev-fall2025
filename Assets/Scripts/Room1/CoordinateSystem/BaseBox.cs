using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Abstract base class for box objects in the game. This class defines common properties and methods
/// that derived box classes must implement, such as a name, initial position/rotation, and object pool release.
/// </summary>
public abstract class BaseBox : MonoBehaviour
{
    /// <summary>
    /// The name of the box. This property must be implemented by derived classes.
    /// </summary>
    public abstract string BoxName { get; set; }

    /// <summary>
    /// The initial position of the box. This is set in derived classes and is read-only to external code.
    /// </summary>
    public Vector3 InitPos { get; protected set; }

    /// <summary>
    /// The initial rotation of the box. This is set in derived classes and is read-only to external code.
    /// </summary>
    public Quaternion InitRot { get; protected set; }

    /// <summary>
    /// This method returns the object to the object pool so that it can be reused instead of being destroyed.
    /// </summary>
    public abstract void ReleaseObjectPool();
}
