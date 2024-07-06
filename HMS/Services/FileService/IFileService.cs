using HMS.Services.Enums;

namespace HMS.Services.FileService
{
    public interface IFileService
    {
        public Tuple<int, string,string> SaveFile(IFormFile file);
        List<Tuple<int, string, string>> SaveFileFolder(IEnumerable<IFormFile> files, FolderName folderName);
        public bool DeleteImage(string filePath);
    }
}
