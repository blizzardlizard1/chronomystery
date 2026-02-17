using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class Interactable : MonoBehaviour
{
    [Header("Interaction Settings")]
    public string promptText = "Press SPACE to interact";

    [Header("Wheel Actions")]
    [Tooltip("Which wheel actions this object supports.")]
    public List<WheelAction> supportedActions = new();

    [Header("Action Events — assign per object in Inspector")]
    public UnityEvent onMoveAction;
    public UnityEvent onBreakAction;
    public UnityEvent onPlaceAction;
    public UnityEvent onChangeAction;

    // Legacy single-interact event (kept for non-wheel interactables)
    [Header("Legacy (non-wheel interaction)")]
    public UnityEvent onInteract;

    private bool playerInRange = false;

    public bool PlayerInRange => playerInRange;
    public string Prompt => promptText;
    public bool HasWheelActions => supportedActions.Count > 0;

    public void SetPlayerInRange(bool value)
    {
        playerInRange = value;

        if (value)
            InteractionUI.Instance.Show(promptText);
        else
            InteractionUI.Instance.Hide();
    }

    /// Called when player presses SPACE. Opens wheel if actions exist,
    /// otherwise falls back to legacy onInteract event.
    public void Interact()
    {
        if (HasWheelActions)
        {
            InteractionUI.Instance.Hide();
            InteractionWheel.Instance.Open(this);
        }
        else
        {
            Debug.Log("Legacy interact");
            onInteract?.Invoke();
        }
    }

    /// Called by the wheel when the player confirms a selection.
    public void ExecuteWheelAction(WheelAction action)
    {
        switch (action)
        {
            case WheelAction.Move: onMoveAction?.Invoke(); break;
            case WheelAction.Break: onBreakAction?.Invoke(); break;
            case WheelAction.Place: onPlaceAction?.Invoke(); break;
            case WheelAction.Change: onChangeAction?.Invoke(); break;
        }

        Debug.Log($"[Interactable] {action} on {gameObject.name}");
    }
}