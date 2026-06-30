namespace Library.Helpers;

/// <summary>
/// Temporary password selection and Triple-DES encryption helpers for legacy password flows.
/// </summary>
public class ConvertPassword
{
    /// <summary>
    /// Returns a random password from the built-in temporary password list.
    /// </summary>
    /// <returns>A password string from <see cref="PwList"/>.</returns>
    public string GetRandomPassword()
    {
        var listPasswords = PwList;
        var randomNum = new Random();
        var randomPos = randomNum.Next(listPasswords.Count);
        return listPasswords[randomPos];
    }

    /// <summary>
    /// List of temporary passwords when a user forgets their password
    /// </summary>
    private static readonly List<string> PwList = new()
        {
            "D9tzdJVz",
            "BZcd6dSf",
            "6hajvvKP",
            "Csthykh9",
            "9XxWf7HW",
            "tk2WH9Q6",
            "mGZuxP9f",
            "7EER2NXC",
            "QJwgsJNt",
            "KfjgCysN",
            "dURs8TLy",
            "F92nMvnk",
            "2vZdrdFH",
            "e7zzR5vG",
            "n9zRKQQV",
            "5DrjdgDC",
            "j4V7mNcG",
            "q5cbVTZC",
            "76dkjeZW",
            "SPTxzmu3",
            "bETyvfEH",
            "FW3T9kqM",
            "VpDFrJ9w",
            "AL4xjhBJ",
            "9jfCyf3A",
            "x8YSk259",
            "aBywZUap",
            "TLDwxKSz",
            "AbPf6gJD",
            "KjUVrqR7",
            "ZSRJHjkG",
            "aJpVt8eT",
            "ug6ftMKJ",
            "hdXPVtkw",
            "auxVVgTB",
            "syDu5WLw",
            "cTUEdYDx",
            "HkceveZW",
            "CCmntvPG",
            "GaTPFQm4",
            "phrbzetw",
            "7hByPsyT",
            "45E4zV7V",
            "9RALwNx6",
            "HALaaAqk",
            "S9dmMVwa",
            "w9sUprnC",
            "z6Z23fXJ",
            "AkwQjLmx",
            "cvLSQ34L",
            "ATVtxpkX",
            "qe7GHyNj",
            "AxRREn9m",
            "SrXZDGaf",
            "t9eJEpsS",
            "gAe9jXe7",
            "UsfffG4t",
            "cw6MyASM",
            "F7myZsBd",
            "XksCESYh",
            "uBkdzBP9",
            "S8Rvf2ma",
            "WYKnmCzr",
            "3Z2uBn5h",
            "xvBdLvFX",
            "VxFwt8VJ",
            "nBE8LRQe",
            "EmaLGb9m",
            "yT9CPms6",
            "FhHZUgXQ",
            "zAAcWxzt",
            "AazVZ2gb",
            "kWKetY2v",
            "GjJZYJHP",
            "5RH82yCW",
            "pEd5XJqm",
            "5VS9Hb79",
            "cMQMWUSz",
            "k3DVtUjX",
            "UgQtcanT",
            "wnZZ6zsu",
            "XKptP9NB",
            "HjV5wPw4",
            "gfvFSzKu",
            "raHJdCvx",
            "4Pyepe9d",
            "mGzmKhQF",
            "XhdDgRgz",
            "gtKPLxMu",
            "x8Gm9JZV",
            "vFkd2eps",
            "exLSqSyS",
            "cZrDyJMJ",
            "V37Pbssb",
            "pb9UX2m8",
            "CUQx9vTp",
            "gGrEZuSw",
            "cH4SSbRF",
            "7qvcB4Hy",
            "qprwAMg7",
        };

    /// <summary>
    /// Encrypts plain text using Triple-DES with an MD5-derived key from the supplied passphrase.
    /// </summary>
    /// <param name="source">Clear text to encrypt.</param>
    /// <param name="key">Passphrase used to derive the encryption key.</param>
    /// <returns>Base64-encoded ciphertext.</returns>
    public string Encrypt(string source, string key)
    {
        var desCryptoProvider = new TripleDESCryptoServiceProvider();
        var hashMd5Provider = new MD5CryptoServiceProvider();

        var byteHash = hashMd5Provider.ComputeHash(Encoding.UTF8.GetBytes(key));
        desCryptoProvider.Key = byteHash;
        desCryptoProvider.Mode = CipherMode.ECB; //CBC, CFB
        var byteBuff = Encoding.UTF8.GetBytes(source);

        var encoded =
            Convert.ToBase64String(
                desCryptoProvider.CreateEncryptor().TransformFinalBlock(byteBuff, 0, byteBuff.Length));
        return encoded;
    }

    /// <summary>
    /// Decrypts Triple-DES ciphertext using an MD5-derived key from the supplied passphrase.
    /// </summary>
    /// <param name="encodedText">Base64-encoded ciphertext.</param>
    /// <param name="key">Passphrase used to derive the decryption key.</param>
    /// <returns>Decrypted clear text.</returns>
    public string Decrypt(string encodedText, string key)
    {
        var desCryptoProvider = new TripleDESCryptoServiceProvider();
        var hashMd5Provider = new MD5CryptoServiceProvider();

        var byteHash = hashMd5Provider.ComputeHash(Encoding.UTF8.GetBytes(key));
        desCryptoProvider.Key = byteHash;
        desCryptoProvider.Mode = CipherMode.ECB; //CBC, CFB
        var byteBuff = Convert.FromBase64String(encodedText);

        var plaintext =
            Encoding.UTF8.GetString(desCryptoProvider.CreateDecryptor()
                .TransformFinalBlock(byteBuff, 0, byteBuff.Length));
        return plaintext;
    }
}
