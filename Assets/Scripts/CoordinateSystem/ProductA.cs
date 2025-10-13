using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProductA : MonoBehaviour, IProduct
{
    [SerializeField]
    private string m_ProductName = "ProductA";

    public string ProductName { get => m_ProductName; set => m_ProductName = value; }


    public void Initialize()
    {
        // Add any unique set up logic here
        gameObject.name = m_ProductName;

        /*
        m_ParticleSystem = GetComponentInChildren<ParticleSystem>();

        if (m_ParticleSystem == null)
            return;

        m_ParticleSystem.Stop();
        m_ParticleSystem.Play();
        */
    }
}
