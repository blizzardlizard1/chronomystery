using UnityEngine;

[CreateAssetMenu(menuName = "Twine/Audio Map")]
public class TwineAudioMap : ScriptableObject
{
    [System.Serializable]
    public class Entry
    {
        [Tooltip("Twine passage pid, like 1, 2, 14")]
        public string passagePID;
        public AudioClip clip;
    }

    public Entry[] entries;

    public AudioClip GetClip(string pid)
    {
        if (string.IsNullOrWhiteSpace(pid) || entries == null)
            return null;

        foreach (var entry in entries)
        {
            if (entry != null && entry.passagePID == pid)
                return entry.clip;
        }

        return null;
    }
}