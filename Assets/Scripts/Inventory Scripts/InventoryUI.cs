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
        StartCoroutine(Init());
    }

    // Waits for player to spawn into Scene before trying to find their inventory
    private IEnumerator Init()
    {
        while (PlayerController.Instance == null)
            yield return null;

        if (!inventory)
            inventory = PlayerController.Instance.GetComponent<PlayerInventory>();

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