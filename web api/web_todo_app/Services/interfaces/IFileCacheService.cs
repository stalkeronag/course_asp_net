namespace web_todo_app.Services.interfaces
{
    public interface IFileCacheService
    {
        public Task CacheFileName(string fileName, string path);

        public Task<string> FindFileInCache(string fileName);
    }
}
