using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class Interactable : MonoBehaviour
{
    [Header("Interaction Settings")]
    public string promptText = "Press SPACE to interact";

    [Header("Wheel Actions")]
    [Tooltip("Which wheel actions this object supports.")]
    public List<ActionEntry> supportedActions = new();

    [Header("Action Events — assign per object in Inspector")]
    public UnityEvent onTopAction;
    public UnityEvent onRightAction;
    public UnityEvent onBottomAction;
    public UnityEvent onLeftAction;

    // Legacy single-interact event (kept for non-wheel interactables)
    [Header("Legacy (non-wheel interaction)")]
    public UnityEvent onInteract;

    private bool playerInRange = false;

    public bool PlayerInRange => playerInRange;
    public string Prompt => promptText;
    public bool HasWheelActions => supportedActions.Count > 0;

    public void SetPlayerInRange(bool value)
    {
        // Ignore closed mirrors
        if (value && CompareTag("Mirror"))
        {
            var mirror = GetComponent<TimeSwitch>();
            if (mirror != null && !mirror.IsOpen)
            {
                playerInRange = false;
                InteractionUI.Instance.Hide();
                return;
            }
        }

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
            onInteract?.Invoke();
        }
    }

    /// Called by the wheel when the player confirms a selection.
    public void ExecuteWheelAction(WheelAction action)
    {
        switch (action)
        {
            case WheelAction.Top: onTopAction?.Invoke(); break;
            case WheelAction.Right: onRightAction?.Invoke(); break;
            case WheelAction.Bottom: onBottomAction?.Invoke(); break;
            case WheelAction.Left: onLeftAction?.Invoke(); break;
        }

    }
}