using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpawn : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject[] objectsToSpawn; // Array of objects to spawn
    public LayerMask requiredLayer; // Layer of objects eligible to spawn
    public float spawnChance = 30f; // Percentage chance to spawn (0-100)
    public Vector3 spawnOffset; // Offset to adjust spawn position relative to the socket

    public static HashSet<int> spawnedLayers = new HashSet<int>(); // Tracks layers of spawned objects (static for global access)

    private void Start()
    {
        float randomValue = Random.Range(0f, 100f);
        if (randomValue <= spawnChance)
        {
            SpawnObjectWithLayer();
        }
    }

    private void OnEnable()
    {
        float randomValue = Random.Range(0f, 100f);
        if (randomValue <= spawnChance)
        {
            SpawnObjectWithLayer();
        }
    }

    private void SpawnObjectWithLayer()
    {
        if (objectsToSpawn.Length == 0)
        {
            Debug.LogWarning("No objects assigned to spawn!");
            return;
        }

        foreach (GameObject obj in objectsToSpawn)
        {
            if ((1 << obj.layer & requiredLayer.value) != 0) // Check if object's layer matches the required layer
            {
                GameObject spawned = Instantiate(obj, transform.position + spawnOffset, transform.rotation, transform);
                spawnedLayers.Add(spawned.layer); // Add layer of spawned object to the set
                Debug.Log($"Spawned {obj.name} at {gameObject.name} on layer '{LayerMask.LayerToName(spawned.layer)}'.");
                return; // Stop after spawning one object
            }
        }

        Debug.LogWarning($"No objects with the required layer '{LayerMask.LayerToName(requiredLayer)}' found in the assigned list.");
    }
}
