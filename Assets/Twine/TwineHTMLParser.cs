using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class TwineHTMLParser
{
    public Dictionary<string, TwinePassage> passages = new();
    public string startPassageName;

    public void LoadTwine(string html)
    {
        passages.Clear();
        startPassageName = null;

        var storyMatch = Regex.Match(html,
            @"<tw-storydata[^>]*startnode=""(\d+)""",
            RegexOptions.IgnoreCase);

        if (!storyMatch.Success)
        {
            Debug.LogError("No startnode found.");
            return;
        }

        string startPID = storyMatch.Groups[1].Value;

        var matches = Regex.Matches(html,
            @"<tw-passagedata[^>]*pid=""(\d+)""[^>]*name=""([^""]*)""[^>]*>([\s\S]*?)</tw-passagedata>",
            RegexOptions.IgnoreCase);

        foreach (Match m in matches)
        {
            var p = new TwinePassage
            {
                pid = m.Groups[1].Value,
                name = CleanName(m.Groups[2].Value),
                rawText = m.Groups[3].Value
            };

            passages[p.name] = p;

            if (p.pid == startPID)
                startPassageName = p.name;
        }

        foreach (var p in passages.Values)
        {
            p.cleanedText = CleanText(p.rawText);
            p.choices = ExtractChoices(p.rawText);
        }
    }

    private string CleanText(string raw)
    {
        string t = raw;

        t = Regex.Replace(t, @"\[\[[^\]]+\]\]", "");
        t = t.Replace("&quot;", "\"")
             .Replace("&#39;", "'")
             .Replace("&amp;", "&");

        t = Regex.Replace(t, "<.*?>", "");

        return t.Trim();
    }

    private List<TwineChoice> ExtractChoices(string raw)
    {
        List<TwineChoice> list = new();

        var matches = Regex.Matches(raw, @"\[\[([^\]]+)\]\]");

        foreach (Match m in matches)
        {
            string content = m.Groups[1].Value;

            string text;
            string target;

            if (content.Contains("|"))
            {
                var parts = content.Split('|');
                text = CleanChoiceText(parts[0]);   // what player sees
                target = CleanName(parts[1]);       // passage lookup key
            }
            else
            {
                text = CleanChoiceText(content);
                target = CleanName(content);
            }

            list.Add(new TwineChoice
            {
                text = text,
                targetPassageName = target
            });
        }

        return list;
    }

    private string CleanChoiceText(string raw)
    {
        string t = raw;

        // Decode HTML entities
        t = t.Replace("&quot;", "\"")
             .Replace("&#39;", "'")
             .Replace("&amp;", "&");

        // Strip HTML tags
        t = Regex.Replace(t, "<.*?>", "");

        return t.Trim();
    }

    private string CleanName(string raw)
    {
        if (string.IsNullOrEmpty(raw))
            return raw;

        string t = raw;

        t = t.Replace("&quot;", "\"")
             .Replace("&#39;", "'")
             .Replace("&amp;", "&");

        return t.Trim();
    }



}