using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class IncomeTracker : MonoBehaviour
{
    public TMP_Text moneyText; // Drag and drop the TextMeshPro component in the Inspector

    private void Start()
    {
        // Ensure moneyText is assigned
        if (moneyText == null)
        {
            Debug.LogError("MoneyText is not assigned!");
            return;
        }

        // Update the UI with the current money at the start
        UpdateMoneyDisplay(Income.Instance.GetMoney());

        // Subscribe to the money change event
        Income.Instance.OnMoneyChanged += UpdateMoneyDisplay;
    }

    private void OnDestroy()
    {
        // Unsubscribe to prevent memory leaks
        Income.Instance.OnMoneyChanged -= UpdateMoneyDisplay;
    }

    // This method will be called whenever the money changes
    private void UpdateMoneyDisplay(int newMoneyAmount)
    {
        moneyText.text = $"Money: {newMoneyAmount}";
    }
}
