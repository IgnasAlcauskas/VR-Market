using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    // The name of the item, to match it with what the NPC is requesting
    public string itemName;

    // You can add additional properties like item type, weight, or description if necessary
    public int itemValue;       // The value of the item (optional)

    public bool itemAvailable;  // Is the item available?

    public int itemCost;        // Cost of the item to make it available.
}
