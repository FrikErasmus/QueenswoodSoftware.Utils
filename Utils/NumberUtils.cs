namespace QueenswoodSoftware.Utils.Utils;

public static class NumberUtils
{
    public static string FormatCurrency(decimal amount, string symbol = "R", int decimals = 2) =>
        $"{symbol}{amount.ToString($"N{decimals}")}";

    public static decimal Clamp(decimal value, decimal min, decimal max) =>
        value < min ? min : value > max ? max : value;

    public static int Clamp(int value, int min, int max) =>
        value < min ? min : value > max ? max : value;

    public static decimal RoundTo(decimal value, int decimals) =>
        Math.Round(value, decimals, MidpointRounding.AwayFromZero);

    public static bool IsNumeric(string? value) =>
        !string.IsNullOrWhiteSpace(value) && decimal.TryParse(value, out _);

    public static bool IsInteger(string? value) =>
        !string.IsNullOrWhiteSpace(value) && long.TryParse(value, out _);

    public static decimal Percentage(decimal value, decimal total) =>
        total == 0 ? 0 : Math.Round(value / total * 100, 2, MidpointRounding.AwayFromZero);

    public static decimal PercentageOf(decimal percent, decimal total) =>
        Math.Round(percent / 100 * total, 2, MidpointRounding.AwayFromZero);

    public static string FormatFileSize(long bytes)
    {
        string[] units = ["B", "KB", "MB", "GB", "TB"];
        var size = (double)bytes;
        var unit = 0;
        while (size >= 1024 && unit < units.Length - 1) { size /= 1024; unit++; }
        return $"{size:F2} {units[unit]}";
    }

    public static int SafeInt(string? value, int defaultValue = 0) =>
        int.TryParse(value, out var result) ? result : defaultValue;

    public static decimal SafeDecimal(string? value, decimal defaultValue = 0) =>
        decimal.TryParse(value, out var result) ? result : defaultValue;
}