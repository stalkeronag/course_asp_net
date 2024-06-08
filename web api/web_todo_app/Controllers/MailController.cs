using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using web_todo_app.Dto;
using WebApi.Data;
using WebApi.Services.Implementations;
using WebApi.Services.Interfaces;

namespace web_todo_app.Controllers
{
    [Route("[controller]")]
    public class MailController : Controller
    {

        private readonly IEmailService emailService;

        public MailController(IEmailService emailService)
        {
            this.emailService = emailService;
        }

        //[Authorize(Roles ="Admin")]
        [HttpPost("SendEmail")]
        //[Authorize(Policy = "IsAdmin")]
        public IActionResult SendEmail(EmailDto emailDto)
        {
            emailService.SendEmail(emailDto);
            return Ok();
        }
    }
}
