using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PooledObject))]
public class OrangeBox : BaseBox
{
    [SerializeField]
    private string m_BoxName = "Orange";
    public override string BoxName { get => m_BoxName; set => m_BoxName = value; }

    // reference to the PooledObject component so we can return to the pool
    private PooledObject pooledObject;

    private void Awake()
    {
        pooledObject = GetComponent<PooledObject>();
    }

    private void Start()
    {
        InitPos = transform.position;
        InitRot = transform.rotation;
    }

    void Update()
    {
        Vector3 position = transform.position;
        if (position.y <= 0.3f)
        {
            ReleaseObjectPool();
        }
    }

    public override void ReleaseObjectPool()
    {
        transform.position = InitPos;
        transform.rotation = InitRot;

        // Reset the moving Rigidbody
        Rigidbody rBody = GetComponent<Rigidbody>();
        rBody.velocity = new Vector3(0f, 0f, 0f);
        rBody.angularVelocity = new Vector3(0f, 0f, 0f);
        // Release the orangeBox back to the pool
        pooledObject.Release();
    }
}
