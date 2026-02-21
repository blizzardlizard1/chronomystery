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
    }

    private IEnumerator SpawnRoutine()
    {
        Transform spawnPoint = transform;
        var portals = FindObjectsOfType<TimeSwitch>();

        foreach (var portal in portals)
        {
            if (portal.PortalID == MirrorManager.TargetPortalID)
            {
                spawnPoint = portal.SpawnPoint;
                break;
            }
        }

        GameObject player = PlayerController.Instance.gameObject;
        player.transform.SetPositionAndRotation(spawnPoint.position, spawnPoint.rotation);

        if (SmoothCameraFollow.Instance != null)
            SmoothCameraFollow.Instance.SetTarget(player.transform);

        if (EnterTransition != null)
            yield return EnterTransition.OnEnterScene();

        MirrorManager.TargetPortalID = null;
    }
}