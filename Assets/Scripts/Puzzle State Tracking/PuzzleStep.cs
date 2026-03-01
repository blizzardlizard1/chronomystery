using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class PuzzleStep : ScriptableObject
{
    public string stepName;

    // Called every frame by the tracker
    public abstract bool IsComplete(PuzzleTracker tracker);

    // Called when a scene loads (override when step involves switching scene)
    public virtual void OnSceneLoaded(Scene scene) { }
}