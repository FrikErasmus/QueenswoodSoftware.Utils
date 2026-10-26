namespace QueenswoodSoftware.Utils.Helpers;

/// <summary>
/// Contract for AES-based string encryption and decryption using the configured library key.
/// </summary>
public interface ISecure
{
    /// <summary>
    /// Encrypts clear text using the resolved encryption key and salt-derived AES parameters.
    /// </summary>
    /// <param name="clearText">Plain text to encrypt.</param>
    /// <returns>Base64-encoded ciphertext.</returns>
    public string Encrypt(string clearText);

    /// <summary>
    /// Decrypts ciphertext produced by <see cref="Encrypt(string)"/>.
    /// </summary>
    /// <param name="clearText">Base64-encoded ciphertext to decrypt.</param>
    /// <returns>Decrypted plain text.</returns>
    public string Decrypt(string clearText);
}

/// <summary>
/// AES and legacy Triple-DES encryption helpers with key resolution from environment or legacy default.
/// </summary>
public class Secure
{
    private const string LegacyEncryptionKey = "MAKV2SPBNI99212";
    private static readonly byte[] Salt = { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 };

    /// <summary>
    /// Key supplied by the host at startup. Checked after <c>LIBRARY_ENCRYPTION_KEY</c> and before the legacy default.
    /// </summary>
    public static string? ConfiguredKey { get; set; }

    /// <summary>
    /// Resolves the encryption passphrase from <c>LIBRARY_ENCRYPTION_KEY</c>, then <see cref="ConfiguredKey"/>,
    /// then the legacy default.
    /// </summary>
    /// <returns>Encryption key string used for Rfc2898DeriveBytes.</returns>
    private static string ResolveEncryptionKey()
    {
        var key = Environment.GetEnvironmentVariable("LIBRARY_ENCRYPTION_KEY");
        if (!string.IsNullOrWhiteSpace(key))
            return key;
        if (!string.IsNullOrWhiteSpace(ConfiguredKey))
            return ConfiguredKey;
        Log.Warning("LIBRARY_ENCRYPTION_KEY and ConfiguredKey are not set; using legacy default encryption key.");
        return LegacyEncryptionKey;
    }

    /// <summary>
    /// Encrypts clear text using AES and a key derived from <see cref="ResolveEncryptionKey"/>.
    /// </summary>
    /// <param name="clearText">Plain text to encrypt.</param>
    /// <returns>Base64-encoded ciphertext.</returns>
    public string Encrypt(string clearText)
    {
        var clearBytes = Encoding.Unicode.GetBytes(clearText);
        using var encryptor = Aes.Create();
        var pdb = new Rfc2898DeriveBytes(ResolveEncryptionKey(), Salt);
        encryptor.Key = pdb.GetBytes(32);
        encryptor.IV = pdb.GetBytes(16);
        using var ms = new MemoryStream();
        using (var cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
        {
            cs.Write(clearBytes, 0, clearBytes.Length);
            cs.Close();
        }
        clearText = Convert.ToBase64String(ms.ToArray());
        return clearText;
    }

    /// <summary>
    /// Decrypts AES ciphertext produced by the instance <see cref="Encrypt(string)"/> method.
    /// </summary>
    /// <param name="cipherText">Base64-encoded ciphertext.</param>
    /// <returns>Decrypted plain text.</returns>
    public string Decrypt(string cipherText)
    {
        var cipherBytes = Convert.FromBase64String(cipherText);
        using var encryptor = Aes.Create();
        var pdb = new Rfc2898DeriveBytes(ResolveEncryptionKey(), Salt);
        encryptor.Key = pdb.GetBytes(32);
        encryptor.IV = pdb.GetBytes(16);
        using var ms = new MemoryStream();
        using (var cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
        {
            cs.Write(cipherBytes, 0, cipherBytes.Length);
            cs.Close();
        }
        cipherText = Encoding.Unicode.GetString(ms.ToArray());
        return cipherText;
    }

    /// <summary>
    /// Encrypts plain text using legacy Triple-DES with an MD5-derived key from the supplied passphrase.
    /// </summary>
    /// <param name="source">Clear text to encrypt.</param>
    /// <param name="key">Passphrase used to derive the encryption key.</param>
    /// <returns>Base64-encoded ciphertext.</returns>
    [Obsolete("Uses legacy MD5/3DES. Prefer instance Encrypt/Decrypt with AES and LIBRARY_ENCRYPTION_KEY.")]
    public static string Encrypt(string source, string key)
    {
        var desCryptoProvider = new TripleDESCryptoServiceProvider();
        var hashMd5Provider = new MD5CryptoServiceProvider();
        var byteHash = hashMd5Provider.ComputeHash(Encoding.UTF8.GetBytes(key));
        desCryptoProvider.Key = byteHash;
        desCryptoProvider.Mode = CipherMode.ECB; //CBC, CFB
        var byteBuff = Encoding.UTF8.GetBytes(source);
        var encoded =
            Convert.ToBase64String(desCryptoProvider.CreateEncryptor().TransformFinalBlock(byteBuff, 0, byteBuff.Length));
        return encoded;
    }

    /// <summary>
    /// Decrypts legacy Triple-DES ciphertext using an MD5-derived key from the supplied passphrase.
    /// </summary>
    /// <param name="encodedText">Base64-encoded ciphertext.</param>
    /// <param name="key">Passphrase used to derive the decryption key.</param>
    /// <returns>Decrypted clear text.</returns>
    [Obsolete("Uses legacy MD5/3DES. Prefer instance Encrypt/Decrypt with AES and LIBRARY_ENCRYPTION_KEY.")]
    public static string Decrypt(string encodedText, string key)
    {
        var desCryptoProvider = new TripleDESCryptoServiceProvider();
        var hashMd5Provider = new MD5CryptoServiceProvider();
        var byteHash = hashMd5Provider.ComputeHash(Encoding.UTF8.GetBytes(key));
        desCryptoProvider.Key = byteHash;
        desCryptoProvider.Mode = CipherMode.ECB; //CBC, CFB
        var byteBuff = Convert.FromBase64String(encodedText);
        var plaintext = Encoding.UTF8.GetString(desCryptoProvider.CreateDecryptor().TransformFinalBlock(byteBuff, 0, byteBuff.Length));
        return plaintext;
    }
}