using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInventory : MonoBehaviour
{
    public int inventorySize = 5;
    public UnityEvent onInventoryChanged;
    public List<ItemData> slots = new List<ItemData>();

    private void Awake()
    {
        for (int i = 0; i < inventorySize; i++)
            slots.Add(null);
    }

    public bool AddItem(ItemData item)
    {
        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i] == null)
            {
                slots[i] = item;
                onInventoryChanged?.Invoke();
                return true;
            }
        }
        Debug.Log("Inventory full!");
        return false;
    }

    public bool PlaceItem(int slotIndex)
    {
        if (slots[slotIndex] == null) return false;
        RemoveItem(slotIndex);
        return true;
    }

    public void RemoveItem(ItemData item) {
        int slot = slots.IndexOf(item);
        if (slot >= 0) RemoveItem(slot);
    }

    public void RemoveItem(int slotIndex)
    {
        if (slots[slotIndex] != null)
        {
            slots[slotIndex] = null;
            onInventoryChanged?.Invoke();
        }
    }

    public bool HasItem(ItemData item) => slots.Contains(item);

    public bool HasItemName(string itemName) {
        return slots.Exists(slot => slot != null && slot.itemName == itemName);
    }
}