using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InventorySlot : MonoBehaviour
{
    public TextMeshProUGUI slotText;

    public void SetSlot(string name)
    {
        slotText.text = name ?? "";
    }
}