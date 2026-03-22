using UnityEngine;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Net;   // For HtmlDecode

public static class TwineHTMLParser
{
    public static Dictionary<string, TwinePassage> Parse(string html)
    {
        var passages = new Dictionary<string, TwinePassage>();

        var passageRegex = new Regex(
            "<tw-passagedata[^>]*name=\"([^\"]+)\"[^>]*>([\\s\\S]*?)</tw-passagedata>|" +
            "<div[^>]*tiddler=\"([^\"]+)\"[^>]*>([\\s\\S]*?)</div>"
        );

        foreach (Match m in passageRegex.Matches(html))
        {
            string title = m.Groups[1].Success ? m.Groups[1].Value : m.Groups[3].Value;
            string rawText = m.Groups[2].Success ? m.Groups[2].Value : m.Groups[4].Value;

            var passage = new TwinePassage();
            passage.title = title;

            // Clean passage dialogue text
            passage.text = CleanText(rawText);

            // Extract choices
            var linkRegex = new Regex("\\[\\[([^\\]]+?)(?:->([^\\]]+))?\\]\\]");
            foreach (Match link in linkRegex.Matches(rawText))
            {
                string rawLabel  = link.Groups[1].Value;
                string rawTarget = link.Groups[2].Success ? link.Groups[2].Value : rawLabel;

                string cleanLabel = CleanChoice(rawLabel);

                passage.choices.Add(new TwineChoice
                {
                    label = cleanLabel,   // ❤️ Cleaned!
                    targetPassage = rawTarget
                });
            }

            passages[title] = passage;
        }

        return passages;
    }

    /// <summary>
    /// Cleans Twine passage display text.
    /// - HTML decode
    /// - Remove [[links]]
    /// - Remove HTML tags
    /// - Trim whitespace
    /// </summary>
    private static string CleanText(string input)
    {
        if (string.IsNullOrEmpty(input))
            return "";

        string decoded = WebUtility.HtmlDecode(input);
        decoded = Regex.Replace(decoded, "\\[\\[(.*?)\\]\\]", "");
        decoded = Regex.Replace(decoded, "<.*?>", "");

        return decoded.Trim();
    }

    /// <summary>
    /// Cleans Twine choice label text.
    /// - HTML decode
    /// - Remove HTML tags
    /// - Trim whitespace
    /// (Do NOT remove [[...]] since the label is already extracted)
    /// </summary>
    private static string CleanChoice(string input)
    {
        if (string.IsNullOrEmpty(input))
            return "";

        string decoded = WebUtility.HtmlDecode(input);
        decoded = Regex.Replace(decoded, "<.*?>", "");

        return decoded.Trim();
    }
}