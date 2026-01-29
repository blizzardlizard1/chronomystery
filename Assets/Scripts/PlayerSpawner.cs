using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;

    void Start()
    {
        if (!string.IsNullOrEmpty(MirrorManager.TargetPortalID))
        {
            // Find the portal we should spawn at
            var portals = FindObjectsOfType<TimeSwitch>();

            foreach (var portal in portals)
            {
                if (portal.PortalID == MirrorManager.TargetPortalID)
                {
                    SpawnPlayer(portal.SpawnPoint);
                    return;
                }
            }

            // Fallback: spawn at default location if no matching portal
            Debug.LogWarning($"No portal found with ID: {MirrorManager.TargetPortalID}");
            SpawnPlayer(transform);
        }

    }

    void SpawnPlayer(Transform spawnPoint)
    {
        GameObject player = Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
        
        if (SmoothCameraFollow.Instance != null)
        {
            SmoothCameraFollow.Instance.SetTarget(player.transform);
        }
    }
}
