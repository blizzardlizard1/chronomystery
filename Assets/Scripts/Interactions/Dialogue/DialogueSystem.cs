using System;
using UnityEngine;

public class DialogueSystem : MonoBehaviour
{
    public static DialogueSystem Instance;

    private DialogueObject currentDialogue;
    private int index = 0;
    private Action onFinish;

    private bool isRunning = false;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (!isRunning) return;

        if (Input.GetKeyDown(KeyCode.Space))
            NextLine();
    }

    /// Start a dialogue sequence
    public void StartDialogue(DialogueObject dialogue, Action onFinishCallback = null)
    {
        currentDialogue = dialogue;
        index = 0;
        onFinish = onFinishCallback;
        isRunning = true;

        ShowCurrentLine();
    }

    private void ShowCurrentLine()
    {
        var line = currentDialogue.lines[index];
        DialogueUI.Instance.Show(
            line.speaker.ToString(), 
            line.text
        );
    }

    private void NextLine()
    {
        index++;

        if (index >= currentDialogue.lines.Count)
        {
            EndDialogue();
            return;
        }

        ShowCurrentLine();
    }

    private void EndDialogue()
    {
        isRunning = false;
        DialogueUI.Instance.Hide();
        onFinish?.Invoke();
    }
}
