using UnityEngine;

public enum WheelAction
{
    Top,   // 0 — top segment
    Right,  // 1 — right segment
    Bottom,  // 2 — bottom segment
    Left  // 3 — left segment
}

[System.Serializable]
public struct ActionEntry
{
    public WheelAction action;
    public string name;
    
    [Tooltip("Leave empty if no item is needed")]
    public string requiredItem;

    [Tooltip("Dialogue that displays when performing this action.")]
    public string dialogue;
}