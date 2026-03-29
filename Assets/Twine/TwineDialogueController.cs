using UnityEngine;
using System.Collections.Generic;

public class TwineDialogueController : MonoBehaviour
{
    public static TwineDialogueController Instance;

    private Dictionary<string, TwinePassage> passages;
    private TwinePassage current;

    void Awake() => Instance = this;

    public void LoadTwine(string htmlText)
    {
        passages = TwineHTMLParser.Parse(htmlText);
    }

    public void StartAt(string passageName)
    {
        if (!passages.TryGetValue(passageName, out current))
        {
            Debug.LogError("Twine: Could not find passage " + passageName);
            return;
        }

        ShowCurrent();
    }

    public void SelectChoice(int index)
    {
        if (current == null || index < 0 || index >= current.choices.Count)
            return;

        TwineChoice choice = current.choices[index];

        // 1. Detect end dialogue choices
        if (IsEndChoice(choice.label))
        {
            DialogueUI.Instance.Hide();
            current = null;
            return;
        }

        // 2. Jump to next passage
        StartAt(choice.targetPassage);
    }

    private bool IsEndChoice(string label)
    {
        if (string.IsNullOrEmpty(label))
            return false;

        string lower = label.ToLower();

        return lower.Contains("end dialogue") ||
               lower == "end" ||
               lower.Contains("goodbye") ||
               lower.Contains("bye");
    }

    private void ShowCurrent()
    {
        // Hook this into existing Dialogue UI system
        DialogueUI.Instance.Show(
            current.text,
            current.choices
        );
    }
}