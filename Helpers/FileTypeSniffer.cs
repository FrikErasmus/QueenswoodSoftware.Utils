namespace QueenswoodSoftware.Utils.Helpers;

/// <summary>
/// Identifies common file types from magic-byte signatures at the start of a buffer.
/// </summary>
public static class FileTypeSniffer
{
    private static readonly Dictionary<string, byte[][]> Signatures = new(StringComparer.OrdinalIgnoreCase)
    {
        { "pdf", new[] { new byte[] { 0x25, 0x50, 0x44, 0x46 } } },
        { "png", new[] { new byte[] { 0x89, 0x50, 0x4E, 0x47 } } },
        { "jpg", new[] { new byte[] { 0xFF, 0xD8, 0xFF } } },
        { "jpeg", new[] { new byte[] { 0xFF, 0xD8, 0xFF } } },
        { "gif", new[] { new byte[] { 0x47, 0x49, 0x46, 0x38 } } },
        { "tif", new[] { new byte[] { 0x49, 0x49, 0x2A, 0x00 }, new byte[] { 0x4D, 0x4D, 0x00, 0x2A } } },
        { "tiff", new[] { new byte[] { 0x49, 0x49, 0x2A, 0x00 }, new byte[] { 0x4D, 0x4D, 0x00, 0x2A } } },
        { "zip", new[] { new byte[] { 0x50, 0x4B, 0x03, 0x04 } } }
    };

    /// <summary>
    /// Determines whether file bytes match the magic signature for the given extension.
    /// </summary>
    /// <param name="fileBytes">File content bytes to inspect.</param>
    /// <param name="extension">File extension with or without a leading dot (e.g. pdf, .png).</param>
    /// <returns>True when a known signature for the extension matches the buffer start; false when unknown or mismatched.</returns>
    public static bool MatchesExtension(byte[] fileBytes, string extension)
    {
        if (fileBytes == null) throw new ArgumentNullException(nameof(fileBytes));
        if (string.IsNullOrWhiteSpace(extension)) return false;
        var normalized = extension.Trim().TrimStart('.');
        if (!Signatures.TryGetValue(normalized, out var signatureList))
            return false;
        return signatureList.Any(signature => StartsWith(fileBytes, signature));
    }

    /// <summary>
    /// Detects the first matching file extension from known magic-byte signatures.
    /// </summary>
    /// <param name="fileBytes">File content bytes to inspect.</param>
    /// <returns>Matched extension without a leading dot, or null when no signature matches.</returns>
    public static string? DetectExtension(byte[] fileBytes)
    {
        if (fileBytes == null) throw new ArgumentNullException(nameof(fileBytes));
        foreach (var kvp in Signatures)
        {
            if (kvp.Value.Any(signature => StartsWith(fileBytes, signature)))
                return kvp.Key;
        }
        return null;
    }

    /// <summary>
    /// Tests whether a byte buffer begins with the specified signature bytes.
    /// </summary>
    /// <param name="input">Buffer to test.</param>
    /// <param name="signature">Expected leading byte sequence.</param>
    /// <returns>True when <paramref name="input"/> is at least as long as <paramref name="signature"/> and bytes match.</returns>
    private static bool StartsWith(byte[] input, byte[] signature)
    {
        if (input.Length < signature.Length) return false;
        for (var i = 0; i < signature.Length; i++)
        {
            if (input[i] != signature[i]) return false;
        }
        return true;
    }
}