using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PuzzleOneTracker : MonoBehaviour
{
    [Header("Puzzle State")]
    public bool puzzleComplete = false;
    public int puzzleScore = 0;

    [Header("Settings")]
    public string toolsItemName = "Tools";
    public string pastSceneName = "Past";
    public string presentSceneName = "Present";

    // since its not in the starting scene, we need to find in runtime
    public string puddleObjectName = "Puddle";

    private PlayerInventory playerInventory;
    private GameObject puddleObject;

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

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Update()
    {
        if (puzzleComplete) return;

        TryFindPlayerInventory();
        CheckSceneProgress();
        CheckInventoryProgress();
        CheckPuddleProgress();
    }

    // Find player inventory after player spawns in runtime
    private void TryFindPlayerInventory()
    {
        if (playerInventory != null) return;

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerInventory = player.GetComponent<PlayerInventory>();
            if (playerInventory != null)
                Debug.Log("PlayerInventory found");
        }
    }

    // When a scene loads, try to find the puddle if we're in the Past
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == pastSceneName)
        {
            puddleObject = GameObject.Find(puddleObjectName);
            if (puddleObject != null)
                Debug.Log("Puddle found");
        }
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
        if (playerInventory == null) return;

        if (!pickedUpTools && playerInventory.HasItem(toolsItemName))
        {
            pickedUpTools = true;
            AddProgress("Picked Up Tools");
        }
    }

    private void CheckPuddleProgress()
    {
        if (!fixedPuddle && puddleObject == null && wentToPast)
        {
            fixedPuddle = true;
            AddProgress("Fixed Puddle");
        }
    }

    private void AddProgress(string stepName)
    {
        puzzleScore += stepValue;
        Debug.Log($"Step completed: {stepName}. Score = {puzzleScore}");

        if (puzzleScore >= completionScore)
        {
            puzzleComplete = true;
            Debug.Log("Puzzle Complete!");
        }
    }
}