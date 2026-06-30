using System.Web;

namespace Library.Helpers;

public static class HtmlHelper
{
    private static readonly Regex StripTagsRegex = new(@"<[^>]*>", RegexOptions.Compiled);

    public static string StripTags(string html)
    {
        if (string.IsNullOrWhiteSpace(html)) return html;
        return StripTagsRegex.Replace(html, string.Empty);
    }

    public static string Encode(string value) => HttpUtility.HtmlEncode(value);

    public static string Decode(string value) => HttpUtility.HtmlDecode(value);

    public static string Sanitize(string html, IEnumerable<string>? allowedTags = null)
    {
        if (string.IsNullOrWhiteSpace(html)) return html;

        var allowed = (allowedTags ?? []).Select(t => t.ToLowerInvariant()).ToHashSet();
        if (allowed.Count == 0) return StripTags(html);

        return Regex.Replace(html, @"<(/?)(\w+)([^>]*)>", m =>
        {
            var tag = m.Groups[2].Value.ToLowerInvariant();
            return allowed.Contains(tag) ? m.Value : string.Empty;
        });
    }

    public static string TextToHtml(string text)
    {
        if (string.IsNullOrEmpty(text)) return text;
        return HttpUtility.HtmlEncode(text).Replace("\n", "<br/>");
    }

    public static bool ContainsHtml(string value) =>
        !string.IsNullOrEmpty(value) && StripTagsRegex.IsMatch(value);
}
