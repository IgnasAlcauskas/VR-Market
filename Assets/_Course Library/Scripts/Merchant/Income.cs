using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Income : MonoBehaviour
{
    public static Income Instance;
    public int currentMoney = 0;

    // Create an event to notify when money changes
    public event Action<int> OnMoneyChanged;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void AddMoney(int amount)
    {
        currentMoney += amount;
        Debug.Log($"Player now has {currentMoney} coins!");

        // Trigger the event when money changes
        OnMoneyChanged?.Invoke(currentMoney);
    }

    public void RemoveMoney(int amount)
    {
        currentMoney -= amount;
        Debug.Log($"Player now has {currentMoney} coins!");

        // Trigger the event when money changes
        OnMoneyChanged?.Invoke(currentMoney);
    }

    public int GetMoney()
    {
        return currentMoney;
    }
}
