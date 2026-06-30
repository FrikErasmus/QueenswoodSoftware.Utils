using System.IdentityModel.Tokens.Jwt;

namespace Library.Helpers;

public static class TokenHelper
{
    public static IDictionary<string, object?> GetClaims(string token)
    {
        var handler = new JwtSecurityTokenHandler();

        if (!handler.CanReadToken(token))
            return new Dictionary<string, object?>();

        var jwt = handler.ReadJwtToken(token);
        return jwt.Claims.ToDictionary(c => c.Type, c => (object?)c.Value);
    }

    public static string? GetClaim(string token, string claimType)
    {
        var handler = new JwtSecurityTokenHandler();
        if (!handler.CanReadToken(token)) return null;
        var jwt = handler.ReadJwtToken(token);
        return jwt.Claims.FirstOrDefault(c => c.Type == claimType)?.Value;
    }

    public static DateTime? GetExpiry(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        if (!handler.CanReadToken(token)) return null;
        return handler.ReadJwtToken(token).ValidTo;
    }

    public static bool IsExpired(string token)
    {
        var expiry = GetExpiry(token);
        return expiry is null || expiry.Value < DateTime.UtcNow;
    }

    public static bool IsValid(string token)
    {
        if (string.IsNullOrWhiteSpace(token)) return false;
        try
        {
            var handler = new JwtSecurityTokenHandler();
            return handler.CanReadToken(token) && !IsExpired(token);
        }
        catch
        {
            return false;
        }
    }

    public static string? GetSubject(string token) => GetClaim(token, "sub");

    public static string? GetEmail(string token) =>
        GetClaim(token, "email") ?? GetClaim(token, "unique_name");
}
