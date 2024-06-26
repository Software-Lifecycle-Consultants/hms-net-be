namespace HMS.Services.FileService
{
    public interface IFileService
    {
        public Tuple<int, string> SaveFile(IFormFile file);
        public Tuple<int, string> SaveFile(IFormFile file, string folderName);
        public bool DeleteImage(string fileName);
    }
}
