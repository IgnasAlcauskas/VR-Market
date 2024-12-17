using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class SpendingSystem : MonoBehaviour
{
    [Header("Income Manager")]
    public Income incomeManager; // Reference to the Income script.

    [Header("Item Settings")]
    public List<Item> allItems;    // List of all items in the game.
    public List<string> predefinedTags;  // Predefined tags to display in the dropdown.

    [Header("Rent Settings")]
    public int rentCost = 50; // Rent cost

    [Header("UI References")]
    public Transform itemListContainer;  // Container to hold item entries.
    public GameObject itemEntryPrefab;   // Prefab for displaying item info.

    private Dictionary<string, Item> randomItemsPerTag = new Dictionary<string, Item>();

    private void Start()
    {
        // Generate a random item for each tag.
        GenerateRandomItemsForAllTags();
        
        // Populate the UI with these random items.
        PopulateUI();
    }

    // Picks a random item for each tag
    private void GenerateRandomItemsForAllTags()
    {
        randomItemsPerTag.Clear(); // Clear previous random items

        foreach (string tag in predefinedTags)
        {
            // Find all items with the selected tag that are not available
            List<Item> itemsWithTag = allItems.FindAll(item => item.CompareTag(tag) && !item.itemAvailable);

            if (itemsWithTag.Count > 0)
            {
                // Pick a random item from the list
                Item randomItem = itemsWithTag[Random.Range(0, itemsWithTag.Count)];
                randomItemsPerTag[tag] = randomItem;
            }
        }
    }

    // Populates the UI with the randomly selected items
    private void PopulateUI()
    {
    if (itemListContainer == null)
    {
        Debug.LogError("Item List Container is not assigned!");
        return;
    }

    if (itemEntryPrefab == null)
    {
        Debug.LogError("Item Entry Prefab is not assigned!");
        return;
    }

    // Clear previous UI entries
    foreach (Transform child in itemListContainer)
    {
        Destroy(child.gameObject);
    }

    foreach (var kvp in randomItemsPerTag)
    {
        string tag = kvp.Key;
        Item item = kvp.Value;

        if (item == null)
        {
            Debug.LogError("Item is null for tag: " + tag);
            continue;
        }

        // Instantiate the prefab
        GameObject itemEntry = Instantiate(itemEntryPrefab, itemListContainer);

        // Ensure the child objects are not null
        TMP_Text itemText = itemEntry.transform.Find("ItemText")?.GetComponent<TMP_Text>();
        Button unlockButton = itemEntry.transform.Find("UnlockButton")?.GetComponent<Button>();

        if (itemText == null || unlockButton == null)
        {
            Debug.LogError("Required components (ItemText or UnlockButton) are missing in the prefab.");
            continue;
        }

        itemText.text = $"{item.itemName} - {item.itemCost} Coins";

        unlockButton.onClick.AddListener(() =>
        {
            AttemptToUnlockItem(item);
        });
    }
    }

    // Attempts to unlock the item by checking if the player has enough money
    private void AttemptToUnlockItem(Item item)
    {
        if (incomeManager.GetMoney() >= item.itemCost)
        {
            incomeManager.RemoveMoney(item.itemCost);
            item.itemAvailable = true; // Mark item as available
            Debug.Log($"{item.itemName} is now unlocked!");

            // Refresh the UI to reflect the changes
            PopulateUI();
        }
        else
        {
            Debug.Log($"Not enough money to buy {item.itemName}.");
        }
    }

    // Method to pay rent
    public void PayRent()
    {
        if (incomeManager.GetMoney() >= rentCost)
        {
            incomeManager.RemoveMoney(rentCost); // Deduct the rent cost
            Debug.Log("Rent paid successfully!");

            // Optionally, update UI or do any other logic related to paying rent.
        }
        else
        {
            Debug.Log("Not enough money to pay rent.");
        }
    }

    public void LoadGameSceneByIndex()
    {   
        SceneManager.LoadScene(1); 
    }
}


