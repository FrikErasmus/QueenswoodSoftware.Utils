namespace QueenswoodSoftware.Utils.Helpers;

public static class DataEncryption
{
    public const string PrimaryKeySetting = "Security:EncryptionKey";
    public const string LegacyKeySetting = "Security:LegacyEncryptionKey";
    public const string ExternalStorageKeySetting = "ExternalStorage:EncryptionKey";

    public static string ResolveEncryptKey(IConfiguration config)
    {
        var key = config[PrimaryKeySetting]
            ?? config[ExternalStorageKeySetting]
            ?? config[LegacyKeySetting];

        if (string.IsNullOrWhiteSpace(key))
        {
            throw new InvalidOperationException(
                "Data encryption is not configured. Set Security:EncryptionKey (or LegacyEncryptionKey during migration).");
        }

        return key;
    }

    public static string Encrypt(IConfiguration config, string plainText)
    {
        return new ConvertPassword().Encrypt(plainText, ResolveEncryptKey(config));
    }

    public static bool TryDecrypt(IConfiguration config, string cipherText, out string plainText)
    {
        plainText = string.Empty;
        foreach (var key in GetDecryptKeys(config))
        {
            try
            {
                plainText = new ConvertPassword().Decrypt(cipherText, key);
                return true;
            }
            catch (CryptographicException)
            {
                // try next key
            }
            catch (FormatException)
            {
                return false;
            }
        }

        return false;
    }

    public static IEnumerable<string> GetDecryptKeys(IConfiguration config)
    {
        var seen = new HashSet<string>(StringComparer.Ordinal);
        foreach (var setting in new[] { PrimaryKeySetting, ExternalStorageKeySetting, LegacyKeySetting })
        {
            var key = config[setting];
            if (!string.IsNullOrWhiteSpace(key) && seen.Add(key))
            {
                yield return key;
            }
        }
    }

    public static bool UsesLegacyKeyOnly(IConfiguration config, string cipherText)
    {
        var primary = config[PrimaryKeySetting];
        if (string.IsNullOrWhiteSpace(primary))
        {
            return false;
        }

        try
        {
            new ConvertPassword().Decrypt(cipherText, primary);
            return false;
        }
        catch (CryptographicException)
        {
            return TryDecrypt(config, cipherText, out _);
        }
    }
}