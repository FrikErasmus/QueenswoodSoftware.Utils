namespace QueenswoodSoftware.Utils.Models;

/// <summary>
/// Email message definition for SMTP sending.
/// </summary>
public class MailObject
{
    /// <summary>Sender address; falls back to configuration when null.</summary>
    public MailAddress? From { get; set; }

    /// <summary>Primary recipients.</summary>
    public List<MailAddress>? To { get; set; }

    /// <summary>Carbon-copy recipients.</summary>
    public List<MailAddress>? Cc { get; set; }

    /// <summary>Blind carbon-copy recipients.</summary>
    public List<MailAddress>? Bcc { get; set; }

    /// <summary>File attachments included with the message.</summary>
    public List<Attachment>? Attachments { get; set; }

    /// <summary>Email subject line.</summary>
    public string? Subject { get; set; }

    /// <summary>HTML or plain-text message body.</summary>
    public string? Body { get; set; }
}

/// <summary>
/// SMTP server connection settings.
/// </summary>
public class Smtp
{
    /// <summary>SMTP host name or IP address.</summary>
    public string? Server { get; set; }

    /// <summary>SMTP port number.</summary>
    public int Port { get; set; }

    /// <summary>Default from address.</summary>
    public string? From { get; set; }

    /// <summary>Display name shown to recipients.</summary>
    public string? Display { get; set; }

    /// <summary>SMTP authentication username.</summary>
    public string? UserName { get; set; }

    /// <summary>SMTP authentication password.</summary>
    public string? Password { get; set; }
}