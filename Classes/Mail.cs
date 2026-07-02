using QueenswoodSoftware.Utils.Models;

namespace QueenswoodSoftware.Utils.Classes;

/// <summary>
/// Sends SMTP email messages using configuration from the application settings.
/// </summary>
public class Mail
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
            var smtpClient = new SmtpClient(_config["Mail:Server"], int.Parse(_config["Mail:Port"]!));

            var mail = new MailMessage();

            mail.From = mailObject.From ?? new MailAddress(_config["Mail:From"]!, _config["Mail:Name"]!);

            //Loop To
            foreach (var t in mailObject.To!)
            {
                mail.To.Add(t.Address);// declare the email to
            }
            //Loop Cc
            if (mailObject.Cc != null)
            {
                foreach (var t in mailObject.Cc!)
                {
                    mail.CC.Add(t.Address);// declare the email to
                }
            }
            //Loop Bcc
            if (mailObject.Bcc != null)
            {
                foreach (var t in mailObject.Bcc!)
                {
                    mail.Bcc.Add(t.Address);// declare the email to
                }
            }
            //Loop Attachments
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

                // enable SSL for encryption across channels
                smtpClient.EnableSsl = true;
                // Port 465 for SSL communication
                smtpClient.Port = int.Parse(_config["Mail:Port"]!);
                // Provide authentication information with Gmail SMTP server to authenticate your sender account
                smtpClient.Credentials = new System.Net.NetworkCredential(_config["Mail:UserName"], _config["Mail:Password"]);
                //Send the mail
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