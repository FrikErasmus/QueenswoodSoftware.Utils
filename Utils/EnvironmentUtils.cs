namespace Library.Utils;

public static class EnvironmentUtils
{
    public static string MachineName => Environment.MachineName;

    public static string UserName => Environment.UserName;

    public static string OsVersion => Environment.OSVersion.ToString();

    public static bool Is64Bit => Environment.Is64BitProcess;

    public static string CurrentDirectory => Environment.CurrentDirectory;

    public static string? GetVariable(string name) => Environment.GetEnvironmentVariable(name);

    public static string GetVariable(string name, string defaultValue) =>
        Environment.GetEnvironmentVariable(name) ?? defaultValue;

    public static bool IsRunningInContainer() =>
        Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true"
        || File.Exists("/.dockerenv");

    public static bool IsDevelopment() =>
        GetEnvironmentName().Equals("Development", StringComparison.OrdinalIgnoreCase);

    public static bool IsProduction() =>
        GetEnvironmentName().Equals("Production", StringComparison.OrdinalIgnoreCase);

    public static string GetEnvironmentName() =>
        Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")
        ?? Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT")
        ?? "Production";

    public static int ProcessorCount => Environment.ProcessorCount;

    public static long WorkingSetBytes => Environment.WorkingSet;

    public static string FormatWorkingSet() =>
        NumberUtils.FormatFileSize(Environment.WorkingSet);
}
