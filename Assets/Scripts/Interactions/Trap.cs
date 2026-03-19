using UnityEngine;

public class Trap : MonoBehaviour
{
    [SerializeField] private float destroyDelay = 5f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(gameObject, destroyDelay);
        }
    }
}