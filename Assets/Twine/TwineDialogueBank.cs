using UnityEngine;

[CreateAssetMenu(menuName = "Twine/Dialogue Bank")]
public class TwineDialogueBank : ScriptableObject
{
    [System.Serializable]
    public class Entry
    {
        public TextAsset htmlFile;
        public string requiredTag; // leave empty for fallback
    }

    public Entry[] dialogues;
}