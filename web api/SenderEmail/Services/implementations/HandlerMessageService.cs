using MailKit.Net.Smtp;
using MimeKit;
using Newtonsoft.Json;
using SenderEmail.Dto;
using SenderEmail.Services.Interfaces;
using System.Text;


namespace SenderEmail.Services.implementations
{
    public class HandlerMessageService : IHandlerMessageService
    {
        private readonly IConfiguration configuration;

        public HandlerMessageService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public async Task Handle(byte[] message)
        {

            using var emailMessage = new MimeMessage();
            var emailDto = JsonConvert.DeserializeObject<EmailDto>(Encoding.UTF8.GetString(message));
            emailMessage.From.Add(new MailboxAddress("test", configuration["EmailFrom"]));
            emailMessage.To.Add(new MailboxAddress("", emailDto.Email));
            emailMessage.Subject = emailDto.Subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Plain)
            {
                Text = emailDto.Body
            };

            using (var client = new SmtpClient())
            {
                var uri = new Uri(configuration["ConnectionStrings:SmtpGoogle"]);

                await client.ConnectAsync(uri);
                await client.AuthenticateAsync(configuration["EmailFrom"], configuration["GooglePassword"]);
                await client.SendAsync(emailMessage);

                await client.DisconnectAsync(true);
            }
        }
    }
}
