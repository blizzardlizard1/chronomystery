using System.Collections.Generic;
using UnityEngine;
public class SceneStateManager : MonoBehaviour
{
    public static SceneStateManager Instance { get; private set; }

    // --- Placed Items ---
    private readonly Dictionary<string, ItemData> _placedItems = new();
    public void SetPlacedItem(string pointID, ItemData item) => _placedItems[pointID] = item;
    public void ClearPlacedItem(string pointID) => _placedItems.Remove(pointID);
    public ItemData GetPlacedItem(string pointID) => _placedItems.GetValueOrDefault(pointID);

    // --- Picked Up Pre-placed Items ---
    private readonly HashSet<string> _pickedUp = new();
    public void MarkPickedUp(string pointID) => _pickedUp.Add(pointID);
    public bool IsPickedUp(string pointID) => _pickedUp.Contains(pointID);

    private HashSet<string> _destroyedObjects = new HashSet<string>();
    private HashSet<string> _spawnedObjects = new HashSet<string>();
    private HashSet<string> _unlockedObjects = new HashSet<string>();

    void Awake() {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // --- Destroyed ---
    public void MarkDestroyed(string id) => _destroyedObjects.Add(id);
    public void UnmarkDestroyed(string id) => _destroyedObjects.Remove(id);
    public bool IsDestroyed(string id) => _destroyedObjects.Contains(id);
    public HashSet<string> GetDestroyedSet() => new HashSet<string>(_destroyedObjects);
    public void SetDestroyedSet(HashSet<string> s) => _destroyedObjects = new HashSet<string>(s);

    // --- Spawned ---
    public void MarkSpawned(string id) => _spawnedObjects.Add(id);
    public void UnmarkSpawned(string id) => _spawnedObjects.Remove(id);
    public bool IsSpawned(string id) => _spawnedObjects.Contains(id);
    public HashSet<string> GetSpawnedSet() => new HashSet<string>(_spawnedObjects);
    public void SetSpawnedSet(HashSet<string> s) => _spawnedObjects = new HashSet<string>(s);

    // --- Unlocked ---
    public void MarkUnlocked(string id) => _unlockedObjects.Add(id);
    public void UnmarkUnlocked(string id) => _unlockedObjects.Remove(id);
    public bool IsUnlocked(string id) => _unlockedObjects.Contains(id);
    public HashSet<string> GetUnlockedSet() => new HashSet<string>(_unlockedObjects);
    public void SetUnlockedSet(HashSet<string> s) => _unlockedObjects = new HashSet<string>(s);

    // --- Live Registry ---
    private readonly Dictionary<string, ObjectSync> _registry = new();
    public void Register(string id, ObjectSync obj) => _registry[id] = obj;
    public void Unregister(string id) => _registry.Remove(id);
    public ObjectSync Find(string id) => _registry.GetValueOrDefault(id);

}