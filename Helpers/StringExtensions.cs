namespace QueenswoodSoftware.Utils.Helpers;

public static class StringExtensions
{
    public static string Truncate(this string value, int maxLength, string suffix = "...")
    {
        if (string.IsNullOrEmpty(value) || value.Length <= maxLength) return value;
        return value[..(maxLength - suffix.Length)] + suffix;
    }

    public static string ToPascalCase(this string value)
    {
        if (string.IsNullOrWhiteSpace(value)) return value;
        var words = value.Split([' ', '_', '-'], StringSplitOptions.RemoveEmptyEntries);
        return string.Concat(words.Select(w => char.ToUpper(w[0]) + w[1..].ToLower()));
    }

    public static string ToCamelCase(this string value)
    {
        var pascal = value.ToPascalCase();
        if (string.IsNullOrEmpty(pascal)) return pascal;
        return char.ToLower(pascal[0]) + pascal[1..];
    }

    public static string ToSnakeCase(this string value)
    {
        if (string.IsNullOrWhiteSpace(value)) return value;
        return Regex.Replace(value, @"([a-z])([A-Z])", "$1_$2")
                    .Replace(' ', '_')
                    .Replace('-', '_')
                    .ToLower();
    }

    public static int WordCount(this string value)
    {
        if (string.IsNullOrWhiteSpace(value)) return 0;
        return value.Split([' ', '\t', '\n', '\r'], StringSplitOptions.RemoveEmptyEntries).Length;
    }

    public static string RemoveSpecialCharacters(this string value, string allow = "")
    {
        if (string.IsNullOrEmpty(value)) return value;
        return new string(value.Where(c => char.IsLetterOrDigit(c) || allow.Contains(c)).ToArray());
    }

    public static bool IsNullOrWhiteSpace(this string? value) => string.IsNullOrWhiteSpace(value);

    public static string? NullIfEmpty(this string? value) =>
        string.IsNullOrWhiteSpace(value) ? null : value;

    public static string Repeat(this string value, int count) =>
        string.Concat(Enumerable.Repeat(value, count));

    public static string ReverseString(this string value)
    {
        if (string.IsNullOrEmpty(value)) return value;
        return new string(value.Reverse().ToArray());
    }

    public static bool ContainsIgnoreCase(this string value, string search) =>
        value.Contains(search, StringComparison.OrdinalIgnoreCase);

    public static string ToTitleCase(this string value)
    {
        if (string.IsNullOrWhiteSpace(value)) return value;
        return System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(value.ToLower());
    }
}