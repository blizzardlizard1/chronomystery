using UnityEngine;
using TMPro;

public class DialogueUI : MonoBehaviour
{
    public static DialogueUI Instance;

    [SerializeField] private GameObject panel;
    [SerializeField] private TextMeshProUGUI speakerText;
    [SerializeField] private TextMeshProUGUI dialogueText;

    private void Awake()
    {
        Instance = this;
        panel.SetActive(false);
    }

    public void Show(string speaker, string text)
    {
        panel.SetActive(true);
        speakerText.text = speaker;
        dialogueText.text = text;
    }

    public void Hide()
    {
        panel.SetActive(false);
    }
}