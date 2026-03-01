using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

[CreateAssetMenu(menuName = "Puzzle/Steps/Scene Reached")]
public class SceneStep : PuzzleStep
{
    public string sceneName;

    public override bool IsComplete(PuzzleTracker tracker)
    {
        return SceneManager.GetActiveScene().name == sceneName;
    }
}
