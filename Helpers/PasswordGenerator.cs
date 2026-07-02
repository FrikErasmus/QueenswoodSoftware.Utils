namespace QueenswoodSoftware.Utils.Helpers
{
    /// <summary>
    /// Generates deterministic and random alphanumeric passwords from seeded lists.
    /// </summary>
    public static class PasswordGenerator
    {
        // Fixed seed ensures the same passwords are generated every time
        private const int Seed = 12345;

        private const string AlphanumericChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        private const int PasswordLength = 10;
        private const int PasswordCount = 1000;

        /// <summary>
        /// get random password from a deterministic list of 100 passwords.
        /// </summary>
        /// <returns>A randomly selected password from the standard generated list.</returns>
        public static string GetRandomPassword()
        {
            var random = new Random();
            var passwords = GeneratePasswordList();
            return passwords[random.Next(passwords.Count)];
        }

        /// <summary>
        /// Generates a list of 50 deterministic 8-character alphanumeric passwords.
        /// The same passwords will be returned every time this method is called.
        /// </summary>
        /// <returns>A list of 50 unique passwords</returns>
        public static List<string> GeneratePasswordList()
        {
            var passwords = new List<string>();
            var random = new Random(Seed);

            for (var i = 0; i < PasswordCount; i++)
            {
                var password = new StringBuilder(PasswordLength);

                for (var j = 0; j < PasswordLength; j++)
                {
                    var index = random.Next(AlphanumericChars.Length);
                    password.Append(AlphanumericChars[index]);
                }

                passwords.Add(password.ToString());
            }

            return passwords;
        }

        /// <summary>
        /// Generates a list of deterministic passwords with custom parameters.
        /// </summary>
        /// <param name="count">Number of passwords to generate</param>
        /// <param name="length">Length of each password</param>
        /// <param name="seed">Seed for random number generator (optional)</param>
        /// <returns>A list of passwords</returns>
        public static List<string> GeneratePasswordList(int count, int length, int? seed = null)
        {
            var passwords = new List<string>();
            var random = new Random(seed ?? Seed);

            for (var i = 0; i < count; i++)
            {
                var password = new StringBuilder(length);

                for (var j = 0; j < length; j++)
                {
                    var index = random.Next(AlphanumericChars.Length);
                    password.Append(AlphanumericChars[index]);
                }

                passwords.Add(password.ToString());
            }

            return passwords;
        }

        /// <summary>
        /// Gets a specific password by index from the standard list.
        /// </summary>
        /// <param name="index">Index (0-49) of the password to retrieve</param>
        /// <returns>The password at the specified index</returns>
        public static string GetPasswordByIndex(int index)
        {
            if (index is < 0 or >= PasswordCount)
                throw new ArgumentOutOfRangeException(nameof(index), $"Index must be between 0 and {PasswordCount - 1}");

            var passwords = GeneratePasswordList();
            return passwords[index];
        }
    }
}