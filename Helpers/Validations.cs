namespace QueenswoodSoftware.Utils.Helpers;

/// <summary>
/// Validation contract for passwords, model fields, numeric strings, email, and allowed upload extensions.
/// </summary>
public interface IValidate
{
    /// <summary>
    /// Validates password complexity requirements and returns an error message when invalid.
    /// </summary>
    /// <param name="password">Password to validate.</param>
    /// <param name="msg">Error message when validation fails; empty when valid.</param>
    /// <returns>True when the password meets all strength rules; otherwise false.</returns>
    public bool PasswordStrength(string password, out string msg);

    /// <summary>
    /// Validates that no string property on a model contains disallowed characters.
    /// </summary>
    /// <param name="model">Object whose string properties are scanned.</param>
    /// <returns>True when all non-null string properties pass; otherwise false.</returns>
    public bool InvalidChars(object model);

    /// <summary>
    /// Validates that required properties on a model are not null except those named in the allow list.
    /// </summary>
    /// <param name="model">Object whose properties are checked for null.</param>
    /// <param name="allowedNull">Property names permitted to be null.</param>
    /// <returns>True when all required properties have values; otherwise false.</returns>
    public bool NullFields(object model, List<string> allowedNull);

    /// <summary>
    /// Validates that each string in the list contains digits only.
    /// </summary>
    /// <param name="fieldTexts">Strings to validate as integers.</param>
    /// <returns>True when every entry matches the integer pattern; otherwise false.</returns>
    public bool IntOnlyFields(List<string> fieldTexts);

    /// <summary>
    /// Validates that each string in the list is a float or integer format.
    /// </summary>
    /// <param name="fieldTexts">Strings to validate as numeric values.</param>
    /// <returns>True when every entry matches float or integer patterns; otherwise false.</returns>
    public bool FloatOnlyFields(List<string> fieldTexts);

    /// <summary>
    /// Validates an email address format.
    /// </summary>
    /// <param name="email">Email address to test.</param>
    /// <returns>True when the email is non-empty and matches the expected pattern; otherwise false.</returns>
    public bool IsValidEmail(string? email);

    /// <summary>
    /// Determines whether a file extension is on the allowed upload list.
    /// </summary>
    /// <param name="ext">File extension without a leading dot.</param>
    /// <returns>True when the extension is permitted for upload.</returns>
    public bool IsValidExtention(string ext);
}

/// <summary>
/// Model, password, email, and upload extension validation helpers.
/// </summary>
public class Validations : IValidate
{
    /// <summary>
    /// Validates an email address format.
    /// </summary>
    /// <param name="email">Email address to test.</param>
    /// <returns>True when the email is non-empty and matches the expected pattern; otherwise false.</returns>
    public bool IsValidEmail(string? email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return false;
        var trimmedEmail = email.Trim();
        return Regex.IsMatch(trimmedEmail,
            @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
            RegexOptions.IgnoreCase);
    }

    /// <summary>Regular expression character class of disallowed input characters.</summary>
    public string ValidateJson = "[`&%^*#|+~?<={}]"; //JsonNode.Parse(File.ReadAllText("../EimLibrary/Validate.json"));

    /// <summary>
    /// Validates a support ticket model for illegal characters and required fields.
    /// </summary>
    /// <param name="model">Support ticket object to validate.</param>
    /// <param name="msg">Error message when validation fails; empty when valid.</param>
    /// <returns>True when the model passes all support ticket checks; otherwise false.</returns>
    public bool SupportTicket(object model, out string msg)
    {
        msg = string.Empty;
        // if illegal characters are submitted
        if (!InvalidChars(model)) msg = "Illegal characters found, please re-evaluate your text.";
        // if values are null
        if (!NullFields(model, new List<string>())) msg = "Please complete all required fields.";
        return string.IsNullOrEmpty(msg);
    }

    /// <summary>
    /// Validates AlphaNumeric Only
    /// </summary>
    /// <param name="msg">String to test for alphanumeric characters only.</param>
    /// <returns>True when the string contains only letters and digits; otherwise false.</returns>
    public bool AlphaNumericOnly(string msg)
    {
        var alphaNumeric = new Regex(@"^[a-zA-Z0-9]*$");
        return alphaNumeric.IsMatch(msg);
    }

