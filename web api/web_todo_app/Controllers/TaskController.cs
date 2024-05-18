using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using web_todo_app.Dto;
using web_todo_app.Models;
using WebApi.Data;

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

        [Authorize]
        [HttpGet("GetTaskById")]
        
        public IActionResult GetTaskById(string id)
        {
            var task = context.Tasks.Where(task => task.Id.Equals(id)).FirstOrDefault();
            ViewBag.TaskId = id;
            ViewBag.Task = task;
            return View();
        }

        [Authorize]
        [HttpGet("GetAllTasks")]
        public IActionResult GetAllTasks()
        {
            var tasks = context.Tasks;
            ViewBag.Tasks = tasks.ToList();

            return View();
        }

        [Authorize]
        [HttpPost("AddTask")]
        public async Task<IActionResult> AddTask([FromBody] AddTaskDto addTaskDto)
        {
            var addTask = new TaskModel { Title = addTaskDto.Title, Description = addTaskDto.Description };
            context.Tasks.Add(addTask);
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpPost("RemoveTask")]
        [Authorize(Roles="Admin")]
        public async Task<IActionResult> RemoveTaskById(string id)
        {
            var task = context.Tasks.Where(task => task.Id.Equals(id)).FirstOrDefault();
            context.Tasks.Remove(task);
            await context.SaveChangesAsync();
            return Ok();
        }
    }
}
