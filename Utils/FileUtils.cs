using System.IO.Compression;

namespace Library.Utils
{
    /// <summary>
    /// Provides static helpers for validating, uploading, copying, and reading files and directories.
    /// </summary>
    public static class FileUtils
    {
        private static List<Uploader>? _uploads;
        private const long MaxFileSize = 1024 * 1024 * 18;
        private const int MaxAllowedFiles = 5;

        /// <summary>
        /// Validates uploaded files for count, supported type, and maximum size.
        /// </summary>
        /// <param name="_uploads">The list of uploaded files to validate.</param>
        /// <returns>A <see cref="ResponseMessage"/> indicating whether validation passed.</returns>
        public static ResponseMessage ValidateFiles(List<Uploader>? _uploads)
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
        /// Determines whether a file extension is supported for upload.
        /// </summary>
        /// <param name="fileName">The file name or extension to check.</param>
        /// <returns>True when the extension is allowed; otherwise false.</returns>
        public static bool CheckFileType(string fileName)
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
        /// Saves uploaded files from memory streams to the specified web-root folder.
        /// </summary>
        /// <param name="files">The uploaded files to save.</param>
        /// <param name="folderpath">The relative folder path under wwwroot.</param>
        /// <returns>A <see cref="ResponseMessage"/> indicating upload success or failure.</returns>
        public static async Task<ResponseMessage> UploadFiles(List<Uploader>? files, string folderpath)
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

        /// <summary>
        /// Appends text content to an existing file.
        /// </summary>
        /// <param name="filePath">The path of the file to append to.</param>
        /// <param name="content">The text content to append.</param>
        public static void AppendTextToFile(string filePath, string content)
        {
            File.AppendAllText(filePath, content);
        }

        /// <summary>
        /// Calculates the SHA-256 hash of a file as a lowercase hexadecimal string.
        /// </summary>
        /// <param name="filePath">The path of the file to hash.</param>
        /// <returns>The file hash, or null when the file does not exist.</returns>
        public static string CalculateFileHash(string filePath)
        {
            if (!File.Exists(filePath)) return null;
            using (var stream = File.OpenRead(filePath))
            {
                using (var sha256 = SHA256.Create())
                {
                    var hash = sha256.ComputeHash(stream);
                    return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                }
            }
        }

        /// <summary>
        /// Calculates the MD5 checksum of a file after copy operations.
        /// </summary>
        /// <param name="fileAfter">The path of the copied file.</param>
        /// <returns>The MD5 checksum as an uppercase hexadecimal string.</returns>
        public static string CheckFileCrcAfterCopy(string fileAfter)
        {
            byte[] secondHash = null;
            if (fileAfter == null) return string.Concat(secondHash.Select(x => x.ToString("X2"))); //check this
            var lastFileInfo = new FileInfo(fileAfter);
            using (var lastFileStream = lastFileInfo.OpenRead())
            {
                secondHash = MD5.Create().ComputeHash(lastFileStream);
            }
            return string.Concat(secondHash.Select(x => x.ToString("X2"))); //check this
        }

        /// <summary>
        /// Calculates the MD5 checksum of a file before copy operations.
        /// </summary>
        /// <param name="fileBefore">The path of the source file.</param>
        /// <returns>The MD5 checksum as an uppercase hexadecimal string.</returns>
        public static string CheckFileCrcBeforeCopy(string fileBefore)
        {
            byte[] firstHash = null;
            if (fileBefore == null) return string.Concat(firstHash.Select(x => x.ToString("X2"))); //check this
            var firstFileInfo = new FileInfo(fileBefore);
            using (var firstFileStream = firstFileInfo.OpenRead())
            {
                firstHash = MD5.Create().ComputeHash(firstFileStream);
            }
            firstFileInfo = null;
            GC.WaitForPendingFinalizers();
            GC.Collect();
            return string.Concat(firstHash.Select(x => x.ToString("X2"))); //check this
        }

        /// <summary>
        /// Determines whether a directory exists at the specified path.
        /// </summary>
        /// <param name="folderToCheck">The directory path to check.</param>
        /// <returns>True when the directory exists; otherwise false.</returns>
        public static bool CheckFolderExists(string folderToCheck)
        {
            var message = Directory.Exists(folderToCheck);
            message = message;
            return message;
        }

