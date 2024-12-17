using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;

public class GameStarter : MonoBehaviour
{
    [Header("Game Settings")]
    public float roundDuration = 180f; // Duration of the round in seconds (3 minutes)
    private float currentTimer;

    [Header("Game Elements")]
    public GameObject[] actionElements; // GameObjects to toggle during the game (e.g., NPC spawners, etc.)
    public string[] itemTagsToClear; // Tags of items to remove at the end of the round

    [Header("Elements to Disable and Re-enable")]
    public GameObject[] elementsToDisableAndReenable; // Objects to disable at the start and re-enable at the end

    [Header("UI Elements")]
    public GameObject startButton; // Button to start the game (for XR interaction)
    public TMP_Text timerText; // Text to display the countdown

    private bool isGameRunning = false;

    private void Start()
    {
        currentTimer = roundDuration;

        // Ensure the initial state of all game elements
        SetActionElementsActive(false); 
        SetElementsActive(elementsToDisableAndReenable, true); // Enable re-enabled elements at the start
    }

    private void Update()
    {
        if (isGameRunning)
        {
            HandleTimer();
        }
    }

    // Starts the game when the button is pressed
    public void StartGame()
    {
        isGameRunning = true;
        currentTimer = roundDuration;

        startButton.SetActive(false); // Hide start button

        SetActionElementsActive(true); // Activate action elements
        SetElementsActive(elementsToDisableAndReenable, false); // Disable specific elements
    }

    // Handles the countdown timer
    private void HandleTimer()
    {
        if (currentTimer > 0)
        {
            currentTimer -= Time.deltaTime;
            UpdateTimerUI();
        }
        else
        {
            EndRound();
        }
    }

    // Ends the current round
    private void EndRound()
    {
        isGameRunning = false;

        SetActionElementsActive(false); // Deactivate action elements
        SetElementsActive(elementsToDisableAndReenable, true); // Re-enable specific elements
        ClearItemsWithTags(); // Remove items with specified tags

        startButton.SetActive(true); // Reactivate the start button for the next round
    }

    // Toggles the active state of the action elements
    private void SetActionElementsActive(bool isActive)
    {
        foreach (GameObject element in actionElements)
        {
            if (element != null)
            {
                element.SetActive(isActive);
            }
        }
    }

    // Toggles the active state of elements to disable and re-enable
    private void SetElementsActive(GameObject[] elements, bool isActive)
    {
        foreach (GameObject element in elements)
        {
            if (element != null)
            {
                element.SetActive(isActive);
            }
        }
    }

    // Clears items with specific tags from the game world
    private void ClearItemsWithTags()
    {
        foreach (string tag in itemTagsToClear)
        {
            GameObject[] items = GameObject.FindGameObjectsWithTag(tag);
            foreach (GameObject item in items)
            {
                Destroy(item);
            }
        }
    }

    // Updates the timer UI
    private void UpdateTimerUI()
    {
        int minutes = Mathf.FloorToInt(currentTimer / 60);
        int seconds = Mathf.FloorToInt(currentTimer % 60);
        timerText.text = $"{minutes:00}:{seconds:00}";
    }
}
