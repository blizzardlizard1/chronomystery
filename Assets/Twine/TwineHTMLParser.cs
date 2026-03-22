using UnityEngine;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public static class TwineHTMLParser
{
    public static Dictionary<string, TwinePassage> Parse(string html)
    {
        var passages = new Dictionary<string, TwinePassage>();

        // Match Twine passages (both formats)
        var passageRegex = new Regex(
            "<tw-passagedata[^>]*name=\"([^\"]+)\"[^>]*>([\\s\\S]*?)</tw-passagedata>|" +
            "<div[^>]*tiddler=\"([^\"]+)\"[^>]*>([\\s\\S]*?)</div>"
        );

        foreach (Match m in passageRegex.Matches(html))
        {
            // Sugarcube style
            string title = m.Groups[1].Success ? m.Groups[1].Value : m.Groups[3].Value;
            string rawText = m.Groups[2].Success ? m.Groups[2].Value : m.Groups[4].Value;

            var p = new TwinePassage();
            p.title = title;
            p.text = StripTags(rawText);

            // Find choices [[label->passage]]
            var linkRegex = new Regex("\\[\\[([^\\]]+?)(?:->([^\\]]+))?\\]\\]");
            foreach (Match link in linkRegex.Matches(rawText))
            {
                string label = link.Groups[1].Value;
                string target = link.Groups[2].Success ? link.Groups[2].Value : label;

                p.choices.Add(new TwineChoice { label = label, targetPassage = target });
            }

            passages[title] = p;
        }

        return passages;
    }

    private static string StripTags(string input)
    {
        return Regex.Replace(input, "<.*?>", "").Trim();
    }
}