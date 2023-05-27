namespace BookSalesApp.IServices
{
    public interface IFileService
    {
        public string GetExtension(IFormFile file);
        public bool CheckImage(IFormFile file);
        public bool CheckFile(IFormFile file);
        public Task<string> AddImage(IFormFile file);
        public Task<string> AddFile(IFormFile file, string name);
        public void DeleteFile(string url);
        public void DeleteImage(string url);
        public bool CheckFileExistance(string url);
    }
}
