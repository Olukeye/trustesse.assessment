using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;

namespace Trustesse_Assessment.EmailService
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            var smtpServer = _configuration["Email:SmtpServer"];
            var smtpPort = int.Parse(_configuration["Email:SmtpPort"]);
            var smtpUser = _configuration["Email:SmtpUser"];
            var smtpPassword = _configuration["Email:SmtpPassword"];

            using var smtp = new SmtpClient();
            smtp.Connect(smtpServer, smtpPort, SecureSocketOptions.StartTls);
            smtp.Authenticate(smtpUser, smtpPassword);

            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(smtpUser));
            email.To.Add(MailboxAddress.Parse(to));
            email.Subject = subject;
            email.Body = new TextPart(TextFormat.Html) { Text = body };

            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }
    }
    //public class EmailSender : IEmailSender
    //{
    //    private readonly EmailConfiguration _emailConfig;

    //    public EmailSender(EmailConfiguration emailConfig)
    //    {
    //        _emailConfig = emailConfig;
    //    }

    //    public async Task SendEmailAsync(Message message)
    //    {
    //        var mailMessage = CreateEmailMessage(message);
    //        await SendAsync(mailMessage);
    //    }

    //    private MimeMessage CreateEmailMessage(Message message)
    //    {
    //        var emailMessage = new MimeMessage();
    //        emailMessage.From.Add(new MailboxAddress(_emailConfig.UserName, _emailConfig.From));
    //        emailMessage.To.AddRange(message.To);
    //        emailMessage.Subject = message.Subject;
    //        var bodyBuilder = new BodyBuilder { 
    //            HtmlBody = string.Format("<h2 style='color:red;'>{0}</h2>", message.Content) 
    //        };

    //        if (message.Attachments != null && message.Attachments.Any())
    //        {
    //            byte[] fileBytes;
    //            foreach (var attachment in message.Attachments)
    //            {
    //                using (var ms = new MemoryStream())
    //                {
    //                    attachment.CopyTo(ms);
    //                    fileBytes = ms.ToArray();
    //                }
    //                bodyBuilder.Attachments.Add(attachment.FileName, fileBytes, ContentType.Parse(attachment.ContentType));
    //            }
    //        }
    //        emailMessage.Body = bodyBuilder.ToMessageBody();
    //        return emailMessage;
    //    }


    //    private async Task SendAsync(MimeMessage mailMessage)
    //    {
    //        var client = new SmtpClient();

    //        {
    //            try
    //            {
    //                await client.ConnectAsync(_emailConfig.SmtpServer, _emailConfig.Port, true);
    //                client.AuthenticationMechanisms.Remove("XOAUTH2");
    //                await client.AuthenticateAsync(_emailConfig.UserName, _emailConfig.Password);
    //                await client.SendAsync(mailMessage);
    //            }
    //            catch
    //            {
    //                //log an error message or throw an exception or both.
    //                throw;
    //            }
    //            finally
    //            {
    //                await client.DisconnectAsync(true);
    //                client.Dispose();
    //            }
    //        }
    //    }
    //}

}
