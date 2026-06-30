using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace Library.Helpers;

/// <summary>
/// Suppresses device push (FCM/Expo) when a user is in an out-of-office date range.
/// </summary>
public static class PushNotificationPolicy
{
    public static bool IsOutOfOfficeActive(DateTime? from, DateTime? to, DateTime asOf)
    {
        if (from == null || to == null)
        {
            return false;
        }

        var day = asOf.Date;
        return day >= from.Value.Date && day <= to.Value.Date;
    }

    public static bool IsDevicePushSuppressed(IConfiguration config, string userId, DateTime? asOf = null)
    {
        if (string.IsNullOrWhiteSpace(userId))
        {
            return false;
        }

        var (from, to) = QueryOutOfOfficeByUserId(config, userId.Trim());
        return IsOutOfOfficeActive(from, to, asOf ?? DateTime.Now);
    }

    public static bool IsDevicePushSuppressedByUserKey(IConfiguration config, string key, DateTime? asOf = null)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            return false;
        }

        var (from, to) = QueryOutOfOffice(
            config,
            """
            SELECT TOP 1 u.OutOfOfficeFrom, u.OutOfOfficeTo
            FROM [survey].[Users] u (NOLOCK)
            WHERE u.[Key] = @Key
            """,
            cmd => cmd.Parameters.AddWithValue("@Key", key.Trim()));

        return IsOutOfOfficeActive(from, to, asOf ?? DateTime.Now);
    }

    public static bool IsDevicePushSuppressedByToken(IConfiguration config, string token, DateTime? asOf = null)
    {
        if (string.IsNullOrWhiteSpace(token))
        {
            return false;
        }

        var (from, to) = QueryOutOfOffice(
            config,
            """
            SELECT TOP 1 u.OutOfOfficeFrom, u.OutOfOfficeTo
            FROM [survey].[Setting] s (NOLOCK)
            INNER JOIN [survey].[Users] u (NOLOCK) ON u.Id = s.[User]
            WHERE s.[Token] = @Token
              AND s.[Enable] = 1
            """,
            cmd => cmd.Parameters.AddWithValue("@Token", token.Trim()));

        return IsOutOfOfficeActive(from, to, asOf ?? DateTime.Now);
    }

    private static (DateTime? From, DateTime? To) QueryOutOfOfficeByUserId(IConfiguration config, string userId) =>
        QueryOutOfOffice(
            config,
            """
            SELECT TOP 1 OutOfOfficeFrom, OutOfOfficeTo
            FROM [survey].[Users] (NOLOCK)
            WHERE Id = @UserId
            """,
            cmd => cmd.Parameters.AddWithValue("@UserId", userId));

    private static (DateTime? From, DateTime? To) QueryOutOfOffice(
        IConfiguration config,
        string sql,
        Action<SqlCommand> bind)
    {
        var connectionString = config["ConnectionStrings:SurveyBridge"];
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            Log.Warning("PushNotificationPolicy: ConnectionStrings:SurveyBridge is not configured");
            return (null, null);
        }

        try
        {
            using var connection = new SqlConnection(connectionString);
            using var cmd = new SqlCommand(sql, connection);
            bind(cmd);
            connection.Open();

            using var reader = cmd.ExecuteReader();
            if (!reader.Read())
            {
                return (null, null);
            }

            return (ReadDate(reader, 0), ReadDate(reader, 1));
        }
        catch (Exception ex)
        {
            Log.Warning(ex, "PushNotificationPolicy: failed to read out-of-office dates");
            return (null, null);
        }
    }

    private static DateTime? ReadDate(SqlDataReader reader, int ordinal)
    {
        if (reader.IsDBNull(ordinal))
        {
            return null;
        }

        return reader.GetDateTime(ordinal);
    }
}
