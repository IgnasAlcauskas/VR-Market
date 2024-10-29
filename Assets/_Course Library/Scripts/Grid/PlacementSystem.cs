using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
    [SerializeField]
    private GameObject indicator, cellIndicator;

    [SerializeField]
    private InputManager inputManager;

    [SerializeField]
    private Grid grid;
   
    [SerializeField]
    private ObjectDatabaseSO database;
    private int selectedObjectIndex = -1;

    [SerializeField]
    private GameObject gridVisualization;

  private void Start()
   {
        StopPlacement();
   }

  public void StartPlacement(int ID)
   {
        StopPlacement();
        selectedObjectIndex = database.objectData.FindIndex(data => data.ID == ID);
        if(selectedObjectIndex < 0)
        {
            Debug.LogError($"No ID found {ID}");
            return;
        }
        gridVisualization.SetActive(true);
        cellIndicator.SetActive(true);
        inputManager.OnClicked += PlaceStructure;
        inputManager.OnExit += StopPlacement;
   }

   public void PlaceStructure()
   {
        if(inputManager.IsPointerOverUI())
        {
            return;
        }
        Vector3 Pos = inputManager.GetSelectedPosition();
        Vector3Int gridPosition = grid.WorldToCell(Pos);
        GameObject gameObject = Instantiate(database.objectData[selectedObjectIndex].Prefab);
        gameObject.transform.position = grid.CellToWorld(gridPosition);
   }

  public void StopPlacement()
   {
        selectedObjectIndex = -1;
        gridVisualization.SetActive(false);
        cellIndicator.SetActive(false);
        inputManager.OnClicked -= PlaceStructure;
        inputManager.OnExit -= StopPlacement;
   }

   private void Update()
   {
        if(selectedObjectIndex < 0)
            return;
        Vector3 Pos = inputManager.GetSelectedPosition();
        Vector3Int gridPosition = grid.WorldToCell(Pos);
        indicator.transform.position = Pos;
        cellIndicator.transform.position = grid.CellToWorld(gridPosition);
   }
}
