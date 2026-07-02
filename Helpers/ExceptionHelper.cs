namespace QueenswoodSoftware.Utils.Helpers;

public static class ExceptionHelper
{
    public static string GetFullMessage(Exception ex)
    {
        var messages = new List<string>();
        var current = ex;
        while (current is not null)
        {
            if (!string.IsNullOrWhiteSpace(current.Message))
                messages.Add(current.Message);
            current = current.InnerException;
        }
        return string.Join(" → ", messages);
    }

    public static IEnumerable<Exception> Flatten(Exception ex)
    {
        var current = ex;
        while (current is not null)
        {
            yield return current;
            current = current.InnerException;
        }
    }

    public static string FormatForLogging(Exception ex)
    {
        var sb = new StringBuilder();
        sb.AppendLine($"Exception: {ex.GetType().FullName}");
        sb.AppendLine($"Message: {GetFullMessage(ex)}");
        if (ex.StackTrace is not null)
            sb.AppendLine($"StackTrace: {ex.StackTrace}");
        return sb.ToString();
    }

    public static bool IsTransient(Exception ex) =>
        ex is TimeoutException
        or OperationCanceledException
        or System.Net.Sockets.SocketException
        or System.IO.IOException;

    public static T? TryGet<T>(Func<T> func, T? defaultValue = default)
    {
        try { return func(); }
        catch { return defaultValue; }
    }
}