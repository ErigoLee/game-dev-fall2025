using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    // initial number of cloned objects
    [SerializeField] private uint initPoolSize;
    public uint InitPoolSize => initPoolSize;

    // PooledObject prefab
    [SerializeField] private PooledObject objectToPool;

    // store the pooled objects in stack
    private Stack<PooledObject> stack;

    private void Start()
    {
        SetupPool();
    }

    // creates the pool (invoke when the lag is not noticeable)
    private void SetupPool()
    {
        // missing objectToPool Prefab field
        if (objectToPool == null)
        {
            return;
        }

        stack = new Stack<PooledObject>();

        // populate the pool
        PooledObject instance = null;

        for (int i = 0; i < initPoolSize; i++)
        {
            instance = Instantiate(objectToPool);
            instance.Pool = this;
            instance.gameObject.SetActive(false);
            stack.Push(instance);
        }
    }

    // returns the first active GameObject from the pool
    public PooledObject GetPooledObject()
    {
        // missing objectToPool field
        if (objectToPool == null)
        {
            return null;
        }

        //If stack == 0, it means there are no reusable objects available, so the method returns null.
        //A new PooledObject is not instantiated to prevent infinite object creation.
        if (stack.Count == 0)
        {
            // if the pool is not large enough, instantiate extra PooledObjects
            //PooledObject newInstance = Instantiate(objectToPool);
            //newInstance.Pool = this;
            //return newInstance;

            //Return null if no objects are available (to prevent infinite growth)
            return null;
        }

        // otherwise, just grab the next one from the list
        PooledObject nextInstance = stack.Pop();
        nextInstance.gameObject.SetActive(true);
        return nextInstance;
    }

    // returns the GameObject to the pool
    public void ReturnToPool(PooledObject pooledObject)
    {
        if (pooledObject != null)
        {
            stack.Push(pooledObject);
            pooledObject.gameObject.SetActive(false);
        }
        
    }
}
