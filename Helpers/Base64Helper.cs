namespace QueenswoodSoftware.Utils.Helpers
{
    /// <summary>
    ///
    /// </summary>
    public class Base64Helper
    {
        public string Base64Encode(string plainText)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }

        public string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
            return Encoding.UTF8.GetString(base64EncodedBytes);
        }

        public static MemoryStream Base64ToMemoryStream(string base64String)
        {
            if (string.IsNullOrWhiteSpace(base64String))
                throw new ArgumentException("Base64Helper string cannot be null or empty.", nameof(base64String));

            // Remove data URI prefix if present (e.g. "data:image/png;base64,")
            var cleanedBase64 = base64String.Contains(",")
                ? base64String.Substring(base64String.IndexOf(",") + 1)
                : base64String;

            var bytes = Convert.FromBase64String(cleanedBase64);
            return new MemoryStream(bytes);
        }
    }
}