using BookSalesApp.IServices;

namespace BookSalesApp.Services
{
    public class FileService:IFileService
    {
        public string GetExtension(IFormFile file)
        {
            return Path.GetExtension(file.FileName);
        }
        public bool CheckImage(IFormFile file)
        {
            string extension = GetExtension(file);
            if (extension == ".png" || extension == ".jpg" || extension == ".jpeg"|| extension == ".jfif")
            {
                return true;
            }
            return false;
        }
        public bool CheckFile(IFormFile file)
        {
            string extension = GetExtension(file);
            if (extension == ".pdf" || extension == ".docx")
            {
                return true;
            }
            return false;
        }
        public async Task<string> AddImage(IFormFile file)
        {
            string f=Guid.NewGuid()+GetExtension(file);
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", f);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }
            return f;
        }
        public async Task<string>AddFile(IFormFile file, string name)
        {
            string f =name+"-"+Guid.NewGuid()+GetExtension(file);
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "files", f);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }
            return f;
        }
        public void DeleteImage(string url)
        {
            File.Delete(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", url));
        }
        public void DeleteFile(string url)
        {
            File.Delete(Path.Combine(Directory.GetCurrentDirectory(), "files", url));
        }
        public bool CheckFileExistance(string url)
        {
            if (File.Exists(url))
                return true;
            return false;
        }
    }
}