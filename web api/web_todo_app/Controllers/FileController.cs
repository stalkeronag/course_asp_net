using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Org.BouncyCastle.Pqc.Crypto.Lms;
using System.IO;
using web_todo_app.Models;
using web_todo_app.Services.interfaces;

namespace web_todo_app.Controllers
{
    [Route("[controller]")]
    public class FileController : Controller
    {

        private readonly IFileService fileService;

        private readonly IFileCacheService cacheService;

        public FileController(IFileService fileService, IFileCacheService cacheService)
        {
            this.fileService = fileService;
            this.cacheService = cacheService;
        }

        [HttpPost("UploadFile")]
        public async Task<IActionResult> UploadFile(IFormFile uploadFile)
        {
            Guid guid = Guid.NewGuid();
            string fileName = guid.ToString();
            var fileModel = new FileModel()
            {
                GeneratedName = fileName,
                FileName = uploadFile.FileName,
                RelativePath = uploadFile.FileName
            };
            string path = await fileService.UploadFile(uploadFile.OpenReadStream(), fileModel);

            await cacheService.CacheFileName(fileName, fileModel.RelativePath);

            return View();
        }

        [Authorize(Policy ="IsAdmin")]
        [HttpGet("SearchFiles")]
        public async Task<IActionResult> SearchFiles(string fileName)
        {
            var path = await cacheService.FindFileInCache(fileName);

            if (path != null) 
            {
                return Ok(path);
            }

            path = await fileService.FindFile(fileName);

            if (path != null)
            {
                return Ok(path);
            }

            return BadRequest();
        }
    }
}
