using Newtonsoft.Json;
using System.Text;
using web_todo_app.Dto;
using web_todo_app.Services.interfaces;
using WebApi.Services.Interfaces;

namespace web_todo_app.Services.implementations
{
    public class EmailService : IEmailService
    {
        private readonly IRabbitMqService rabbitMqService;

        public EmailService(IRabbitMqService rabbitMqService)
        {
            this.rabbitMqService = rabbitMqService;
        }

        public void SendEmail(EmailDto emailDto)
        {
            var jsonString = JsonConvert.SerializeObject(emailDto);
            var bytes = Encoding.UTF8.GetBytes(jsonString);
            rabbitMqService.SendMessage(bytes);
            
        }
    }
}
