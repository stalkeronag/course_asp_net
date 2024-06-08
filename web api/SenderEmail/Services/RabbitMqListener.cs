
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SenderEmail.Services.Interfaces;
using System.Data.Common;
using System.Text;
using System.Threading.Channels;

namespace SenderEmail.Services
{
    public class RabbitMqListener
    {
        private readonly IConfiguration configuration;

        private IConnection connection;
        private IModel channel;

        public RabbitMqListener(IConfiguration configuration)
        {
            this.configuration = configuration;
            var factory = new ConnectionFactory { Uri = new Uri(configuration["ConnectionStrings:rabbitmq"]) };
            connection = factory.CreateConnection();
            channel = connection.CreateModel();
            string exchangeName = "direct_email";
            channel.ExchangeDeclare(exchange: exchangeName, type: ExchangeType.Direct);
            var queueName = channel.QueueDeclare(queue: "email_send");
            channel.QueueBind(queue: queueName.QueueName, exchange:exchangeName,routingKey: "email");
        }
        public Task ExecuteAsync(CancellationToken stoppingToken, IHandlerMessageService handler)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var routingKey = ea.RoutingKey;
                Console.WriteLine($" [x] Received '{routingKey}':'{message}'");
                await handler.Handle(body);
            };
            channel.BasicConsume(queue: "email_send",
                                 autoAck: true,
                                 consumer: consumer);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            channel.Close();
            connection.Close();
        }
    }
}
