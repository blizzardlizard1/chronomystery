using UnityEngine;

[CreateAssetMenu(menuName = "Twine/Audio Map")]
public class TwineAudioMap : ScriptableObject
{
    [System.Serializable]
    public class Entry
    {
        public string passageName;
        public AudioClip clip;
    }

    public Entry[] entries;
}