using web_todo_app.Dto;

namespace WebApi.Services.Interfaces
{
    public interface IEmailService
    {
        public void SendEmail(EmailDto emailDto);

    }
}
