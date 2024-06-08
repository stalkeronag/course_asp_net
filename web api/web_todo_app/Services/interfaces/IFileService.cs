using web_todo_app.Models;

namespace web_todo_app.Services.interfaces
{
    public interface IFileService
    {
        public Task<string> UploadFile(Stream stream, FileModel fileModel);

        public Task DeleteFileByPath(string path);

        public Task SendFile(Stream stream);

        public Task<string> FindFile(string fileName);

    }
}
