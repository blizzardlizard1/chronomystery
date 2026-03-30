using UnityEngine;
using UnityEngine.SceneManagement;

public class SockChecker : MonoBehaviour
{
    [Header("Plug PREFABS here (not scene objects)")]
    public GameObject socks;        // Prefab expected in Scene B
    public GameObject dirtySocks;   // Prefab to reveal in Scene A

    public string sceneBName = "SceneB";

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;

        Debug.Log("[SockChecker] Initialized and set to persist across scenes.");
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"[SockChecker] Scene loaded: {scene.name}");

        if (scene.name != sceneBName)
        {
            Debug.Log("[SockChecker] Not Scene B — ignoring.");
            return;
        }

        Debug.Log("[SockChecker] Now inside Scene B. Checking for socks prefab...");

        // Does the socks prefab exist in this scene?
        if (FindInstanceOfPrefab(socks))
        {
            Debug.Log("[SockChecker] Socks prefab FOUND in Scene B!");

            string id = dirtySocks.name;

            Debug.Log($"[SockChecker] Marking '{id}' as spawned so it will appear in Scene A.");

            SceneStateManager.Instance.MarkSpawned(id);
        }
        else
        {
            Debug.Log("[SockChecker] Socks prefab NOT present in Scene B.");
        }
    }

    /// <summary>
    /// Detects if an instance of the given prefab exists in the current scene.
    /// </summary>
    private bool FindInstanceOfPrefab(GameObject prefab)
    {
        var allObjects = FindObjectsOfType<GameObject>();

        foreach (var obj in allObjects)
        {
            // Unity instantiates objects with the same name as the prefab
            if (obj.name == prefab.name)
                return true;
        }

        return false;
    }
}
