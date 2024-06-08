namespace SenderEmail.Services.Interfaces
{
    public interface IHandlerMessageService
    {
        public Task Handle(byte[] message);
    }
}
