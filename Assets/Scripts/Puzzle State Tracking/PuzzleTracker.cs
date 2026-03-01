using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PuzzleTracker : MonoBehaviour
{
    [Header("Puzzle Steps (in order)")]
    public List<PuzzleStep> steps = new List<PuzzleStep>();

    public int currentStepIndex = 0;
    public bool puzzleComplete = false;

    public PlayerInventory PlayerInventory { get; private set; }

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
        CheckCurrentStep();
    }

    private void TryFindPlayerInventory()
    {
        if (PlayerInventory != null) return;

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
            PlayerInventory = player.GetComponent<PlayerInventory>();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (currentStepIndex < steps.Count)
            steps[currentStepIndex].OnSceneLoaded(scene);
    }

    private void CheckCurrentStep()
    {
        if (currentStepIndex >= steps.Count)
        {
            puzzleComplete = true;
            Debug.Log("Puzzle Complete!");
            return;
        }

        PuzzleStep step = steps[currentStepIndex];

        if (step.IsComplete(this))
        {
            Debug.Log($"Step completed: {step.stepName}");
            currentStepIndex++;

            if (currentStepIndex >= steps.Count)
            {
                puzzleComplete = true;
                Debug.Log("Puzzle Complete!");
            }
        }
    }
}