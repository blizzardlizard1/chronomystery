using UnityEngine;

public class ObjectSync : MonoBehaviour
{
    [SerializeField] private string objectID;
    [SerializeField] private string[] linkedObjectIDs;

    [Tooltip("If true, this object starts hidden and only appears after RevealPersistent().")]
    [SerializeField] private bool startsHidden = false;

    void OnEnable() => SceneStateManager.Instance.Register(objectID, this);
    void OnDisable() => SceneStateManager.Instance.Unregister(objectID);


    public string ObjectID => objectID;
    public bool StartsHidden => startsHidden;
    void Start()
    {
        var mgr = SceneStateManager.Instance;

        if (mgr.IsDestroyed(objectID))
        {
            Destroy(gameObject);
            return;
        }

        if (startsHidden && !mgr.IsSpawned(objectID))
        {
            // Hide all renderers and colliders instead of deactivating,
            // so the object stays registered in SceneStateManager
            foreach (var r in GetComponentsInChildren<Renderer>()) r.enabled = false;
            foreach (var c in GetComponentsInChildren<Collider>()) c.enabled = false;
        }
    }

    /// <summary>Permanently removes this object and any linked ones.</summary>
    public void DestroyPersistent()
    {
        var mgr = SceneStateManager.Instance;
        mgr.MarkDestroyed(objectID);
        foreach (var id in linkedObjectIDs) mgr.MarkDestroyed(id);
        Destroy(gameObject);
    }

    /// <summary>Makes this object visible and marks it as spawned persistently.</summary>
    public void RevealPersistent()
    {
        var mgr = SceneStateManager.Instance;
        mgr.MarkSpawned(objectID);
        foreach (var id in linkedObjectIDs) mgr.MarkSpawned(id);
        foreach (var r in GetComponentsInChildren<Renderer>()) r.enabled = true;
        foreach (var c in GetComponentsInChildren<Collider>()) c.enabled = true;
    }

}