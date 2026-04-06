using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class TwineHTMLParser
{
    public Dictionary<string, TwinePassage> passages = new();
    public string startPassageName = null;

    public void LoadTwine(string html)
    {
        passages.Clear();
        startPassageName = null;

        // 1. Detect startnode PID
        var storyMatch = Regex.Match(html,
            @"<tw-storydata[^>]*startnode=""(\d+)""", RegexOptions.IgnoreCase);

        if (!storyMatch.Success)
        {
            Debug.LogError("TwineHTMLParser: Could not find <tw-storydata> or startnode.");
            return;
        }

        string startPID = storyMatch.Groups[1].Value;
        Debug.Log($"TwineHTMLParser: Found startnode pid = {startPID}");

        // 2. Parse all passages
        var matches = Regex.Matches(html,
            @"<tw-passagedata[^>]*pid=""(\d+)""[^>]*name=""([^""]*)""[^>]*>([\s\S]*?)</tw-passagedata>",
            RegexOptions.IgnoreCase);

        foreach (Match m in matches)
        {
            string pid = m.Groups[1].Value;
            string name = m.Groups[2].Value;
            string text = m.Groups[3].Value;

            var passage = new TwinePassage
            {
                pid = pid,
                name = name,
                rawText = text
            };

            passages[name] = passage;

            // If PID matches startnode, mark this as the start passage
            if (pid == startPID)
            {
                startPassageName = name;
                Debug.Log($"TwineHTMLParser: Start passage resolved to '{name}'");
            }
        }

        // 3. Clean & extract choices
        foreach (var p in passages.Values)
        {
            p.cleanedText = CleanText(p.rawText);
            p.choices = ExtractChoices(p.rawText);
        }
    }

    // Removes HTML entities + clears [[links]]
    private string CleanText(string raw)
    {
        string t = raw;

        // Remove choice markup COMPLETELY from display text
        t = Regex.Replace(t, @"\[\[([^\|\]]+)\|([^\]]+)\]\]", ""); // [[Text|Target]]
        t = Regex.Replace(t, @"\[\[([^\]]+)\]\]", "");             // [[Target]]

        // Decode Twine HTML entities
        t = t.Replace("&quot;", "\"")
             .Replace("&#39;", "'")
             .Replace("&amp;", "&");

        // Strip all HTML tags
        t = Regex.Replace(t, "<.*?>", "");

        return t.Trim();
    }

    // Returns all choices inside a passage
    private List<TwineChoice> ExtractChoices(string raw)
    {
        List<TwineChoice> list = new();

        // Format: [[Choice Text|Target]]
        var linkMatches = Regex.Matches(raw, @"\[\[([^\|\]]+)\|([^\]]+)\]\]");

        foreach (Match m in linkMatches)
        {
            list.Add(new TwineChoice
            {
                text = CleanText(m.Groups[1].Value),
                targetPassageName = m.Groups[2].Value
            });
        }

        // Format: [[Target]]
        var simpleMatches = Regex.Matches(raw, @"\[\[([^\]]+)\]\]");

        foreach (Match m in simpleMatches)
        {
            string target = m.Groups[1].Value;

            if (!list.Exists(c => c.targetPassageName == target))
            {
                list.Add(new TwineChoice
                {
                    text = CleanText(target),
                    targetPassageName = target
                });
            }
        }

        return list;
    }
}