using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;
using NextOptimization.Business.DTOs;

namespace NextOptimization.Business.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void SendMail(EmailDTO emailDTO, string template)
        {
            var email = new MimeMessage();

            email.From.Add(MailboxAddress.Parse(_configuration.GetSection("EmailConfig").GetSection("SenderMail").Value));
            email.To.Add(MailboxAddress.Parse(emailDTO.To));

            CreateMessage(emailDTO, template, email);

            SendMail(email);
        }

        private void SendMail(MimeMessage email)
        {
            using (var client = new SmtpClient())
            {
                client.Connect(_configuration.GetSection("EmailConfig").GetSection("Host").Value,
                               Convert.ToInt32(_configuration.GetSection("EmailConfig").GetSection("Port").Value),
                               false);

                client.Authenticate(_configuration.GetSection("EmailConfig").GetSection("SenderMail").Value,
                                    _configuration.GetSection("EmailConfig").GetSection("SenderPassword").Value);

                client.Send(email);
                client.Disconnect(true);
            }
        }

        private void CreateMessage(EmailDTO emailDTO, string template, MimeMessage email)
        {
            email.Subject = emailDTO.Subject;

            var bodyBuilder = new BodyBuilder();
            using (StreamReader reader = File.OpenText(_configuration.GetSection("EmailTemplates").GetSection(template).Value))
            {
                bodyBuilder.HtmlBody = reader.ReadToEnd();
            }

            email.Body = bodyBuilder.ToMessageBody();
        }
    }
}
