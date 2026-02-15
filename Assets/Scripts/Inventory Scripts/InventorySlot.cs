using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InventorySlot : MonoBehaviour
{
    public TextMeshProUGUI slotText;

    public void SetSlot(bool hasItem)
    {
        slotText.text = hasItem ? "Item" : "";
    }
}