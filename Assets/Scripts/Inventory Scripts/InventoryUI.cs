using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public PlayerInventory inventory;
    public InventorySlot slotPrefab;
    public Transform slotParent;

    private InventorySlot[] slots;

    private void Start()
    {
        slots = new InventorySlot[inventory.inventorySize];

        for (int i = 0; i < slots.Length; i++)
            slots[i] = Instantiate(slotPrefab, slotParent);

        inventory.onInventoryChanged.AddListener(UpdateUI);
        UpdateUI();
    }

    private void UpdateUI()
    {
        for (int i = 0; i < slots.Length; i++)
            slots[i].SetSlot(inventory.slots[i]);
    }
}