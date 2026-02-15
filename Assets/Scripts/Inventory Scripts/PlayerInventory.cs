using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class PlayerInventory : MonoBehaviour
{
    public int inventorySize = 5; // fixed 5 slots

    public UnityEvent onInventoryChanged;

    public List<bool> slots = new List<bool>(); 
    // true = item present, false = empty

    private void Awake()
    {
        for (int i = 0; i < inventorySize; i++)
            slots.Add(false);
    }

    public bool AddItem()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            if (!slots[i])
            {
                slots[i] = true;
                onInventoryChanged?.Invoke();
                return true;
            }
        }

        Debug.Log("Inventory full!");
        return false;
    }

    public void RemoveItem(int slotIndex)
    {
        if (slots[slotIndex])
        {
            slots[slotIndex] = false;
            onInventoryChanged?.Invoke();
        }
    }
}