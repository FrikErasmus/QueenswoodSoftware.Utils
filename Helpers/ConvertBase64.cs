namespace QueenswoodSoftware.Utils.Helpers
{
    /// <summary>
    /// Base64 encoding helpers and image MIME type detection from file magic bytes.
    /// </summary>
    public class ConvertBase64
    {
        /// <summary>
        /// Encodes a UTF-8 string as Base64.
        /// </summary>
        /// <param name="plainText">The text to encode.</param>
        /// <returns>Base64-encoded string.</returns>
        public string Base64Encode(string plainText)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }

        /// <summary>
        /// Decodes a Base64 string to UTF-8 text.
        /// </summary>
        /// <param name="base64EncodedData">The Base64 string to decode.</param>
        /// <returns>Decoded UTF-8 text.</returns>
        public string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
            return Encoding.UTF8.GetString(base64EncodedBytes);
        }

        /// <summary>
        /// Detects a common image MIME type from the leading bytes of a buffer.
        /// </summary>
        /// <param name="bytes">File or image byte content.</param>
        /// <returns>MIME type string (e.g. image/png), or null when unrecognized or buffer too short.</returns>
        public string GetImageMimeTypeFromBytes(byte[] bytes)
        {
            if (bytes.Length < 8) return null; // unknown

            if (bytes.Take(8).SequenceEqual(new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A }))
                return "image/png";

            if (bytes[0] == 0xFF && bytes[1] == 0xD8)
                return "image/jpeg";

            if (bytes[0] == 0x47 && bytes[1] == 0x49 && bytes[2] == 0x46)
                return "image/gif";

            if (bytes[0] == 0x42 && bytes[1] == 0x4D)
                return "image/bmp";

            return null; // unknown
        }
    }
}