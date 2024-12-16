using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemData
{
    public string itemName; // Name of the item
    public string itemTag; // Layer of the item
    public string layerName; // Name of the items layer
    public int itemPrice;   // Price of the item
    public bool isAvailable = false; // Availability flag
}
