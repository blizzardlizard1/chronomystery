using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(menuName = "Puzzle/Steps/Object Destroyed")]
public class DestroyStep : PuzzleStep
{
    public string objectName;
    private GameObject cachedObject;

    public override void OnSceneLoaded(Scene scene)
    {
        cachedObject = GameObject.Find(objectName);
    }

    public override bool IsComplete(PuzzleTracker tracker)
    {
        return cachedObject == null;
    }
}
