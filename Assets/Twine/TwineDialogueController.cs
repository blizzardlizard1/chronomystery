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
        if (index < 0 || index >= current.choices.Count) return;

        string next = current.choices[index].targetPassage;
        StartAt(next);
    }

    private void ShowCurrent()
    {
        // Hook this into your existing Dialogue UI system
        DialogueUI.Instance.Show(
            current.text,
            current.choices
        );
    }
}