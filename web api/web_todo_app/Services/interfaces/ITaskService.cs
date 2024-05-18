using web_todo_app.Models;

namespace WebApi.Services.Interfaces
{
    public interface ITaskService
    {
        public Task AddTask();

        public Task RemoveTask();

        public Task<TaskModel> GetAllExpiredTask();

        public Task<TaskModel> GetTaskById();

        public Task<TaskModel> GetTaskByName();

        public Task<TaskModel> MarkTaskAsDone();

    }
}
