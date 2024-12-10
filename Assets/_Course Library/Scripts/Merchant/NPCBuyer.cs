using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCBuyer : MonoBehaviour
{
    [Header("Item Request")]
    public string requestedItem; // The item requested by the NPC
    public int itemPrice; // The price of the requested item
    public GameObject uiPrefab; // Prefab for the request UI element
    private GameObject activeUI; // Instance of the spawned UI element
    private bool uiSpawned = false;

    [Header("Item Prices")]
    public List<ItemData> itemsForSale; // List of items and their prices

    private BasicWalkScript walkScript;

    private void Start()
    {
        walkScript = GetComponent<BasicWalkScript>();
        if (walkScript == null)
        {
            Debug.LogError("NPCBuyer requires BasicWalkScript on the same GameObject!");
        }
        ChooseItem();
    }

    private void Update()
    {
        if (walkScript != null && walkScript.inLine && !uiSpawned)
        {
            SpawnRequestUI();
        }
    }

    private void ChooseItem()
    {
        if (itemsForSale.Count == 0)
        {
            Debug.LogError("No items available for sale!");
            return;
        }

        // Randomly select an item
        int randomIndex = Random.Range(0, itemsForSale.Count);
        ItemData selectedItem = itemsForSale[randomIndex];

        requestedItem = selectedItem.itemName;
        itemPrice = selectedItem.itemPrice;

        Debug.Log($"{gameObject.name} is requesting: {requestedItem} for {itemPrice} coins.");

        int itemLayer = LayerMask.NameToLayer(selectedItem.layerName); // Ensure ItemData has a 'layerName' property
        if (!RandomSpawn.spawnedLayers.Contains(itemLayer))
        {
            itemPrice = Mathf.CeilToInt(itemPrice * 1.5f);
            Debug.Log($"{gameObject.name} is requesting a rare item ({requestedItem}). Price increased to {itemPrice} coins.");
        }
    }

    private void SpawnRequestUI()
    {
        // Spawn the UI only once
        if (uiPrefab != null)
        {
            activeUI = Instantiate(uiPrefab, transform.position + new Vector3(0, 2, 0), Quaternion.identity, transform);
            activeUI.GetComponent<NPCRequestUI>().SetRequest($"{requestedItem} - {itemPrice} coins");
            uiSpawned = true; // Set the flag to prevent multiple spawns
        }
    }

    // This method will be called when an item is placed near the NPC's head
    private void OnTriggerEnter(Collider other)
    {
        Item item = other.GetComponent<Item>();  // Assuming 'Item' has a reference to the item name
        if (item != null)
        {
            // Check if the item matches the requested item
            if (item.itemName == requestedItem)
            {
                // NPC receives the item
                ReceiveItem(item);
            }
            else
            {
                 Debug.Log($"{gameObject.name} says: 'This isn't what I asked for!'");
            }
        }
    }

    public void ReceiveItem(Item item)
    {
        // NPC accepts the item and pays the player
        Debug.Log($"{gameObject.name} received the {item.itemName}!");

        // Add money to the player's inventory
        Income.Instance.AddMoney(itemPrice);
        Debug.Log($"Player earned {itemPrice} coins!");

        // Destroy the request UI
        if (activeUI != null)
        {
            Destroy(activeUI);
        }

        // Notify BasicWalkScript to make the NPC leave
        if (walkScript != null)
        {
            walkScript.GetOutOfLine();
        }

        // Destroy the item
        Destroy(item.gameObject);
    }
}

