using System.Net.Mail;
using System.Net;
using MimeKit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using Transactional_Outbox_Pattern_Use_Api.Settings;

namespace Transactional_Outbox_Pattern_Use_Api.Services
{
    public class EmailService
    {

        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly MailSettings _mailSettings;

        public EmailService(IWebHostEnvironment webHostEnvironment,
            IOptions<MailSettings> mailSettings)
        {
            _webHostEnvironment = webHostEnvironment;
            _mailSettings = mailSettings.Value;
        }
        //public async Task SendEmailAsync(string email, string subject, string body)
        //{
        //    var client = new SmtpClient("smtp.mailtrap.io", 587)
        //    {
        //        Credentials = new NetworkCredential("d1463fc64ea77f", "6cd8dc93ca0263"),
        //        EnableSsl = true
        //    };
        //    await client.SendMailAsync("from@example.com", "to@example.com", subject, body);
        //   // await client.SendAsync("from@example.com", "to@example.com", "Hello world", "testbody");

        //}

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            try
            {
                MailMessage message = new()
                {
                    From = new MailAddress(_mailSettings.Email!, _mailSettings.DisplayName),
                    Body = htmlMessage,
                    Subject = subject,
                    IsBodyHtml = true
                };

                // Use a different email in development if specified
                message.To.Add(_webHostEnvironment.IsDevelopment() ? "abdualeem.hosny@nagwa.com" : email);

                using SmtpClient smtpClient = new(_mailSettings.Host)
                {
                    Port = _mailSettings.Port,
                    Credentials = new NetworkCredential(_mailSettings.Email, _mailSettings.Password),
                    EnableSsl = true
                };

                await smtpClient.SendMailAsync(message);
            }
            catch (SmtpException ex)
            {
                // Log the exception for debugging
                Console.WriteLine($"SMTP Exception: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                // Log any other exceptions
                Console.WriteLine($"General Exception: {ex.Message}");
                throw;
            }
        }

    }
}

