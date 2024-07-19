using HMS.Services.Enums;

namespace HMS.Services.FileService
{
    public interface IFileService
    {
        public Tuple<int, string,string> SaveFile(IFormFile file);
        //List<Tuple<int, string, string>> SaveFileToFolder(IEnumerable<IFormFile> files, FolderName folderName);
        Tuple<int, string, string> SaveFileFolder(IFormFile file, FolderName folderName);
        public bool DeleteImage(string filePath);
        public Tuple<int, string, string> UpdateImageInPlace(IFormFile file, string filePath, FolderName folderName);

    }  
}
