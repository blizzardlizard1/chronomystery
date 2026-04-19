using System;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Twine/Dialogue Bank")]
public class TwineDialogueBank : ScriptableObject
{
    [System.Serializable]
    public class Entry
    {
        public TextAsset htmlFile;
        public TwineAudioMap audioMap;

        [Header("Scene Conditions")]
        public string requiredTag;

        [Header("Inventory Requirements")]
        public ItemData[] itemsRequiredBeforeDialogue;

        [Header("Rewards")]
        public ItemData[] itemsGivenToPlayer;

        [Header("Costs")]
        public ItemData[] itemsTakenFromPlayer;

        [Header("Final State")]
        public bool isFinalEntry;

        public bool destroyItself;
        public String changeTag;
        public bool onInteract;

    }

    public Entry[] dialogues;
}