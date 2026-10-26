using QueenswoodSoftware.Utils.Models;

namespace QueenswoodSoftware.Utils.Classes;

/// <summary>
/// Contract for SMTP email sending.
/// </summary>
public interface IMail
{
    /// <summary>
    /// Sends an email message using the configured SMTP server and credentials.
    /// Failures are logged and do not throw.
    /// </summary>
    Task SendMail(MailObject mailObject);

    /// <summary>
    /// Sends an email message and reports whether the SMTP server accepted it.
    /// Failures are logged and do not throw.
    /// </summary>
    /// <returns><c>true</c> when the message was handed to the SMTP server; otherwise <c>false</c>.</returns>
    async Task<bool> TrySendMail(MailObject mailObject)
    {
        await SendMail(mailObject);
        return true;
    }

    /// <summary>
    /// Sends an email and returns a <see cref="ResponseMessage"/> describing whether the SMTP server accepted it.
    /// </summary>
    /// <param name="mailObject">The mail details including recipients, subject, body, and attachments.</param>
    /// <param name="cancellationToken">Cancels the SMTP send.</param>
    Task<ResponseMessage> TrySendMailAsync(MailObject mailObject, CancellationToken cancellationToken = default);
}

/// <summary>
/// Sends SMTP email messages using configuration from the application settings.
/// Reads <c>Mail:Server</c> (or <c>Mail:Host</c>), <c>Mail:Port</c>, <c>Mail:From</c>,
/// <c>Mail:UserName</c>, <c>Mail:Password</c> and optionally <c>Mail:Name</c> / <c>Mail:PortalName</c>
/// and <c>Mail:EnableSsl</c> (default true).
/// </summary>
public class Mail : IMail
{
    private readonly IConfiguration _config;

    /// <summary>
    /// Initializes a new instance of the <see cref="Mail"/> class.
    /// </summary>
    /// <param name="configuration">Application configuration containing mail server settings.</param>
    public Mail(IConfiguration configuration)
    {
        _config = configuration;
    }

    /// <summary>
    /// Sends an email message using the configured SMTP server and credentials.
    /// </summary>
    /// <param name="mailObject">The mail details including recipients, subject, body, and attachments.</param>
    /// <returns>A task that completes when sending finishes; failures are logged and do not throw.</returns>
    public async Task SendMail(MailObject mailObject)
    {
        await TrySendMailAsync(mailObject);
    }

    /// <summary>
    /// Sends an email message and reports whether the SMTP server accepted it.
    /// </summary>
    /// <param name="mailObject">The mail details including recipients, subject, body, and attachments.</param>
    /// <returns><c>true</c> when the message was handed to the SMTP server; otherwise <c>false</c>.</returns>
    public async Task<bool> TrySendMail(MailObject mailObject)
    {
        var result = await TrySendMailAsync(mailObject);
        return result.Success;
    }

    /// <summary>
    /// Sends an email message and reports whether the SMTP server accepted it.
    /// </summary>
    /// <param name="mailObject">The mail details including recipients, subject, body, and attachments.</param>
    /// <param name="cancellationToken">Cancels the SMTP send.</param>
    /// <returns>A successful response when the message was accepted; otherwise a failure with a user-facing message.</returns>
    public async Task<ResponseMessage> TrySendMailAsync(MailObject mailObject, CancellationToken cancellationToken = default)
    {
        var recipients = string.Join(", ", (mailObject?.To ?? new List<MailAddress>()).Select(a => a.Address));

        if (mailObject?.To == null || mailObject.To.Count == 0)
        {
            Log.Error("Mail not sent: no recipients. Subject={Subject}", mailObject?.Subject);
            return new ResponseMessage { Success = false, ShortDescription = "No email recipient is configured." };
        }

        var host = _config["Mail:Server"] ?? _config["Mail:Host"];
        var portText = _config["Mail:Port"];
        var userName = _config["Mail:UserName"];
        var password = _config["Mail:Password"];
        var fromAddress = _config["Mail:From"];
        var displayName = _config["Mail:Name"] ?? _config["Mail:PortalName"] ?? fromAddress;
        var enableSsl = !bool.TryParse(_config["Mail:EnableSsl"], out var ssl) || ssl;

        if (string.IsNullOrWhiteSpace(host)
            || !int.TryParse(portText, out var port)
            || (mailObject.From == null && string.IsNullOrWhiteSpace(fromAddress))
            || string.IsNullOrWhiteSpace(userName)
            || string.IsNullOrWhiteSpace(password))
        {
            Log.Error("Mail not sent: Mail configuration incomplete. HostSet={HostSet} PortValid={PortValid} FromSet={FromSet} UserSet={UserSet} PasswordSet={PasswordSet} To={Recipients} Subject={Subject}",
                !string.IsNullOrWhiteSpace(host),
                int.TryParse(portText, out _),
                mailObject.From != null || !string.IsNullOrWhiteSpace(fromAddress),
                !string.IsNullOrWhiteSpace(userName),
                !string.IsNullOrWhiteSpace(password),
                recipients,
                mailObject.Subject);
            return new ResponseMessage { Success = false, ShortDescription = "Email is not configured. Please contact support directly." };
        }

        try
        {
            using var smtpClient = new SmtpClient(host, port)
            {
                DeliveryMethod = SmtpDeliveryMethod.Network,
                EnableSsl = enableSsl,
                // Must be false before Credentials is set, otherwise the Windows identity is used.
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(userName, password)
            };

            using var mail = new MailMessage
            {
                From = mailObject.From ?? new MailAddress(fromAddress!, displayName),
                Subject = mailObject.Subject ?? string.Empty,
                Body = mailObject.Body ?? string.Empty,
                IsBodyHtml = true
            };

            foreach (var to in mailObject.To) mail.To.Add(to);
            if (mailObject.Cc != null) foreach (var cc in mailObject.Cc) mail.CC.Add(cc);
            if (mailObject.Bcc != null) foreach (var bcc in mailObject.Bcc) mail.Bcc.Add(bcc);
            if (mailObject.Attachments != null) foreach (var attachment in mailObject.Attachments) mail.Attachments.Add(attachment);

            await smtpClient.SendMailAsync(mail, cancellationToken);

            Log.Information("Mail sent. To={Recipients} Subject={Subject}", recipients, mail.Subject);
            return new ResponseMessage { Success = true };
        }
        catch (OperationCanceledException)
        {
            return new ResponseMessage { Success = false, ShortDescription = "Sending the email was cancelled." };
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Mail send failed. Host={Host} Port={Port} User={User} To={Recipients} Subject={Subject}",
                host, port, userName, recipients, mailObject.Subject);
            return new ResponseMessage { Success = false, ShortDescription = "Unable to send the email at this time. Please try again later." };
        }
    }
}
