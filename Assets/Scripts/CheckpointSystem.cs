using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckpointSystem : MonoBehaviour
{
    public static CheckpointSystem Instance { get; private set; }

    private CheckpointData _savedCheckpoint;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Call this to save the current world state as a checkpoint.
    public void SaveCheckpoint()
    {
        var inv = PlayerController.Instance.GetComponent<PlayerInventory>();

        _savedCheckpoint = new CheckpointData
        {
            sceneName = SceneManager.GetActiveScene().name,
            playerPosition = PlayerController.Instance.transform.position,
            playerRotation = PlayerController.Instance.transform.rotation,
            inventorySlots = new List<string>(inv.slots),
            destroyedObjects = SceneStateManager.Instance.GetDestroyedSet()
        };

        Debug.Log("Checkpoint saved!");
    }

    // Call this to restore the world state to the last checkpoint.
    public void LoadCheckpoint()
    {
        if (_savedCheckpoint == null)
        {
            Debug.Log("No checkpoint saved!");
            return;
        }

        // Restore destroyed objects state
        SceneStateManager.Instance.SetDestroyedSet(_savedCheckpoint.destroyedObjects);

        // Restore inventory
        var inv = PlayerController.Instance.GetComponent<PlayerInventory>();
        inv.slots = new List<string>(_savedCheckpoint.inventorySlots);
        inv.onInventoryChanged?.Invoke();

        // Reload the checkpoint scene
        MirrorManager.TargetPortalID = null;
        MirrorManager.Instance.StartCoroutine(ReloadRoutine());
    }

    private System.Collections.IEnumerator ReloadRoutine()
    {
        var transition = MirrorManager.Instance.Transition;

        if (transition != null)
            yield return transition.OnExitScene();

        AsyncOperation loadOp = SceneManager.LoadSceneAsync(_savedCheckpoint.sceneName);
        loadOp.allowSceneActivation = false;

        while (loadOp.progress < 0.9f)
        {
            transition?.OnLoadProgress(loadOp.progress / 0.9f);
            yield return null;
        }

        if (transition != null)
            yield return transition.OnSceneReady();

        loadOp.allowSceneActivation = true;
        yield return null;

        // Restore player position
        PlayerController.Instance.transform.SetPositionAndRotation(
            _savedCheckpoint.playerPosition,
            _savedCheckpoint.playerRotation
        );

        if (transition != null)
            yield return transition.OnEnterScene();
    }

    public bool HasCheckpoint => _savedCheckpoint != null;

    private class CheckpointData
    {
        public string sceneName;
        public Vector3 playerPosition;
        public Quaternion playerRotation;
        public List<string> inventorySlots;
        public HashSet<string> destroyedObjects;
    }
}