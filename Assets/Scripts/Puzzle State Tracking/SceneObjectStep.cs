using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(menuName = "Puzzle/Steps/Scene Has Object")]
public class SceneObjectStep : PuzzleStep
{
    [Tooltip("The tag assigned to the prefab instance in the scene.")]
    public string targetTag;

    public override bool IsComplete(PuzzleTracker tracker)
    {
        // Check if any object with this tag exists in the current scene
        GameObject target = GameObject.FindWithTag(targetTag);
        
        return target != null;
    }
}