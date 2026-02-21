using System;
using System.Collections;
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
    public bool IsOpen => open;

    private ISceneTransition Transition => MirrorManager.Instance?.Transition;

    public void TimeTravel()
    {
        if (open && !MirrorManager.Instance.IsTransitioning)
        {
            // Run on MirrorManager so the coroutine survives the scene unload
            MirrorManager.Instance.StartCoroutine(TimeTravelRoutine());
        }
    }

    private IEnumerator TimeTravelRoutine()
    {
        MirrorManager.Instance.IsTransitioning = true;

        // --- Phase 1: Exit transition (fade out, start cutscene, etc.) ---
        if (Transition != null)
            yield return Transition.OnExitScene();

        // --- Phase 2: Set up destination  ---
        MirrorManager.TargetPortalID = portalID;

        // --- Phase 3: Load scene asynchronously ---
        AsyncOperation loadOp = SceneManager.LoadSceneAsync(destinationName);
        loadOp.allowSceneActivation = false;

        while (loadOp.progress < 0.9f)
        {
            Transition?.OnLoadProgress(loadOp.progress / 0.9f);
            yield return null;
        }

        if (Transition != null)
            yield return Transition.OnSceneReady();

        // --- Phase 4: Activate the new scene ---
        loadOp.allowSceneActivation = true;

        // Wait a frame for the scene to fully initialize
        yield return null;

        // --- Phase 5: Enter transition (fade in) ---
        // This works because the coroutine is running on MirrorManager
        if (Transition != null)
            yield return Transition.OnEnterScene();

        MirrorManager.Instance.IsTransitioning = false;
    }
}

public interface ISceneTransition
{
    IEnumerator OnExitScene();
    void OnLoadProgress(float normalizedProgress);
    IEnumerator OnSceneReady();
    IEnumerator OnEnterScene();
}