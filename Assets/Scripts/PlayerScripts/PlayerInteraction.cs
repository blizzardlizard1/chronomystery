using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public static PlayerInteraction Instance { get; private set; }
    private Interactable currentInteractable;
    private bool interactionLocked;

    void Start()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Update()
    {   
        if (interactionLocked) return;

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

    public void SetInteractionLocked(bool locked)
    {   
        if (!locked)
        {
            StartCoroutine(UnlockNextFrame());
            return;
        }
        interactionLocked = locked;
    }

    private IEnumerator UnlockNextFrame()
    {
        yield return null;
        interactionLocked = false;
    }
}