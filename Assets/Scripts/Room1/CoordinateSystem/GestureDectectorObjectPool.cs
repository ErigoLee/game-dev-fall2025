using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestureDectectorObjectPool : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject spawnPointObj;
    private Vector3 spawnPos;
    private Vector3 spawnPos2;
    private Quaternion spawnRot;
    [SerializeField] ObjectPool lightBlueObjectPool;
    [SerializeField] ObjectPool redObjectPool;
    [SerializeField] ObjectPool orangeObjectPool;
    private float m_DistanceLimit = 1.25f;

    private void Start()
    {
        spawnPos = spawnPointObj.transform.position;
        spawnRot = spawnPointObj.transform.rotation;
        spawnPos2 = spawnPointObj.transform.position;
        spawnPos2.y = 0f;
    }


    public void GestureRecognized(GestureType gestureType)
    {
        Vector3 playerPos = player.transform.position;
        playerPos.y = 0f;
        
        float distance = Vector3.Distance(playerPos, spawnPos2);

        if(distance <= m_DistanceLimit)
        {
            switch(gestureType)
            {
                case GestureType.Rock:
                    PooledObject orangePooledObject = orangeObjectPool.GetPooledObject();
                    if (orangePooledObject != null)
                    {
                        GameObject orangeObj = orangePooledObject.gameObject;
                        orangeObj.SetActive(true);
                        orangeObj.transform.SetPositionAndRotation(spawnPos, spawnRot);
                    }
                    break;
                case GestureType.Paper:
                    PooledObject redPooledObject = redObjectPool.GetPooledObject();
                    if(redPooledObject != null)
                    {
                        GameObject redObj = redPooledObject.gameObject;
                        redObj.SetActive(true);
                        redObj.transform.SetPositionAndRotation(spawnPos, spawnRot);
                    }
                    break;
                case GestureType.Scissors:
                    PooledObject lightBluePooledObject = lightBlueObjectPool.GetPooledObject();        
                    if(lightBluePooledObject != null)
                    {
                        GameObject lightBlueObj = lightBluePooledObject.gameObject;
                        lightBlueObj.SetActive(true);
                        lightBlueObj.transform.SetPositionAndRotation(spawnPos, spawnRot);
                    }
                    break;
            }
        }

        
    }
}
