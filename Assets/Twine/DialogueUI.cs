using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class DialogueUI : MonoBehaviour
{
    public static DialogueUI Instance;

    [Header("UI References")]
    public TMP_Text dialogueText;
    public Transform choiceContainer;
    public Button choiceButtonPrefab;

    void Awake()
    {
        Instance = this;
        gameObject.SetActive(false);
    }

    public void Show(string text, List<TwineChoice> choices)
    {
        dialogueText.text = text;
        ClearChoices();

        for (int i = 0; i < choices.Count; i++)
        {
            var index = i;
            var button = Instantiate(choiceButtonPrefab, choiceContainer);

            button.GetComponentInChildren<TMP_Text>().text = choices[i].label;

            button.onClick.AddListener(() =>
            {
                TwineDialogueController.Instance.SelectChoice(index);
            });
        }

        gameObject.SetActive(true);
    }

    public void Hide()
    {
        ClearChoices();
        gameObject.SetActive(false);
    }

    private void ClearChoices()
    {
        foreach (Transform child in choiceContainer)
            Destroy(child.gameObject);
    }
}