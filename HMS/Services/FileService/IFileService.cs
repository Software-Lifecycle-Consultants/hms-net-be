using HMS.Services.Enums;

namespace HMS.Services.FileService
{
    public interface IFileService
    {
        public Tuple<int, string,string> SaveFile(IFormFile file);
        public Tuple<int, string,string> SaveFileFolder(IFormFile file, FolderName folderName);
        public bool DeleteImage(string filePath);
    }
}
