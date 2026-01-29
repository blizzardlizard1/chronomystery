using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TimeSwitch : MonoBehaviour
{   
    [SerializeField] private bool open = true;
    [SerializeField] private string destinationName;

    [Tooltip("Corresponding mirrors should have the same portalID regardless of time period.")]
    [SerializeField] private string portalID;

    [Tooltip("Where the player spawns when arriving through this mirror.")]
    [SerializeField] private Transform spawnPoint;

    public string PortalID => portalID;
    public Transform SpawnPoint => spawnPoint;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && open)
        {
            MirrorManager.TargetPortalID = portalID;
            SceneManager.LoadScene(destinationName);
        }
    }
}
