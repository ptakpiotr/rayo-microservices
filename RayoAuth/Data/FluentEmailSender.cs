using FluentEmail.Core;
using FluentEmail.Smtp;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net.Mail;

namespace RayoAuth.Data
{
    public class FluentEmailSender : IEmailSender
    {
        private readonly IConfiguration _config;

        public FluentEmailSender(IConfiguration config)
        {
            _config = config;
            SmtpSender sender = new(new SmtpClient(_config["Mailing:Provider"])
            {
                EnableSsl = false,
                DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory,
                PickupDirectoryLocation = @"C:\Users\piter\Documents\Mails"
            });

            Email.DefaultSender = sender;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            await Email.From(_config["Mailing:Email"]).To(email).Subject(subject).Body(htmlMessage, true).SendAsync();
        }
    }
}
