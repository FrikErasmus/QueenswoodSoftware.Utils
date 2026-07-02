namespace QueenswoodSoftware.Utils.Utils;

public static class TimeZoneUtils
{
    private const string SaTimeZoneId = "South Africa Standard Time";

    public static DateTime ToUtc(DateTime localTime, string timeZoneId)
    {
        var tz = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
        return TimeZoneInfo.ConvertTimeToUtc(localTime, tz);
    }

    public static DateTime FromUtc(DateTime utcTime, string timeZoneId)
    {
        var tz = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
        return TimeZoneInfo.ConvertTimeFromUtc(utcTime, tz);
    }

    public static DateTime ToSouthAfrica(DateTime utcTime) =>
        FromUtc(utcTime, SaTimeZoneId);

    public static DateTime SouthAfricaNow() =>
        ToSouthAfrica(DateTime.UtcNow);

    public static DateTime Convert(DateTime time, string fromTimeZoneId, string toTimeZoneId)
    {
        var from = TimeZoneInfo.FindSystemTimeZoneById(fromTimeZoneId);
        var to = TimeZoneInfo.FindSystemTimeZoneById(toTimeZoneId);
        return TimeZoneInfo.ConvertTime(time, from, to);
    }

    public static TimeSpan GetUtcOffset(string timeZoneId)
    {
        var tz = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
        return tz.GetUtcOffset(DateTime.UtcNow);
    }

    public static IEnumerable<string> GetAllTimeZoneIds() =>
        TimeZoneInfo.GetSystemTimeZones().Select(tz => tz.Id);

    public static bool IsValidTimeZoneId(string timeZoneId)
    {
        try { TimeZoneInfo.FindSystemTimeZoneById(timeZoneId); return true; }
        catch { return false; }
    }
}