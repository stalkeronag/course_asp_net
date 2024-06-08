using web_todo_app.Services.interfaces;
using RabbitMQ.Client;
namespace web_todo_app.Services.implementations
{
    public class RabbitMqService : IRabbitMqService
    {
        private readonly IConfiguration configuration;

        public RabbitMqService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void SendMessage(byte[] message)
        {
            string uri = configuration["ConnectionStrings:rabbitmq"];

            var factory = new ConnectionFactory
            {
                Uri = new Uri(uri)
            };

            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
            string exchangeName = "direct_email";
            channel.ExchangeDeclare(exchange: exchangeName, type: ExchangeType.Direct);

            channel.BasicPublish(exchange: exchangeName, routingKey: "email", body:message);
        }
    }
}
