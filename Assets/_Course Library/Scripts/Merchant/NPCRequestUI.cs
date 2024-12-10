using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NPCRequestUI : MonoBehaviour
{
    public TMP_Text requestText; // Or TMP_Text for TextMeshPro

    public void SetRequest(string itemName)
    {
        if (requestText != null)
        {
            requestText.text = $"I want a {itemName}";
        }
    }
}

