using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PuzzleOneTracker : MonoBehaviour
{
    [Header("Puzzle State")]
    public bool puzzleComplete = false;
    public int puzzleScore = 0;

    [Header("References")]
    public PlayerInventory playerInventory;
    public GameObject puddleObject;     // drag the puddle object here
    public string toolsItemName = "Tools";

    [Header("Scene Names")]
    public string pastSceneName = "Past";
    public string presentSceneName = "Present";

    private bool wentToPast = false;
    private bool pickedUpTools = false;
    private bool fixedPuddle = false;
    private bool returnedToPresent = false;

    private const int stepValue = 10;
    private const int completionScore = 40;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject); 
    }

    private void Update()
    {
        if (puzzleComplete) return;

        CheckSceneProgress();
        CheckInventoryProgress();
        CheckPuddleProgress();
    }

    private void CheckSceneProgress()
    {
        string currentScene = SceneManager.GetActiveScene().name;

        if (!wentToPast && currentScene == pastSceneName)
        {
            wentToPast = true;
            AddProgress("Went to Past");
        }

        if (!returnedToPresent && currentScene == presentSceneName && wentToPast)
        {
            returnedToPresent = true;
            AddProgress("Returned to Present");
        }
    }

    private void CheckInventoryProgress()
    {
        if (!pickedUpTools && playerInventory.HasItem(toolsItemName))
        {
            pickedUpTools = true;
            AddProgress("Picked Up Tools");
        }
    }

    private void CheckPuddleProgress()
    {
        if (!fixedPuddle && puddleObject == null)
        {
            fixedPuddle = true;
            AddProgress("Fixed Puddle");
        }
    }

    private void AddProgress(string stepName)
    {
        puzzleScore += stepValue;
        Debug.Log($"Puzzle 1 Step completed: {stepName}. Score = {puzzleScore}");

        if (puzzleScore >= completionScore)
        {
            puzzleComplete = true;
            Debug.Log("Puzzle Complete!");
        }
    }
}
