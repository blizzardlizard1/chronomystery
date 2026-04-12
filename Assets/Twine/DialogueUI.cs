using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueUI : MonoBehaviour
{
    public static DialogueUI Instance;

    public GameObject root;
    public TextMeshProUGUI dialogueText;
    public Transform choiceContainer;
    public GameObject choiceButtonPrefab;

    private void Awake()
    {
        Instance = this;
        Hide();
    }

    public void ShowPassage(TwinePassage passage)
    {
        root.SetActive(true);
        dialogueText.text = passage.cleanedText;

        // Clear old buttons
        foreach (Transform child in choiceContainer)
            Destroy(child.gameObject);

        // Create new choice buttons
        foreach (var c in passage.choices)
        {
            var btnObj = Instantiate(choiceButtonPrefab, choiceContainer);
            btnObj.GetComponentInChildren<TextMeshProUGUI>().text = c.text;
            btnObj.GetComponent<Button>().onClick.AddListener(() =>
            {
                TwineDialogueController.Instance.Choose(c);
            });
        }
    }

    public void Hide()
    {
        root.SetActive(false);
    }
}