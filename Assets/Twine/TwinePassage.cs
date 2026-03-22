using System.Collections.Generic;

[System.Serializable]
public class TwinePassage
{
    public string title;
    public string text;
    public List<TwineChoice> choices = new();
}

[System.Serializable]
public class TwineChoice
{
    public string label;
    public string targetPassage;
}