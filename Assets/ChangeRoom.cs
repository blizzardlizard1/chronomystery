using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeRoom : MonoBehaviour
{
    [SerializeField] private string destinationScene;
    [SerializeField] private string roomID;
    [SerializeField] private Transform spawnPoint;

    public string RoomID => roomID;
    public Transform SpawnPoint => spawnPoint;

    private ISceneTransition Transition => MirrorManager.Instance?.Transition;

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player") && !MirrorManager.Instance.IsTransitioning)
        {
            MirrorManager.TargetPortalID = roomID;
            MirrorManager.Instance.StartCoroutine(ChangeRoomRoutine());
        }
    }

    private IEnumerator ChangeRoomRoutine()
    {
        MirrorManager.Instance.IsTransitioning = true;

        if (Transition != null)
            yield return Transition.OnExitScene();

        AsyncOperation loadOp = SceneManager.LoadSceneAsync(destinationScene);
        loadOp.allowSceneActivation = false;

        while (loadOp.progress < 0.9f)
        {
            Transition?.OnLoadProgress(loadOp.progress / 0.9f);
            yield return null;
        }

        if (Transition != null)
            yield return Transition.OnSceneReady();

        loadOp.allowSceneActivation = true;
        yield return null;

        if (Transition != null)
            yield return Transition.OnEnterScene();

        MirrorManager.Instance.IsTransitioning = false;
    }
}