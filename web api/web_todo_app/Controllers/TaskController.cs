using Microsoft.AspNetCore.Mvc;
using web_todo_app.Data;
using web_todo_app.Models;

namespace web_todo_app.Controllers
{
    [Route("[controller]")]
    public class TaskController : Controller
    {
        private readonly AppDbContext context;

        public TaskController(AppDbContext context)
        {
            this.context = context;
        }

        [HttpGet("GetTaskById")]
        public IActionResult GetTaskById(string id)
        {
            var task = context.Tasks.Where(task => task.Id.Equals(id)).FirstOrDefault();
            ViewBag.TaskId = id;
            ViewBag.Task = task;
            return View();
        }

        [HttpGet("GetAllTasks")]
        public IActionResult GetAllTasks()
        {
            var tasks = context.Tasks;
            ViewBag.Tasks = tasks.ToList();

            return View();
        }

        [HttpPost("AddTask")]
        public async Task<IActionResult> AddTask(string title, string description)
        {
            var addTask = new TaskModel { Title = title, Description = description };
            context.Tasks.Add(addTask);
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpPost("RemoveTask")]
        public async Task<IActionResult> RemoveTaskById(string id)
        {
            var task = context.Tasks.Where(task => task.Id.Equals(id)).FirstOrDefault();
            context.Tasks.Remove(task);
            await context.SaveChangesAsync();
            return Ok();
        }
    }
}
