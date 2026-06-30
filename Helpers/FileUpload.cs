namespace Library.Helpers
{
    /// <summary>
    /// Validates uploaded file types and sizes and saves files to a wwwroot subfolder.
    /// </summary>
    public class FileUpload
    {
        private static List<Uploader>? _uploads;
        private const long MaxFileSize = 1024 * 1024 * 18;
        private const int MaxAllowedFiles = 5;

        /// <summary>
        /// Validates upload count, allowed extensions, and per-file size limits.
        /// </summary>
        /// <param name="_uploads">List of pending uploads to validate.</param>
        /// <returns>Success or failure <see cref="ResponseMessage"/> describing validation outcome.</returns>
        private ResponseMessage ValidateFile(List<Uploader>? _uploads)
        {
            if (_uploads.Count == 0)
            {
                return new ResponseMessage()
                {
                    Success = false,
                    ShortDescription = "No Files Found!"
                };
            }
            if (_uploads.Count > MaxAllowedFiles)
            {
                return new ResponseMessage()
                {
                    Success = false,
                    ShortDescription = $"The request exceeded the allowed {MaxAllowedFiles} of files (Err: 4)"
                };
            }

            foreach (var file in _uploads)
            {
                if (!CheckFileType(file.TheFile.Name.Split('.').Last().ToLower()))
                {
                    return new ResponseMessage()
                    {
                        Success = false,
                        ShortDescription = $"File format not supported  (Err: 1) (Err: 4)"
                    };
                }

                if (file.TheFile.Length > MaxFileSize)
                {
                    return new ResponseMessage()
                    {
                        Success = false,
                        ShortDescription = $"{file.TheFile.Name} of {file.TheFile.Length} bytes is larger than the limit of {MaxFileSize} bytes (Err: 2)"
                    };
                }
            }
            return new ResponseMessage()
            {
                Success = true
            };
        }

        /// <summary>
        /// Determines whether a file extension is allowed for upload.
        /// </summary>
        /// <param name="fileName">File name or path used to resolve the extension.</param>
        /// <returns>True for doc, docx, pdf, xls, and xlsx extensions; otherwise false.</returns>
        public bool CheckFileType(string fileName)
        {
            var ext = Path.GetExtension(fileName);
            return ext.ToLower() switch
            {
                ".doc" => true,
                ".docx" => true,
                ".pdf" => true,
                ".xls" => true,
                ".xlsx" => true,
                _ => false
            };
        }

        /// <summary>
        /// Saves uploaded files from memory streams to a folder under wwwroot.
        /// </summary>
        /// <param name="files">Upload descriptors containing file metadata and memory streams.</param>
        /// <param name="folderpath">Relative subfolder under wwwroot where files are stored.</param>
        /// <returns>Success or failure <see cref="ResponseMessage"/> for the save operation.</returns>
        public async Task<ResponseMessage> SaveFiles(List<Uploader>? files, string folderpath)
        {
            if (files == null)
            {
                return new ResponseMessage()
                {
                    Success = false,
                    ShortDescription = "No Files Found"
                };
            }

            try
            {
                foreach (var upload in files!)
                {
                    var saveFolder = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\\", folderpath);

                    if (!Directory.Exists(saveFolder))
                    {
                        Directory.CreateDirectory(saveFolder);
                    }

                    var filePath = Path.Combine(saveFolder, upload.TheFile!.Name);

                    var fi = new FileInfo(filePath);

                    if (new FileInfo(filePath).Exists)
                    {
                        fi.Delete();
                    }

                    await using var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
                    upload.Memory!.Position = 0; // Ensure the memory stream is at the start
                    await upload.Memory.CopyToAsync(fileStream);
                    return new ResponseMessage()
                    {
                        Success = true
                    };
                }
            }
            catch (Exception e)
            {
                return new ResponseMessage()
                {
                    Success = false,
                    ShortDescription = e.Message
                };
            }
            return new ResponseMessage()
            {
                Success = false,
                ShortDescription = "Upload failed!"
            };
        }
    }

    /// <summary>
    /// Holds an uploaded file's metadata and in-memory content for saving to disk.
    /// </summary>
    public class Uploader
    {
        /// <summary>Uploaded file metadata.</summary>
        public FileInfo? TheFile { get; set; }

        /// <summary>In-memory file contents.</summary>
        public MemoryStream? Memory { get; set; }
    }
}
