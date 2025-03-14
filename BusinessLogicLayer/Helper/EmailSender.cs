using MailKit.Security;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Identity.UI.Services;
using MimeKit;
using MailKit.Net.Smtp; 

public class EmailSender : IEmailSender
{
    private readonly EmailSettings _emailSettings;

    public EmailSender(IOptions<EmailSettings> emailSettings)
    {
        _emailSettings = emailSettings.Value;
    }

    public async Task SendEmailAsync(string email, string subject, string message)
    {
        MimeMessage emailTosend = new MimeMessage();
        emailTosend.From.Add(new MailboxAddress(_emailSettings.FromEmail, _emailSettings.FromEmail));
        emailTosend.To.Add(new MailboxAddress(email, email));
        emailTosend.Subject = subject;
        emailTosend.Body = new TextPart("html"){Text = message};

        using SmtpClient smtp = new SmtpClient();
        try{
            await smtp.ConnectAsync(_emailSettings.SmtpServer, _emailSettings.Port, SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_emailSettings.Username, _emailSettings.Password);
            await smtp.SendAsync(emailTosend);
            await smtp.DisconnectAsync(true);
        }
        catch(Exception ex){
            Console.WriteLine($"Email Sending Fialed : ${ex.Message}");
        }
    }
}