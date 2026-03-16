using UnityEngine;

public enum SpeakerType
{
    NPC,
    Player
}

[System.Serializable]
public class DialogueLine
{
    public SpeakerType speaker;
    [TextArea] public string text;
}