        /// <summary>
        /// Recursively copies a directory and its contents to a destination path.
        /// </summary>
        /// <param name="sourceDirectoryPath">The source directory path.</param>
        /// <param name="destinationDirectoryPath">The destination directory path.</param>
        public static void CopyDirectory(string sourceDirectoryPath, string destinationDirectoryPath)
        {
            if (!Directory.Exists(sourceDirectoryPath))
            {
                throw new DirectoryNotFoundException($"Source directory does not exist or could not be found: {sourceDirectoryPath}");
            }
            Directory.CreateDirectory(destinationDirectoryPath);
            foreach (var file in Directory.GetFiles(sourceDirectoryPath))
            {
                var fileName = Path.GetFileName(file);
                var destFile = Path.Combine(destinationDirectoryPath, fileName);
                File.Copy(file, destFile, true);
            }
            foreach (var subDirectory in Directory.GetDirectories(sourceDirectoryPath))
            {
                var directoryName = new DirectoryInfo(subDirectory).Name;
                var destDirectory = Path.Combine(destinationDirectoryPath, directoryName);
                CopyDirectory(subDirectory, destDirectory);
            }
        }

        /// <summary>
        /// Copies a file to a destination path, overwriting if it already exists.
        /// </summary>
        /// <param name="sourceFilePath">The source file path.</param>
        /// <param name="destinationFilePath">The destination file path.</param>
        public static void CopyFile(string sourceFilePath, string destinationFilePath)
        {
            File.Copy(sourceFilePath, destinationFilePath, true);
        }

        /// <summary>
        /// Creates a directory when it does not already exist.
        /// </summary>
        /// <param name="directoryPath">The directory path to create.</param>
        public static void CreateDirectoryIfNotExists(string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
        }

        /// <summary>
        /// Deletes a directory and all of its contents when it exists.
        /// </summary>
        /// <param name="directoryPath">The directory path to delete.</param>
        public static void DeleteDirectory(string directoryPath)
        {
            if (Directory.Exists(directoryPath))
            {
                Directory.Delete(directoryPath, true);
            }
        }

