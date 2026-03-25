using System.Collections;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;

    private ISceneTransition EnterTransition => MirrorManager.Instance?.Transition;

    void Start()
    {
        StartCoroutine(InitPlayer());
    }

    private IEnumerator InitPlayer()
    {
        yield return null;

        if (PlayerController.Instance == null)
        {
            GameObject player = Instantiate(playerPrefab, transform.position, transform.rotation);
            player.name = "Player";
        }

        if (!string.IsNullOrEmpty(MirrorManager.TargetPortalID))
        {
            yield return SpawnRoutine();
        }
        else
        {
            CheckpointSystem.Instance.SaveCheckpoint();
        }
    }

    private IEnumerator SpawnRoutine()
    {
        Transform spawnPoint = transform;

        // Check TimeSwitch portals
        var portals = FindObjectsOfType<TimeSwitch>();
        foreach (var portal in portals)
        {
            if (portal.PortalID == MirrorManager.TargetPortalID)
            {
                spawnPoint = portal.SpawnPoint;
                break;
            }
        }

        // Check ChangeRoom doors
        var doors = FindObjectsOfType<ChangeRoom>();
        foreach (var door in doors)
        {
            if (door.RoomID == MirrorManager.TargetPortalID)
            {
                spawnPoint = door.SpawnPoint;
                break;
            }
        }

        GameObject player = PlayerController.Instance.gameObject;
        player.transform.SetPositionAndRotation(spawnPoint.position, spawnPoint.rotation);

        if (SmoothCameraFollow.Instance != null)
            SmoothCameraFollow.Instance.SnapToTarget(player.transform);

        if (EnterTransition != null)
            yield return EnterTransition.OnEnterScene();

        MirrorManager.TargetPortalID = null;
    }
}