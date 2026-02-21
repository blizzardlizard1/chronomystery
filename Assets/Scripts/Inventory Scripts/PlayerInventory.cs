using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInventory : MonoBehaviour
{
    public int inventorySize = 5;
    public UnityEvent onInventoryChanged;
    public List<string> slots = new List<string>();

    private void Awake()
    {
        for (int i = 0; i < inventorySize; i++)
            slots.Add(null);
    }

    public bool AddItem(string itemName)
    {
        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i] == null)
            {
                slots[i] = itemName;
                onInventoryChanged?.Invoke();
                return true;
            }
        }

        Debug.Log("Inventory full!");
        return false;
    }

    public void RemoveItem(int slotIndex)
    {
        if (slots[slotIndex] != null)
        {
            slots[slotIndex] = null;
            onInventoryChanged?.Invoke();
        }
    }

    public bool HasItem(string itemName)
    {
        return slots.Contains(itemName);
    }
}