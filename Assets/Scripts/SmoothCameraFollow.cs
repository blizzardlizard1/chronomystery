using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothCameraFollow : MonoBehaviour
{   

    [SerializeField] private Transform target;
    [SerializeField] private float smoothTime;

    private Vector3 offset;
    private Vector3 currentVelocity = Vector3.zero;

    // Start is called before the first frame update
    void Awake()
    {
        offset = transform.position - target.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 targetPosition = target.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref currentVelocity, smoothTime);
    }
}
