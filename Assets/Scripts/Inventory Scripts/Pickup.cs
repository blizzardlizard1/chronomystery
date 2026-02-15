using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// attach this along with interactable and interactable trigger for any items

public class Pickup : MonoBehaviour
{
    public void PickupItem()
    {
        PlayerInventory inv = FindObjectOfType<PlayerInventory>();

        if (inv.AddItem())
        {
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("Inventory full!");
        }
    }
}