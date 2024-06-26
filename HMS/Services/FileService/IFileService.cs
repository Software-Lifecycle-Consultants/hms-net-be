namespace HMS.Services.FileService
{
    public interface IFileService
    {
        public Tuple<int, string> SaveFile(IFormFile file);
        public Tuple<int, string> SaveFileFolder(IFormFile file, FolderName folderName);
        public bool DeleteImage(string fileName);
    }
}
