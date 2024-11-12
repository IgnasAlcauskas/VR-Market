using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSnapZone : MonoBehaviour
{
   [Header("Grid Settings")]
    public int rows = 3;         // Number of rows in the grid
    public int columns = 3;       // Number of columns in the grid
    public float cellSize = 1.0f; // Size of each grid cell
    public Vector3 gridOrigin;    // The starting point of the grid

    private List<Vector3> snapPoints = new List<Vector3>();
    private List<Transform> snappedObjects = new List<Transform>();

    void Start()
    {
        GenerateSnapPoints();
    }

    private void GenerateSnapPoints()
    {
        // Clear existing snap points if regenerating
        snapPoints.Clear();

        // Create a grid of snap points based on rows, columns, and cell size
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                Vector3 snapPosition = gridOrigin + new Vector3(col * cellSize, 0, row * cellSize);
                snapPoints.Add(snapPosition);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object can snap (e.g., by tag or component)
        if (other.CompareTag("Snappable"))
        {
            Vector3 availablePoint = GetAvailableSnapPoint();
            if (availablePoint != Vector3.zero)
            {
                SnapObjectToPoint(other.transform, availablePoint);
            }
        }
    }

    private Vector3 GetAvailableSnapPoint()
    {
        // Find the first unoccupied snap point
        foreach (Vector3 point in snapPoints)
        {
            bool isOccupied = false;
            foreach (Transform obj in snappedObjects)
            {
                if (Vector3.Distance(obj.position, point) < 0.1f)
                {
                    isOccupied = true;
                    break;
                }
            }

            if (!isOccupied)
            {
                return point;
            }
        }

        // If all snap points are occupied, return zero vector (no available spot)
        return Vector3.zero;
    }

    private void SnapObjectToPoint(Transform obj, Vector3 snapPoint)
    {
        // Move the object to the snap point
        obj.position = snapPoint;

        // Add to the list of snapped objects
        snappedObjects.Add(obj);
    }

    private void OnTriggerExit(Collider other)
    {
        // When an object leaves the snap zone, free up its spot
        if (snappedObjects.Contains(other.transform))
        {
            snappedObjects.Remove(other.transform);
        }
    }
}
