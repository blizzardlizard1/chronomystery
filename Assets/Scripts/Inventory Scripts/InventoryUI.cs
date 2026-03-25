using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
public PlayerInventory inventory;
    public InventorySlot slotPrefab;
    public Transform slotParent;

    private InventorySlot[] _slots;
    private int _selectedSlot = -1;

    private void Start() => StartCoroutine(Init());

    private IEnumerator Init()
    {
        while (PlayerController.Instance == null)
            yield return null;

        if (!inventory)
            inventory = PlayerController.Instance.GetComponent<PlayerInventory>();

        _slots = new InventorySlot[inventory.inventorySize];

        for (int i = 0; i < _slots.Length; i++)
        {
            _slots[i] = Instantiate(slotPrefab, slotParent);
            _slots[i].Init(i, this);
        }

        inventory.onInventoryChanged.AddListener(UpdateUI);
        UpdateUI();
    }

    private void UpdateUI()
    {
        for (int i = 0; i < _slots.Length; i++)
            _slots[i].SetSlot(inventory.slots[i]);
    }

    /// <summary>Called by InventoryController to highlight the active slot.</summary>
    public void SetNavigating(bool active, int selectedSlot)
    {
        for (int i = 0; i < _slots.Length; i++)
            _slots[i].SetHighlighted(active && i == selectedSlot);
    }

    /// <summary>Called by InventorySlot buttons. Selects a slot for placement.</summary>
    public void OnSlotClicked(int index)
    {
        _selectedSlot = (_selectedSlot == index) ? -1 : index; // toggle
        Debug.Log(_selectedSlot >= 0 ? $"Selected slot {_selectedSlot}" : "Deselected");
    }

    public int GetSelectedSlot() => _selectedSlot;
    public void ClearSelection() => _selectedSlot = -1;
}