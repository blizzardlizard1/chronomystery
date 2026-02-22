using System.Collections.Generic;
using UnityEngine;

public class SceneStateManager : MonoBehaviour
{
    public static SceneStateManager Instance { get; private set; }

    private HashSet<string> _destroyedObjects = new HashSet<string>();

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

    public void MarkDestroyed(string objectID)
    {
        _destroyedObjects.Add(objectID);
    }

    public bool IsDestroyed(string objectID)
    {
        return _destroyedObjects.Contains(objectID);
    }

    public HashSet<string> GetDestroyedSet()
    {
        return new HashSet<string>(_destroyedObjects);
    }

    public void SetDestroyedSet(HashSet<string> set)
    {
        _destroyedObjects = new HashSet<string>(set);
    }
}