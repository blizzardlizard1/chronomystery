using System.Collections.Generic;
using UnityEngine; // For AudioClip

[System.Serializable]
public class TwinePassage
{
    public string pid;
    public string name;

    public string rawText;
    public string cleanedText;

    public List<TwineChoice> choices = new List<TwineChoice>();

    public AudioClip voiceClip;
}

[System.Serializable]
public class TwineChoice
{
    public string text;
    public string targetPassageName;
}