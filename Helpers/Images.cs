namespace QueenswoodSoftware.Utils.Helpers
{
    /// <summary>
    /// Maps MIME content types to file extensions for uploads and storage.
    /// </summary>
    public class Images
    {
        private readonly Dictionary<string, string> _extensionMap = new()
        {
            { "image/jpeg", ".jpg" },
            { "image/png", ".png" },
            { "image/gif", ".gif" },
            { "image/webp", ".webp" },
            { "image/bmp", ".bmp" },
            { "image/tiff", ".tiff" },
            { "image/svg+xml", ".svg" },
            { "application/pdf", ".pdf" },
            { "application/zip", ".zip" },
            { "application/json", ".json" },
            { "text/plain", ".txt" },
            { "text/html", ".html" },
            { "text/csv", ".csv" },
            { "video/mp4", ".mp4" },
            { "video/mpeg", ".mpeg" },
            { "audio/mpeg", ".mp3" },
            { "audio/wav", ".wav" },
        };

        /// <summary>
        /// Resolves a file extension from a MIME content type string.
        /// </summary>
        /// <param name="contentType">MIME type, optionally including parameters after a semicolon.</param>
        /// <returns>File extension including the leading dot, or empty string when unknown.</returns>
        public string GetExtension(string contentType)
        {
            if (string.IsNullOrWhiteSpace(contentType)) return string.Empty;

            // Strip parameters e.g. "image/jpeg; charset=utf-8"
            var clean = contentType.Split(';')[0].Trim().ToLower();
            Console.WriteLine($"Clean value: '{clean}', Length: {clean.Length}"); // 👈 add this
            if (_extensionMap.TryGetValue(clean, out var ext))
                return ext;

            // Fallback: just use whatever is after the slash
            var parts = clean.Split('/');
            return parts.Length == 2 ? "." + parts[1] : string.Empty;
        }
    }
}