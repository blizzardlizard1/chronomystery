using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    [Header("Interaction Settings")]
    public string promptText = "Press SPACE to interact";

    [Tooltip("Actions that will run when the player interacts.")]
    public UnityEvent onInteract;

    private bool playerInRange = false;

    public bool PlayerInRange => playerInRange;

    public string Prompt => promptText;

    public void SetPlayerInRange(bool value)
    {
        playerInRange = value;

        if (value) {
            InteractionUI.Instance.Show(promptText);
        }
        else {
            InteractionUI.Instance.Hide();
        }
            
    }

    public void Interact()
    {
        onInteract?.Invoke();
    }
}