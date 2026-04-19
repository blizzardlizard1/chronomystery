using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepAudio : MonoBehaviour
{
    public AudioClip[] footstepClips;  // drag multiple clips in here
    public float stepInterval = 0.5f;  // time between steps

    private AudioSource audioSource;
    private float stepTimer;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        bool isMoving = Input.GetAxis("Horizontal") != 0 || 
                        Input.GetAxis("Vertical") != 0;

        if (isMoving)
        {
            stepTimer -= Time.deltaTime;

            if (stepTimer <= 0f)
            {
                PlayRandomStep();
                stepTimer = stepInterval;
            }
        }
        else
        {
            stepTimer = 0f; // reset so first step plays immediately
        }
    }

    void PlayRandomStep()
    {
        if (footstepClips.Length == 0) return;

        AudioClip clip = footstepClips[Random.Range(0, footstepClips.Length)];
        audioSource.PlayOneShot(clip);
    }
}