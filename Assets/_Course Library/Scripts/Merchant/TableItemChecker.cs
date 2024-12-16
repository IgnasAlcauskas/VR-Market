using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableItemChecker : MonoBehaviour
{
    [Header("Table References")]
    public List<Transform> tables;  // List of tables in your scene

    private List<string> rememberedTags = new List<string>();  // List to store item tags
    private List<int> rememberedLayers = new List<int>();      // List to store item layers

    [Header("Settings")]
    public float checkDelay = 2f;  // Delay before checking the items on the tables
    public float boxHeightIncrease = 2f; // Increase the height of the box for better coverage

    private void Start()
    {
        // Start the coroutine to check items after a delay
        StartCoroutine(CheckItemsWithDelay());
    }

    // Coroutine to check items with a delay
    private IEnumerator CheckItemsWithDelay()
    {
        // Wait for the specified delay before checking
        yield return new WaitForSeconds(checkDelay);

        // Once the delay is over, check the tables
        CheckItemsOnTables();
    }

    // Method to check the items on each table
    void CheckItemsOnTables()
    {
        // Clear previous data
        rememberedTags.Clear();
        rememberedLayers.Clear();

        foreach (Transform table in tables)
        {
            // Adjust the box size to include more height
            Vector3 boxSize = table.localScale / 2;
            boxSize.y += boxHeightIncrease; // Increase the height of the box to ensure coverage

            // Find all colliders within the adjusted box area of the table
            Collider[] itemsOnTable = Physics.OverlapBox(table.position, boxSize, table.rotation);

            foreach (Collider item in itemsOnTable)
            {
                // Add the item's tag to the rememberedTags list
                rememberedTags.Add(item.tag);
                Debug.Log($"Item with tag {item.tag} found on table {table.name}.");

                // Add the item's layer to the rememberedLayers list
                rememberedLayers.Add(item.gameObject.layer);
                Debug.Log($"Item with layer {LayerMask.LayerToName(item.gameObject.layer)} found on table {table.name}.");
            }
        }
    }

    // Optionally, create a method to retrieve the remembered tags or layers
    public List<string> GetRememberedTags()
    {
        return rememberedTags;
    }

    public List<int> GetRememberedLayers()
    {
        return rememberedLayers;
    }
}
