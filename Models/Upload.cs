namespace Library.Models;

/// <summary>
/// Represents an uploaded file held in memory or on disk.
/// </summary>
public class Uploader
{
    /// <summary>Uploaded file metadata when stored on disk.</summary>
    public FileInfo File { get; set; }

    /// <summary>In-memory copy of the uploaded file contents.</summary>
    public MemoryStream Memory { get; set; }
}
