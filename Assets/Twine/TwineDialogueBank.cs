using UnityEngine;

[CreateAssetMenu(menuName = "Twine/Dialogue Bank")]
public class TwineDialogueBank : ScriptableObject
{
    [System.Serializable]
    public class Entry
    {
        public TextAsset htmlFile;

        [Header("Scene Conditions")]
        public string requiredTag;

        [Header("Inventory Requirements")]
        public ItemData[] itemsRequiredBeforeDialogue;

        [Header("Rewards")]
        public ItemData[] itemsGivenToPlayer;

        [Header("Costs")]
        public ItemData[] itemsTakenFromPlayer;

        [Header("Destroys Itself")]
        public bool destroyItself = false;
    }

    public Entry[] dialogues;
}