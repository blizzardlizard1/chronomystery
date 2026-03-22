using UnityEngine;

public class TwineInteractable : MonoBehaviour
{
    [Tooltip("Reference the Twine HTML file as TextAsset")]
    public TextAsset twineHtml;

    [Tooltip("Start passage name, e.g. 'Start' or 'Intro'")]
    public string startPassage = "Start";

    bool loaded = false;

    public void StartDialogue()
    {
        if (!loaded)
        {
            TwineDialogueController.Instance.LoadTwine(twineHtml.text);
            loaded = true;
        }

        TwineDialogueController.Instance.StartAt(startPassage);
    }
}