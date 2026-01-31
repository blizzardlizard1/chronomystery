using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SphereSync : MonoBehaviour
{
    public SpherePlace state;
    public bool writeToState = false; // ONLY true in past

    private Vector3 runtimePosition;


    void Awake() {
        // Only initialize ONCE, only in the present
        if (!writeToState && !state.initialized)
        {
            state.position = new Vector3(2f, 0f, 0f);
            state.initialized = true;
        }
    }

    void Start()
    {
        // Always read the saved position when a scene loads
        transform.position = state.position;
    }

    void Update()
    {
        // Only the past version writes changes
        if (writeToState)
        {
            state.position = transform.position;
        }
    }
}
