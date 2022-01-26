using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Takas.WebApi.Services.Interfaces
{
    public interface IMailService
    {
        Task SendEmailAsync(string toEmail, string subject, string content);
    }
    public class SendGridMailService : IMailService
    {
        private IConfiguration _configuration;
        public SendGridMailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task SendEmailAsync(string toEmail, string subject, string content)
        {
            try
            {
                SmtpClient smtpClient = new SmtpClient(_configuration["SMTPAdress"], 587);

                smtpClient.Credentials = new System.Net.NetworkCredential(_configuration["FromEmailAddress"],
                    _configuration["FromEmailPassword"]);
                // smtpClient.UseDefaultCredentials = true; // uncomment if you don't want to use the network credentials
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.EnableSsl = false;
                MailMessage mail = new MailMessage {
                    Subject=subject,Body=content 
                };
                mail.IsBodyHtml = true;

                //Setting From , To and CC
                mail.From = new MailAddress(_configuration["FromEmailAddress"], "Takasla");
                mail.To.Add(new MailAddress(toEmail));
                //mail.CC.Add(new MailAddress("MyEmailID@gmail.com"));

                //smtpClient.Send(mail);
                await smtpClient.SendMailAsync(mail);
            }
            catch (Exception ex)
            {
                
            }
            /*var apiKey = _configuration["SendGridAPIKey"];
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("test@takas.com","Example User");
            var to = new EmailAddress(toEmail);
            var msg = MailHelper.CreateSingleEmail(from, to, subject, content, content);
            var response = await client.SendEmailAsync(msg);*/

        }
    }
}
