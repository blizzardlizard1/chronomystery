using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Puzzle/Steps/Inventory Has Item")]
public class InventoryStep : PuzzleStep
{
    public string itemName;

    public override bool IsComplete(PuzzleTracker tracker)
    {
        if (tracker.PlayerInventory == null) return false;
        return tracker.PlayerInventory.HasItem(itemName);
    }
}