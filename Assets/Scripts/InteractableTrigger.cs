using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class InteractableTrigger : MonoBehaviour
{
    private Interactable interactable;

    private void Awake()
    {
        interactable = GetComponentInParent<Interactable>();
        if (interactable == null) {
            Debug.LogError("Trigger Zone could NOT find Interactable on parent!", this);
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            interactable.SetPlayerInRange(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            interactable.SetPlayerInRange(false);
    }
}