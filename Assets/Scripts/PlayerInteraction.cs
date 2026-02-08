using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    private Interactable currentInteractable;

    void Update()
    {
        if (currentInteractable != null && currentInteractable.PlayerInRange)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                currentInteractable.Interact();
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        var interactable = other.GetComponentInParent<Interactable>();
        if (interactable != null)
            currentInteractable = interactable;
    }

    private void OnTriggerExit(Collider other)
    {
        var interactable = other.GetComponentInParent<Interactable>();
        if (interactable != null && interactable == currentInteractable)
            currentInteractable = null;
    }
}