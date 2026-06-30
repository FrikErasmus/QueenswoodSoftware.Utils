using System.ComponentModel;

namespace Library.Helpers;

public static class EnumHelper
{
    public static string GetDescription(Enum value)
    {
        var field = value.GetType().GetField(value.ToString());
        if (field is null) return value.ToString();
        var attr = field.GetCustomAttributes(typeof(DescriptionAttribute), false)
                        .OfType<DescriptionAttribute>()
                        .FirstOrDefault();
        return attr?.Description ?? value.ToString();
    }

    public static T ParseOrDefault<T>(string value, T defaultValue = default!) where T : struct, Enum
    {
        return Enum.TryParse<T>(value, ignoreCase: true, out var result) ? result : defaultValue;
    }

    public static IEnumerable<T> GetValues<T>() where T : struct, Enum =>
        Enum.GetValues<T>();

    public static List<NameValue> ToNameValueList<T>() where T : struct, Enum =>
        Enum.GetValues<T>()
            .Select(e => new NameValue { Name = GetDescription(e), Value = Convert.ToInt32(e).ToString() })
            .ToList();

    public static bool IsDefined<T>(int value) where T : struct, Enum =>
        Enum.IsDefined(typeof(T), value);

    public static T? FromDescription<T>(string description) where T : struct, Enum
    {
        foreach (var value in Enum.GetValues<T>())
        {
            if (GetDescription(value).Equals(description, StringComparison.OrdinalIgnoreCase))
                return value;
        }
        return null;
    }
}
