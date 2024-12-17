using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObjects : MonoBehaviour
{
    [SerializeField] private List<GameObject> prefabsToSpawn; // List of prefabs to spawn
    [SerializeField] private int numberOfObjects = 10; // Max number of objects to spawn
    [SerializeField] private BoxCollider boxCollider; // Box Collider representing the box area
    [SerializeField] private float spawnRadius = 0.5f; // Minimum distance between spawned objects
    [SerializeField] private LayerMask objectLayer; // Layer for spawned objects

    private List<GameObject> availablePrefabs; // Filtered list of available prefabs

    void Start()
    {
        FilterAvailableItems();
        SpawnObject();
    }

    void OnEnable()
    {
        FilterAvailableItems();
        SpawnObject();
    }

    void FilterAvailableItems()
    {
        availablePrefabs = new List<GameObject>();

        foreach (GameObject prefab in prefabsToSpawn)
        {
            Item itemComponent = prefab.GetComponent<Item>();
            if (itemComponent != null && itemComponent.itemAvailable)
            {
                availablePrefabs.Add(prefab);
            }
        }

        if (availablePrefabs.Count == 0)
        {
            Debug.LogError("No available items to spawn!");
        }
    }

    void SpawnObject()
    {
        if (boxCollider == null)
        {
            Debug.LogError("BoxCollider is not assigned!");
            return;
        }

        if (availablePrefabs.Count == 0)
        {
            Debug.LogError("No available prefabs to spawn!");
            return;
        }

        int spawnedCount = 0;

        for (int i = 0; i < numberOfObjects; i++)
        {
            int retries = 10;

            while (retries > 0)
            {
                GameObject prefabToSpawn = GetRandomPrefab();
                Vector3 spawnPosition = GetRandomPositionInBox(prefabToSpawn);

                // Check for overlap
                if (!IsOverlap(spawnPosition, spawnRadius))
                {
                    GameObject spawnedObject = Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);

                    // Assign Rigidbody to ensure physics
                    Rigidbody rb = spawnedObject.GetComponent<Rigidbody>();
                    if (rb == null)
                    {
                        rb = spawnedObject.AddComponent<Rigidbody>();
                    }
                    rb.useGravity = true; // Enable gravity for realistic behavior
                    rb.isKinematic = false;

                    spawnedCount++;
                    break;
                }

                retries--;
            }

            if (retries == 0)
            {
                Debug.Log($"Stopped spawning after {spawnedCount} objects. No space left.");
                break;
            }
        }
    }

    GameObject GetRandomPrefab()
    {
        int randomIndex = Random.Range(0, availablePrefabs.Count);
        return availablePrefabs[randomIndex];
    }

    Vector3 GetRandomPositionInBox(GameObject prefab)
    {
        Bounds prefabBounds = GetPrefabBounds(prefab);

        Vector3 boxSize = boxCollider.size;
        Vector3 boxCenter = boxCollider.center;

        float halfX = (boxSize.x / 2) - (prefabBounds.size.x / 2);
        float halfY = (boxSize.y / 2) - (prefabBounds.size.y / 2);
        float halfZ = (boxSize.z / 2) - (prefabBounds.size.z / 2);

        Vector3 localRandomPosition = new Vector3(
            Random.Range(-halfX, halfX),
            Random.Range(-halfY, halfY),
            Random.Range(-halfZ, halfZ)
        );

        return boxCollider.transform.TransformPoint(boxCenter + localRandomPosition);
    }

    Bounds GetPrefabBounds(GameObject prefab)
    {
        // Temporarily instantiate prefab to calculate its bounds
        GameObject tempObject = Instantiate(prefab);
        // Check for any renderer (MeshRenderer, SpriteRenderer, etc.)
        Renderer renderer = tempObject.GetComponentInChildren<Renderer>();
        Bounds prefabBounds = new Bounds(Vector3.zero, Vector3.zero);
        if (renderer != null)
        {
            prefabBounds = renderer.bounds;
        }
        else
        {
            Debug.LogWarning($"{prefab.name} has no Renderer. Defaulting to zero bounds.");
        }

        Destroy(tempObject);
        return prefabBounds;
    }

    bool IsOverlap(Vector3 position, float radius)
    {
        // Check for objects within the spawn radius
        Collider[] overlaps = Physics.OverlapSphere(position, radius, objectLayer);
        return overlaps.Length > 0;
    }
}
