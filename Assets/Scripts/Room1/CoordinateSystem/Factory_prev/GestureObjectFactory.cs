using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestureObjectFactory : MonoBehaviour
{
    [SerializeField] private GameObject spawnPointObj;
    private Vector3 spawnPoint;
    [SerializeField] private GameObject player;
    [SerializeField] private Factory[] m_factories;

    private float m_DistanceLimit = 1.25f;

    // List to track all created products
    private List<GameObject> m_CreatedProducts = new List<GameObject>();

    private void Start()
    {
        spawnPoint = spawnPointObj.transform.position;
    }

    public void GestureRecognized(GestureType gestureType)
    {
        Vector3 playerPos = player.transform.position;
        playerPos.y = 0;
        Vector3 spawnPos = spawnPoint;
        spawnPos.y = 0;
        float distance = Vector3.Distance(playerPos,spawnPos);
        if(GestureType.Scissors == gestureType && distance <= m_DistanceLimit)
        {
            Factory selectedFactory = m_factories[0];
            IProduct product = selectedFactory.GetProduct(spawnPoint);
            // Add the GameObject of the created product to the list
            if (product is Component component)
            {
                m_CreatedProducts.Add(component.gameObject);
            }

        }

    }


    private void OnDestroy()
    {
        foreach (GameObject product in m_CreatedProducts)
        {
            Destroy(product);
        }
        // Clear the list when the object is destroyed
        m_CreatedProducts.Clear();
    }
}
