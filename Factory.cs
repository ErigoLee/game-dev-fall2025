using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Factory : MonoBehaviour
{
    // Abstract method to get a product instance.
    public abstract IProduct GetProduct(Vector3 position);

    // Shared method with all factories.
    public string GetLog(IProduct product)
    {
        string logMessage = "Factory: created product " + product.ProductName;
        return logMessage;
    }
}