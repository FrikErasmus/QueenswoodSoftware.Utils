namespace QueenswoodSoftware.Utils.Helpers
{
    /// <summary>
    /// File system checks, MD5 checksum helpers, and chunked file equality comparison.
    /// </summary>
    public class Files
    {
        /// <summary>
        /// Determines whether a directory exists at the given path.
        /// </summary>
        /// <param name="folderToCheck">Directory path to test.</param>
        /// <returns>True when the directory exists; otherwise false.</returns>
        public bool CheckFolderExists(string folderToCheck)
        {
            var message = Directory.Exists(folderToCheck);
            message = message;
            return message;
        }

        /// <summary>
        /// Computes the MD5 hash of a file before a copy operation.
        /// </summary>
        /// <param name="fileBefore">Path to the source file.</param>
        /// <returns>Uppercase hexadecimal MD5 digest, or concatenated null hash when path is null.</returns>
        public string CheckFileCrcBeforeCopy(string fileBefore)
        {
            byte[] firstHash = null;
            if (fileBefore == null) return string.Concat(firstHash!.Select(x => x.ToString("X2"))); //check this
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
        /// Computes the MD5 hash of a file after a copy operation.
        /// </summary>
        /// <param name="fileAfter">Path to the destination file.</param>
        /// <returns>Uppercase hexadecimal MD5 digest, or concatenated null hash when path is null.</returns>
        public string CheckFileCrcAfterCopy(string fileAfter)
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
        /// Compares two files in fixed-size chunks by reading long-sized segments from each stream.
        /// </summary>
        /// <param name="fileSentCrc">Path to the first file.</param>
        /// <param name="fileRecievedCrc">Path to the second file.</param>
        /// <returns>True when file lengths match and all compared chunks are equal; otherwise false.</returns>
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
    }
}