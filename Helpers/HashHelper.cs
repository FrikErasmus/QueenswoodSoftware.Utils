namespace Library.Helpers;
/// <summary>
/// Cryptographic hash and HMAC helpers that return lowercase hexadecimal strings.
/// </summary>
public static class HashHelper
{
    /// <summary>
    /// Computes the SHA-256 hash of a UTF-8 encoded string.
    /// </summary>
    /// <param name="input">The text to hash.</param>
    /// <returns>Lowercase hexadecimal SHA-256 digest.</returns>
    public static string Sha256(string input)
    {
        if (input == null) throw new ArgumentNullException(nameof(input));
        return Sha256(Encoding.UTF8.GetBytes(input));
    }
    /// <summary>
    /// Computes the SHA-256 hash of a byte array.
    /// </summary>
    /// <param name="input">The bytes to hash.</param>
    /// <returns>Lowercase hexadecimal SHA-256 digest.</returns>
    public static string Sha256(byte[] input)
    {
        if (input == null) throw new ArgumentNullException(nameof(input));
        using var sha = SHA256.Create();
        var hash = sha.ComputeHash(input);
        return Convert.ToHexString(hash).ToLowerInvariant();
    }
    /// <summary>
    /// Computes the SHA-512 hash of a UTF-8 encoded string.
    /// </summary>
    /// <param name="input">The text to hash.</param>
    /// <returns>Lowercase hexadecimal SHA-512 digest.</returns>
    public static string Sha512(string input)
    {
        if (input == null) throw new ArgumentNullException(nameof(input));
        return Sha512(Encoding.UTF8.GetBytes(input));
    }
    /// <summary>
    /// Computes the SHA-512 hash of a byte array.
    /// </summary>
    /// <param name="input">The bytes to hash.</param>
    /// <returns>Lowercase hexadecimal SHA-512 digest.</returns>
    public static string Sha512(byte[] input)
    {
        if (input == null) throw new ArgumentNullException(nameof(input));
        using var sha = SHA512.Create();
        var hash = sha.ComputeHash(input);
        return Convert.ToHexString(hash).ToLowerInvariant();
    }
    /// <summary>
    /// Computes an HMAC-SHA256 digest using a UTF-8 encoded secret key.
    /// </summary>
    /// <param name="input">The message to authenticate.</param>
    /// <param name="secret">The shared secret key.</param>
    /// <returns>Lowercase hexadecimal HMAC-SHA256 digest.</returns>
    public static string HmacSha256(string input, string secret)
    {
        if (input == null) throw new ArgumentNullException(nameof(input));
        if (secret == null) throw new ArgumentNullException(nameof(secret));
        var key = Encoding.UTF8.GetBytes(secret);
        var value = Encoding.UTF8.GetBytes(input);
        using var hmac = new HMACSHA256(key);
        return Convert.ToHexString(hmac.ComputeHash(value)).ToLowerInvariant();
    }
    /// <summary>
    /// Verifies that a string's SHA-256 hash matches an expected hexadecimal value.
    /// </summary>
    /// <param name="input">The text to hash and compare.</param>
    /// <param name="expectedHexHash">Expected lowercase or mixed-case hexadecimal digest.</param>
    /// <returns>True when the computed hash equals the expected value; otherwise false.</returns>
    public static bool VerifySha256(string input, string expectedHexHash)
    {
        if (expectedHexHash == null) throw new ArgumentNullException(nameof(expectedHexHash));
        var actual = Sha256(input);
        return string.Equals(actual, expectedHexHash.Trim().ToLowerInvariant(), StringComparison.Ordinal);
    }
}
