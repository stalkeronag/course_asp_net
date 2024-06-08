using SenderEmail.Services;
using SenderEmail.Services.implementations;
using SenderEmail.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddTransient<IHandlerMessageService, HandlerMessageService>();
var app = builder.Build();

RabbitMqListener rabbitMqListener = new RabbitMqListener(app.Services.GetService<IConfiguration>());
try
{
	var handler = app.Services.GetService<IHandlerMessageService>();
	while (true)
	{
		CancellationToken cancellationToken = new CancellationToken();
		await rabbitMqListener.ExecuteAsync(cancellationToken, handler);
	}
}
catch (Exception)
{

	rabbitMqListener.Dispose();
}