        /// <summary>
        /// Deletes a file when it exists.
        /// </summary>
        /// <param name="filePath">The file path to delete.</param>
        public static void DeleteFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        /// <summary>
        /// Compares two files in fixed-size chunks to determine whether their contents are equal.
        /// </summary>
        /// <param name="fileSentCrc">The path of the first file.</param>
        /// <param name="fileRecievedCrc">The path of the second file.</param>
        /// <returns>True when both files have the same length and content; otherwise false.</returns>
        public static bool FilesAreEqualHash(string fileSentCrc, string fileRecievedCrc)
        {
            const int BYTES_TO_READ = sizeof(long);

            var file1 = new FileInfo(fileSentCrc);
            var file2 = new FileInfo(fileRecievedCrc);

            if (file1.Length != file2.Length)
                return false;
            var iterations = (int)Math.Ceiling((double)file1.Length / BYTES_TO_READ);

            using var fs1 = file1.OpenRead();
            using var fs2 = file2.OpenRead();
            var one = new byte[BYTES_TO_READ];
            var two = new byte[BYTES_TO_READ];

            for (var i = 0; i < iterations; i++)
            {
                fs1.ReadExactly(one, 0, BYTES_TO_READ);
                fs2.ReadExactly(two, 0, BYTES_TO_READ);

                if (BitConverter.ToInt64(one, 0) != BitConverter.ToInt64(two, 0))
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Gets the size of a file in bytes.
        /// </summary>
        /// <param name="filePath">The path of the file.</param>
        /// <returns>The file size in bytes, or -1 when the file does not exist.</returns>
        public static long GetFileSize(string filePath)
        {
            if (!File.Exists(filePath)) return -1;
            return new FileInfo(filePath).Length;
        }

        /// <summary>
        /// Recursively moves a directory and its contents to a destination path.
        /// </summary>
        /// <param name="sourceDirectoryPath">The source directory path.</param>
        /// <param name="destinationDirectoryPath">The destination directory path.</param>
        public static void MoveDirectory(string sourceDirectoryPath, string destinationDirectoryPath)
        {
            if (!Directory.Exists(sourceDirectoryPath))
            {
                throw new DirectoryNotFoundException($"Source directory does not exist or could not be found: {sourceDirectoryPath}");
            }
            Directory.CreateDirectory(destinationDirectoryPath);
            foreach (var file in Directory.GetFiles(sourceDirectoryPath))
            {
                var fileName = Path.GetFileName(file);
                var destFile = Path.Combine(destinationDirectoryPath, fileName);
                File.Move(file, destFile);
            }
            foreach (var subDirectory in Directory.GetDirectories(sourceDirectoryPath))
            {
                var directoryName = new DirectoryInfo(subDirectory).Name;
                var destDirectory = Path.Combine(destinationDirectoryPath, directoryName);
                MoveDirectory(subDirectory, destDirectory);
            }
            Directory.Delete(sourceDirectoryPath);
        }

        /// <summary>
        /// Moves a file to a new path.
        /// </summary>
        /// <param name="sourceFilePath">The source file path.</param>
        /// <param name="destinationFilePath">The destination file path.</param>
        public static void MoveFile(string sourceFilePath, string destinationFilePath)
        {
            File.Move(sourceFilePath, destinationFilePath);
        }

        /// <summary>
        /// Reads all bytes from a file.
        /// </summary>
        /// <param name="filePath">The path of the file to read.</param>
        /// <returns>The file contents as a byte array, or null when the file does not exist.</returns>
        public static byte[] ReadBytesFromFile(string filePath)
        {
            if (!File.Exists(filePath)) return null;
            return File.ReadAllBytes(filePath);
        }

        /// <summary>
        /// Reads all text from a file.
        /// </summary>
        /// <param name="filePath">The path of the file to read.</param>
        /// <returns>The file contents as a string, or null when the file does not exist.</returns>
        public static string ReadFileToString(string filePath)
        {
            if (!File.Exists(filePath)) return null;
            return File.ReadAllText(filePath);
        }

        /// <summary>
        /// Extracts a zip archive to the specified destination directory.
        /// </summary>
        /// <param name="sourceZipFilePath">The path of the zip file.</param>
        /// <param name="destinationDirectoryPath">The destination directory path.</param>
        public static void UnzipFile(string sourceZipFilePath, string destinationDirectoryPath)
        {
            ZipFile.ExtractToDirectory(sourceZipFilePath, destinationDirectoryPath);
        }

        /// <summary>
        /// Writes a byte array to a file, replacing any existing content.
        /// </summary>
        /// <param name="filePath">The path of the file to write.</param>
        /// <param name="data">The bytes to write.</param>
        public static void WriteBytesToFile(string filePath, byte[] data)
        {
            File.WriteAllBytes(filePath, data);
        }

        /// <summary>
        /// Writes text to a file, replacing any existing content.
        /// </summary>
        /// <param name="filePath">The path of the file to write.</param>
        /// <param name="content">The text content to write.</param>
        public static void WriteStringToFile(string filePath, string content)
        {
            File.WriteAllText(filePath, content);
        }

        /// <summary>
        /// Writes text to a file, replacing any existing content.
        /// </summary>
        /// <param name="filePath">The path of the file to write.</param>
        /// <param name="content">The text content to write.</param>
        public static void WriteTextToFile(string filePath, string content)
        {
            File.WriteAllText(filePath, content);
        }

        /// <summary>
        /// Writes text to a file, optionally appending to existing content.
        /// </summary>
        /// <param name="filePath">The path of the file to write.</param>
        /// <param name="content">The text content to write.</param>
        /// <param name="append">When true, appends to the file; otherwise replaces existing content.</param>
        public static void WriteTextToFile(string filePath, string content, bool append)
        {
            if (append)
            {
                File.AppendAllText(filePath, content);
            }
            else
            {
                File.WriteAllText(filePath, content);
            }
        }

        /// <summary>
        /// Creates a zip archive from a directory.
        /// </summary>
        /// <param name="sourceDirectoryPath">The source directory path.</param>
        /// <param name="destinationZipFilePath">The destination zip file path.</param>
        public static void ZipDirectory(string sourceDirectoryPath, string destinationZipFilePath)
        {
            ZipFile.CreateFromDirectory(sourceDirectoryPath, destinationZipFilePath);
        }
    }

    /// <summary>
    /// Represents an uploaded file held in memory before it is saved to disk.
    /// </summary>
    public class Uploader
    {
        /// <summary>Uploaded file metadata.</summary>
        public FileInfo? TheFile { get; set; }

        /// <summary>In-memory file contents.</summary>
        public MemoryStream? Memory { get; set; }
    }
}
