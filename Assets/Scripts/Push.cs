using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Push : MonoBehaviour
{
    public float pushStrength = 5f;

    private void OnCollisionStay(Collision collision)
    {
        // Get player rigidbody
        Rigidbody rb = collision.rigidbody;
        if (rb == null) return;

        // Get direction from player to object
        Vector3 direction = transform.position - collision.transform.position;
        direction.y = 0f;

        // Apply force
        GetComponent<Rigidbody>().AddForce(direction.normalized * pushStrength);
    }
}

