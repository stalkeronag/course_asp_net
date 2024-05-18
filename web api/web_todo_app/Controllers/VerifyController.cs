using Microsoft.AspNetCore.Mvc;

namespace web_todo_app.Controllers
{
    [Route("[Controller]")]
    public class VerifyController
    {
        [HttpPost("VerifyMail")]
        public Task<IActionResult> VerifyMail()
        {
            throw new NotImplementedException();
        }

        [HttpPost("VerifyPhone")]
        public Task<IActionResult> VerifyPhone()
        {
            throw new NotImplementedException();
        }
    }
}
