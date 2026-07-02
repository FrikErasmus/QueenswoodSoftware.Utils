namespace QueenswoodSoftware.Utils.Utils
{
    /// <summary>
    /// Provides static helpers for validating user input, models, and common data formats.
    /// </summary>
    public static class ValidationUtils
    {
        /// <summary>Regular expression character class of disallowed input characters.</summary>
        public static string ValidateJson = "[`&%^*#|+~?<={}]"; //JsonNode.Parse(File.ReadAllText("../EimLibrary/Validate.json"));

        /// <summary>
        /// Validates a support ticket model for illegal characters and required fields.
        /// </summary>
        /// <param name="model">The support ticket model to validate.</param>
        /// <param name="msg">When validation fails, contains the error message; otherwise empty.</param>
        /// <returns>True when the model passes all checks; otherwise false.</returns>
        public static bool SupportTicket(object model, out string msg)
        {
            msg = string.Empty;

            // if illegal characters are submitted
            if (!InvalidChars(model)) msg = "Illegal characters found, please re-evaluate your text.";

            // if values are null
            if (!NullFields(model, new List<string>())) msg = "Please complete all required fields.";

            return string.IsNullOrEmpty(msg);
        }

        /// <summary>
        /// When a user changes their password, validate that the password meets the security requirements
        /// </summary>
        /// <param name="password"></param>
        /// <param name="msg"></param>
        /// <returns>bool & error msg</returns>
        public static bool PasswordStrength(string password, out string msg)
        {
            msg = string.Empty;
            var hasNumber = new Regex(@"[0-9]+");
            var hasUpperChar = new Regex(@"[A-Z]+");
            var hasMiniMaxChars = new Regex(@".{8,12}");
            var hasLowerChar = new Regex(@"[a-z]+");
            //var hasSymbols = new Regex(@"[!@#$%^&*()_+=\[{\]};:<>|./?,-]");

            // if password is null or has white space, return error msg and false
            if (string.IsNullOrWhiteSpace(password)) msg = "Please enter a new password !";

            // if the password does not contain a lower case letter, return error msg and false
            if (!hasLowerChar.IsMatch(password)) msg = "Password should contain at least one lower case letter";

            // if the password does not contain an upper case letter, return error msg and false
            if (!hasUpperChar.IsMatch(password)) msg = "Password should contain at least one upper case letter";

            // if the password length is less than 8 or more than 12 chars, return error msg and false
            if (!hasMiniMaxChars.IsMatch(password)) msg = "Password should not be less than 8 or greater than 12 characters";

            // if the password does not contain a number, return error msg and false
            if (!hasNumber.IsMatch(password)) msg = "Password should contain at least one numeric value";

            return string.IsNullOrEmpty(msg);
            // if all tests pass, return true
        }

        /// <summary>
        /// validates a model to ensure no field contains an invalid char
        /// </summary>
        /// <param name="model"></param>
        /// <returns>bool</returns>
        public static bool InvalidChars(object model)
        {
            // test each field in object. if any field in the model contains an invalid char, return false
            foreach (var prop in model.GetType().GetProperties())
            {
                if (prop.GetValue(model) == null) continue;
                if (Regex.IsMatch(prop.GetValue(model, null)!.ToString()!, ValidateJson, RegexOptions.IgnoreCase))
                {
                    return false;
                }
            }

            return true; // if all fields pass, return true

            // _configuration["IllegalChars"]
        }

        /// <summary>
        /// validates a model to ensure no field is null (except if it is in the List allowedNull)
        /// </summary>
        /// <param name="model"></param>
        /// <param name="allowedNull"></param>
        /// <returns>bool</returns>
        public static bool NullFields(object model, List<string> allowedNull)
        {
            // test each field in object. if any field is null and is not in the exception list, return false
            foreach (var prop in model.GetType().GetProperties())
            {
                if (prop.GetValue(model, null) != null) continue;
                if (allowedNull.FirstOrDefault(stringToCheck => stringToCheck.Contains(prop.Name)) == null)
                {
                    return false;
                }
            }

            return true; // if all fields pass, return true
        }

        /// <summary>
        /// validates a List<string> to ensure that each string is in the format of an int</string>
        /// </summary>
        /// <param name="fieldTexts"></param>
        /// <returns>bool</returns>
        public static bool IntOnlyFields(List<string> fieldTexts)
        {
            // test each string in List. if any string is not null and not int, return false
            foreach (var fieldText in fieldTexts)
            {
                if (!Regex.IsMatch(fieldText, @"^\d+$", RegexOptions.IgnoreCase))
                {
                    return false;
                }
            }

            return true; // if all strings pass, return true
        }

        /// <summary>
        /// validates a List<string> to ensure that each string is in the format of a float or int</string>
        /// </summary>
        /// <param name="fieldTexts"></param>
        /// <returns>bool</returns>
        public static bool FloatOnlyFields(List<string> fieldTexts)
        {
            // test each string in List. if any string is not null, not float and not int, return false
            foreach (var fieldText in fieldTexts)
            {
                if (Regex.IsMatch(fieldText, @"^[0-9]*[.][0-9]*$", RegexOptions.IgnoreCase)) continue;
                if (Regex.IsMatch(fieldText, @"^\d+$", RegexOptions.IgnoreCase)) continue;
                return false;
            }

            return true; // if all strings pass, return true
        }

        /// <summary>
        /// validate if value is num
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsNumeric(string value)
        {
            return !string.IsNullOrEmpty(value) && value.All(c => char.IsDigit(c));
        }

        /// <summary>
        /// Validate text only
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool ValidateTextOnly(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return true; // Or false, depending on your requirement for empty strings
            }

            // Regular expression to match only letters, spaces, and optionally hyphens/apostrophes.
            var textOnlyRegex = @"^[a-zA-Z\s'-]+$";

            return Regex.IsMatch(value, textOnlyRegex);
        }

        /// <summary>
        /// Checks if a string is a valid email address.
        /// </summary>
        /// <param name="email">The email address to validate.</param>
        /// <returns>True if the email address is valid, otherwise false.</returns>
        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrEmpty(email)) return false;

            try
            {
                var addr = new MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Checks if a string is a valid phone number.
        /// </summary>
        /// <param name="phoneNumber">The phone number to validate.</param>
        /// <returns>True if the phone number is valid, otherwise false.</returns>
        public static bool IsValidPhoneNumber(string phoneNumber)
        {
            if (string.IsNullOrEmpty(phoneNumber)) return false;

            // You can customize the regex to match your specific phone number format.
            // This is a basic example and might need adjustments.
            var pattern = @"^(\d{1,3})?[\s.-]?\(?\d{3}\)?[\s.-]?\d{3}[\s.-]?\d{4}$";
            return Regex.IsMatch(phoneNumber, pattern);
        }

        /// <summary>
        /// Checks if a string is a valid IP address.
        /// </summary>
        /// <param name="ipAddress">The IP address to validate.</param>
        /// <returns>True if the IP address is valid, otherwise false.</returns>
        public static bool IsValidIpAddress(string ipAddress)
        {
            return !string.IsNullOrEmpty(ipAddress) && IPAddress.TryParse(ipAddress, out _);
        }

        /// <summary>
        /// Checks if a string is a strong password.
        /// </summary>
        /// <param name="password">The password to validate.</param>
        /// <returns>True if the password is strong, otherwise false.</returns>
        public static bool IsStrongPassword(string password)
        {
            if (string.IsNullOrEmpty(password)) return false;

            // Customize the regex to enforce your password policy.
            // This is a basic example and might need adjustments.
            var pattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$";
            return Regex.IsMatch(password, pattern);
        }

        /// <summary>
        /// Checks if a string is a valid URL.
        /// </summary>
        /// <param name="url">The URL to validate.</param>
        /// <returns>True if the URL is valid, otherwise false.</returns>
        public static bool IsValidUrl(string url)
        {
            if (string.IsNullOrEmpty(url)) return false;

            return Uri.TryCreate(url, UriKind.Absolute, out var uriResult) && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        }

        /// <summary>
        /// Checks if an integer is within a specified range.
        /// </summary>
        /// <param name="value">The integer value to check.</param>
        /// <param name="min">The minimum allowed value.</param>
        /// <param name="max">The maximum allowed value.</param>
        /// <returns>True if the value is within the range, otherwise false.</returns>
        public static bool IsInRange(int value, int min, int max)
        {
            return value >= min && value <= max;
        }

        /// <summary>
        /// Checks if a DateTime is in the past.
        /// </summary>
        /// <param name="dateTime">The DateTime to check.</param>
        /// <returns>True if the DateTime is in the past, otherwise false.</returns>
        public static bool IsPastDate(DateTime dateTime)
        {
            return dateTime < DateTime.Now;
        }
    }
}