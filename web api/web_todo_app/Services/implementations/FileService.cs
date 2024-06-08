using Nest;
using web_todo_app.Models;
using web_todo_app.Services.interfaces;
using WebApi.Data;

namespace web_todo_app.Services.implementations
{
    public class FileService : IFileService
    {
        private readonly AppDbContext _context;

        private readonly ILogger<FileService> _logger;

        private readonly IWebHostEnvironment _appEnvironment;

        private readonly IConfiguration configuration;
        public FileService(AppDbContext context, ILogger<FileService> logger, IWebHostEnvironment appEnvironment, IConfiguration configuration)
        {
            _context = context;
            _logger = logger;
            _appEnvironment = appEnvironment;
            this.configuration = configuration;
        }
        public Task DeleteFileByPath(string path)
        {
            throw new NotImplementedException();
        }

        public async Task<string> FindFile(string fileName)
        {
            string path = _context.Files.Where(file => file.GeneratedName.Equals(fileName)).FirstOrDefault().RelativePath;

            if (path == null)
            {
                return null;
            }

            return path;
        }

        public Task SendFile(Stream stream)
        {
            throw new NotImplementedException();
        }

        public async Task<string> UploadFile(Stream stream, FileModel fileModel)
        {
            
            string pathDir = configuration["UsersFilesDir"];
            string path = pathDir + fileModel.GeneratedName;
            if (!Directory.Exists(pathDir))
            {
                Directory.CreateDirectory(pathDir);
            }

            _context.Files.Add(fileModel);
            using (var fileStream = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                await stream.CopyToAsync(fileStream);
            }
            await _context.SaveChangesAsync();

            return path;
        }
    }
}
