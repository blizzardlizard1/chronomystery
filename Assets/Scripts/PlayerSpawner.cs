using System.Collections;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;

    private ISceneTransition EnterTransition => MirrorManager.Instance?.Transition;

    void Start()
    {
        if (!string.IsNullOrEmpty(MirrorManager.TargetPortalID))
        {
            StartCoroutine(SpawnRoutine());
        }
    }

    private IEnumerator SpawnRoutine()
    {
        // Find the matching portal
        Transform spawnPoint = transform; // fallback
        var portals = FindObjectsOfType<TimeSwitch>();

        foreach (var portal in portals)
        {
            if (portal.PortalID == MirrorManager.TargetPortalID)
            {
                spawnPoint = portal.SpawnPoint;
                break;
            }
        }

        // Spawn the player
        GameObject existing = GameObject.FindGameObjectWithTag("Player");
        if (existing != null)
            Destroy(existing);

        GameObject player = Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);

        if (SmoothCameraFollow.Instance != null)
            SmoothCameraFollow.Instance.SetTarget(player.transform);

        // --- Enter transition: fade in, end cutscene, etc. ---
        if (EnterTransition != null)
            yield return EnterTransition.OnEnterScene();

        // Clear the target so normal scene loads aren't affected
        MirrorManager.TargetPortalID = null;
    }
}