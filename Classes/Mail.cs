using QueenswoodSoftware.Utils.Models;

namespace QueenswoodSoftware.Utils.Classes;

/// <summary>
/// Contract for SMTP email sending.
/// </summary>
public interface IMail
{
    /// <summary>
    /// Sends an email message using the configured SMTP server and credentials.
    /// </summary>
    Task SendMail(MailObject mailObject);
}

/// <summary>
/// Sends SMTP email messages using configuration from the application settings.
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
    /// <returns>A completed task; failures are logged and do not throw.</returns>
    public Task SendMail(MailObject mailObject)
    {
        try
        {
            var host = _config["Mail:Server"] ?? _config["Mail:Host"];
            var smtpClient = new SmtpClient(host, int.Parse(_config["Mail:Port"]!));

            var mail = new MailMessage();

            mail.From = mailObject.From ?? new MailAddress(
                _config["Mail:From"]!,
                _config["Mail:Name"] ?? _config["Mail:PortalName"] ?? _config["Mail:From"]!);

            foreach (var t in mailObject.To!)
            {
                mail.To.Add(t.Address);
            }

            if (mailObject.Cc != null)
            {
                foreach (var t in mailObject.Cc!)
                {
                    mail.CC.Add(t.Address);
                }
            }

            if (mailObject.Bcc != null)
            {
                foreach (var t in mailObject.Bcc!)
                {
                    mail.Bcc.Add(t.Address);
                }
            }

            if (mailObject.Attachments != null)
            {
                foreach (var t in mailObject.Attachments!)
                {
                    mail.Attachments.Add(t);
                }
            }

            try
            {
                mail.Subject = mailObject.Subject;
                mail.Body = mailObject.Body;
                mail.IsBodyHtml = true;

                smtpClient.EnableSsl = true;
                smtpClient.Port = int.Parse(_config["Mail:Port"]!);
                smtpClient.Credentials = new System.Net.NetworkCredential(_config["Mail:UserName"], _config["Mail:Password"]);
                smtpClient.Send(mail);

                smtpClient.Dispose();
            }
            catch (Exception ex)
            {
                Log.Warning(ex.Message, ex.InnerException);
            }
        }
        catch (Exception ex)
        {
            Log.Warning(ex.Message, ex.InnerException);
        }

        return Task.CompletedTask;
    }
}
