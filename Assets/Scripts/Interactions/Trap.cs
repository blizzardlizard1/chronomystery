using UnityEngine;

public class Trap : MonoBehaviour
{
    [SerializeField] private float timeToFall = 2f;

    private float timer = 0f;
    private bool playerOnTrap = false;

    private Collider playerCollider;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        playerOnTrap = true;
        timer = 0f;

        // Get the player's collider
        playerCollider = other.GetComponent<Collider>();

        if (playerCollider == null)
            Debug.Log("ERROR: Player has no collider on the same object as the Rigidbody!");
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        playerOnTrap = false;
        timer = 0f;
    }

    private void Update()
    {
        if (!playerOnTrap) return;

        timer += Time.deltaTime;

        if (timer >= timeToFall)
        {
            if (playerCollider != null)
            {
                Debug.Log("Disabling player collider — player will fall!");
                playerCollider.enabled = false;
            }

            playerOnTrap = false; // prevent repeat
        }
    }
}