using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


// attach this along with interactable and interactable trigger for any items
public class InteractableUtils : MonoBehaviour
{
    public string itemName;

    [SerializeField] private string examineText;

    public void PickupItem()
    {
        PlayerInventory inv = FindObjectOfType<PlayerInventory>();

        if (inv.AddItem(itemName))
        {
            GetComponent<ObjectSync>().DestroyPersistent();
        }
        else
        {
            Debug.Log("Inventory full!");
        }
    }

    public void RemoveObject()
    {
        GetComponent<ObjectSync>().DestroyPersistent();
    }
}