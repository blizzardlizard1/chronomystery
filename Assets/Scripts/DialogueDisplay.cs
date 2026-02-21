using System.Collections;
using TMPro;
using UnityEngine;

public class DialogueDisplay : MonoBehaviour
{
    public static DialogueDisplay Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private float typeSpeed = 0.03f;
    [SerializeField] private float displayDuration = 2f;

    private Coroutine _activeDialogue;

    void Awake()
    {
        Instance = this;
        dialogueText.text = "";
    }

    public void Show(string message)
    {
        if (_activeDialogue != null)
            StopCoroutine(_activeDialogue);

        _activeDialogue = StartCoroutine(TypeRoutine(message));
    }

    private IEnumerator TypeRoutine(string message)
    {
        if (dialogueText == null) yield break;

        dialogueText.text = "";

        for (int i = 0; i < message.Length; i++)
        {
            if (dialogueText == null) yield break;
            dialogueText.text += message[i];
            yield return new WaitForSeconds(typeSpeed);
        }

        yield return new WaitForSeconds(displayDuration);

        if (dialogueText != null)
            dialogueText.text = "";

        _activeDialogue = null;
    }
}