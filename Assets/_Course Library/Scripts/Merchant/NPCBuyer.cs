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

    [Header("Special Exit Item")]
    public string specialItemTag = "BeatStick"; // The tag of the item that triggers an early exit
    public int penaltyAmount = 10; // Coins deducted when the special item is used

    [Header("Tables and Item Checker")]
    [SerializeField]
    public TableItemChecker tableItemChecker;  // Reference to the TableItemChecker script
    public float priceMultiplier = 1.5f; // Multiplier for items not found on the table

    private BasicWalkScript walkScript;

    private void Start()
    {
        if (tableItemChecker == null)
        {
            tableItemChecker = FindObjectOfType<TableItemChecker>();
        }
        
        walkScript = GetComponent<BasicWalkScript>();
        if (walkScript == null)
        {
            Debug.LogError("NPCBuyer requires BasicWalkScript on the same GameObject!");
        }
        ChooseItem();
    }

    private void OnEnable()
    {
        if (tableItemChecker == null)
        {
            tableItemChecker = FindObjectOfType<TableItemChecker>();
        }
        
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
        if (itemsForSale == null || itemsForSale.Count == 0)
        {
            Debug.LogError($"{gameObject.name}: No items available for sale!");
            return;
        }

        // Filter items to include only those marked as available
        List<ItemData> availableItems = itemsForSale.FindAll(item => item.isAvailable);

        if (availableItems.Count == 0)
        {
            Debug.LogError($"{gameObject.name}: No available items to choose from!");
            return;
        }

        // Randomly select an item from the filtered list
        int randomIndex = Random.Range(0, availableItems.Count);
        ItemData selectedItem = availableItems[randomIndex];

        requestedItem = selectedItem.itemName;
        itemPrice = selectedItem.itemPrice;

        // Check if the item type (layer/tag) is on the table
        if (!IsItemTypeOnTable(selectedItem))
        {
            // If not, increase the price
            itemPrice = Mathf.CeilToInt(itemPrice * priceMultiplier);
            Debug.Log($"{gameObject.name} is requesting a rare item ({requestedItem}). Price increased to {itemPrice} coins.");
        }

        Debug.Log($"{gameObject.name} is requesting: {requestedItem} for {itemPrice} coins.");
    }

    private bool IsItemTypeOnTable(ItemData selectedItem)
    {
        // Check if any item type (layer or tag) is on the table
        List<string> rememberedTags = tableItemChecker.GetRememberedTags();
        List<int> rememberedLayers = tableItemChecker.GetRememberedLayers();

        // Check if the tag or layer of the requested item is in the list of items on the table
        bool isTagOnTable = rememberedTags.Contains(selectedItem.itemTag);
        bool isLayerOnTable = rememberedLayers.Contains(LayerMask.NameToLayer(selectedItem.layerName));

        return isTagOnTable || isLayerOnTable;
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

    private void OnTriggerEnter(Collider other)
    {
                // Check for the special item
        if (other.CompareTag(specialItemTag))
        {
            HandleSpecialItem(other.gameObject);
        }
        else
        {
            Item item = other.GetComponent<Item>(); // Assuming 'Item' has a reference to the item name
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

    private void HandleSpecialItem(GameObject specialItem)
    {
        Debug.Log($"{gameObject.name} encountered the special item: {specialItem.name}. NPC leaving!");

        // Deduct penalty coins
        Income.Instance.RemoveMoney(penaltyAmount);
        Debug.Log($"Player lost {penaltyAmount} coins. Current money: {Income.Instance.currentMoney}");

        // Destroy the request UI
        if (activeUI != null)
        {
            Destroy(activeUI);
        }

        // Notify NPC to leave
        if (walkScript != null)
        {
            walkScript.GetOutOfLine();
        }
    }
}


