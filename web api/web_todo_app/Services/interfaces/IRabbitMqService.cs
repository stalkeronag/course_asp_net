namespace web_todo_app.Services.interfaces
{
    public interface IRabbitMqService
    {
        public void SendMessage(byte[] message);
    }
}
