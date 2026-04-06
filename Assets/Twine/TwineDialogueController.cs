using UnityEngine;

public class TwineDialogueController : MonoBehaviour
{
    public static TwineDialogueController Instance;

    private TwineHTMLParser parser = new TwineHTMLParser();
    public TwinePassage currentPassage;

    private void Awake()
    {
        Instance = this;
    }

    public void LoadTwine(string html)
    {
        parser.LoadTwine(html);
    }

    public void StartDialogue()
    {
        if (parser.startPassageName == null)
        {
            Debug.LogError("TwineDialogueController: No start passage detected.");
            return;
        }

        Debug.Log($"TwineDialogueController: Starting at '{parser.startPassageName}'");

        ShowPassage(parser.startPassageName);
    }

    public void ShowPassage(string name)
    {
        if (!parser.passages.TryGetValue(name, out var passage))
        {
            Debug.LogError($"TwineDialogueController: Passage '{name}' not found.");
            return;
        }

        currentPassage = passage;

        DialogueUI.Instance.ShowPassage(passage);

        // Play voice audio clip (optional)
        if (passage.voiceClip != null)
            AudioSource.PlayClipAtPoint(passage.voiceClip, Camera.main.transform.position);
    }

    public void Choose(TwineChoice choice)
    {
        if (choice.targetPassageName == "*End dialogue*")
        {
            DialogueUI.Instance.Hide();
            return;
        }

        ShowPassage(choice.targetPassageName);
    }
}