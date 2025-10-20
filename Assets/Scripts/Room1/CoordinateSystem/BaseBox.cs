using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseBox : MonoBehaviour
{
    public string BoxName { get; set; }
    public Vector3 InitPos { get; protected set; }
    public Quaternion InitRot { get; protected set; }
    public abstract void ReleaseObjectPool();
}
