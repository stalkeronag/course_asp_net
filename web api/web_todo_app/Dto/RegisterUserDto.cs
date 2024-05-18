using System.ComponentModel.DataAnnotations;

namespace web_todo_app.Dto
{
    public class RegisterUserDto
    {
        
        public string UserName { get; set; }
        
        public string Password { get; set; }
        
        public string PasswordConfirm { get; set; }

        
        public string Email { get; set; }
        
        public string Phone { get; set; }

        public string TelegramId { get; set; }
    }
}
