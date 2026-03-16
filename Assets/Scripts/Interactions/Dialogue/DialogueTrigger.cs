using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField] private DialogueObject dialogue;

    public void Trigger()
    {
        DialogueSystem.Instance.StartDialogue(dialogue);
    }
}