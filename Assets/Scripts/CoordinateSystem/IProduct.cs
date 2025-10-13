using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IProduct
{
    // add common properties and methods here
    public string ProductName { get; set; }

    // customize this for each concrete product
    public void Initialize();
}
