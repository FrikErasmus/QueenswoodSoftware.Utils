namespace Library.Helpers;

public static class RandomHelper
{
    private static readonly Random Rng = new();
    private const string Alpha = "abcdefghijklmnopqrstuvwxyz";
    private const string Upper = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private const string Digits = "0123456789";
    private const string Special = "!@#$%^&*()-_=+[]{}|;:,.<>?";

    public static string AlphaNumeric(int length)
    {
        var chars = Alpha + Upper + Digits;
        return new string(Enumerable.Range(0, length).Select(_ => chars[Rng.Next(chars.Length)]).ToArray());
    }

    public static string Numeric(int length)
    {
        return new string(Enumerable.Range(0, length).Select(_ => Digits[Rng.Next(Digits.Length)]).ToArray());
    }

    public static string WithSpecialChars(int length)
    {
        var chars = Alpha + Upper + Digits + Special;
        return new string(Enumerable.Range(0, length).Select(_ => chars[Rng.Next(chars.Length)]).ToArray());
    }

    public static int Between(int min, int max) => Rng.Next(min, max + 1);

    public static double Between(double min, double max) => min + (Rng.NextDouble() * (max - min));

    public static T Pick<T>(IList<T> items) => items[Rng.Next(items.Count)];

    public static bool Chance(int percent) => Rng.Next(100) < percent;

    public static string Hex(int length) =>
        string.Concat(Enumerable.Range(0, length).Select(_ => Rng.Next(16).ToString("x")));
}