    /// <summary>
    /// When a user changes their password, validate that the password meets the security requirements
    /// </summary>
    /// <param name="password">Password to validate.</param>
    /// <param name="msg">Error message when validation fails; empty when valid.</param>
    /// <returns>True when the password meets all strength rules; otherwise false.</returns>
    public bool PasswordStrength(string password, out string msg)
    {
        msg = string.Empty;
        var hasNumber = new Regex(@"[0-9]+");
        var hasUpperChar = new Regex(@"[A-Z]+");
        var hasMiniMaxChars = new Regex(@".{8}");
        var hasLowerChar = new Regex(@"[a-z]+");
        //var hasSymbols = new Regex(@"[!@#$%^&*()_+=\[{\]};:<>|./?,-]");
        // if password is null or has white space, return error msg and false
        if (string.IsNullOrWhiteSpace(password)) msg = "Please enter a new password !";
        // if the password does not contain a lower case letter, return error msg and false
        if (!hasLowerChar.IsMatch(password)) msg = "Password should contain at least one lower case letter";
        // if the password does not contain an upper case letter, return error msg and false
        if (!hasUpperChar.IsMatch(password)) msg = "Password should contain at least one upper case letter";
        // if the password length is less than 8 or more than 12 chars, return error msg and false
        if (!hasMiniMaxChars.IsMatch(password)) msg = "Password should not be less than 8 characters";
        // if the password does not contain a number, return error msg and false
        if (!hasNumber.IsMatch(password)) msg = "Password should contain at least one numeric value";
        return string.IsNullOrEmpty(msg);
        // if all tests pass, return true
    }

    /// <summary>
    /// validates a model to ensure no field contains an invalid char
    /// </summary>
    /// <param name="model">Object whose string properties are scanned.</param>
    /// <returns>True when all non-null string properties pass; otherwise false.</returns>
    public bool InvalidChars(object model)
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
    /// validates a model to ensure no field is null (except if it is in the ListData allowedNull)
    /// </summary>
    /// <param name="model">Object whose properties are checked for null.</param>
    /// <param name="allowedNull">Property names permitted to be null.</param>
    /// <returns>True when all required properties have values; otherwise false.</returns>
    public bool NullFields(object model, List<string> allowedNull)
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
    /// validates a ListData<string> to ensure that each string is in the format of an int</string>
    /// </summary>
    /// <param name="fieldTexts">Strings to validate as integers.</param>
    /// <returns>True when every entry matches the integer pattern; otherwise false.</returns>
    public bool IntOnlyFields(List<string> fieldTexts)
    {
        // test each string in ListData. if any string is not null and not int, return false
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
    /// validates a ListData<string> to ensure that each string is in the format of a float or int</string>
    /// </summary>
    /// <param name="fieldTexts">Strings to validate as numeric values.</param>
    /// <returns>True when every entry matches float or integer patterns; otherwise false.</returns>
    public bool FloatOnlyFields(List<string> fieldTexts)
    {
        // test each string in ListData. if any string is not null, not float and not int, return false
        foreach (var fieldText in fieldTexts)
        {
            if (Regex.IsMatch(fieldText, @"^[0-9]*[.][0-9]*$", RegexOptions.IgnoreCase)) continue;
            if (Regex.IsMatch(fieldText, @"^\d+$", RegexOptions.IgnoreCase)) continue;
            return false;
        }
        return true; // if all strings pass, return true
    }

    /// <summary>
    /// Only this will be allowed to be uploaded
    /// </summary>
    /// <param name="ext">File extension without a leading dot.</param>
    /// <returns>True when the extension is permitted for upload.</returns>
    public bool IsValidExtention(string ext)
    {
        var list = new List<string> {
            "txt", "doc", "docx", "pdf", "xls", "xlsx", "csv", "ppt", "pptx", "jpg", "jpeg", "png", "gif",
            "bmp", "tiff", "tif", "zip", "rar", "7z", "tar", "gz", "html", "htm", "xml", "json"
        };
        return list.Exists(s => s == ext);
    }
}