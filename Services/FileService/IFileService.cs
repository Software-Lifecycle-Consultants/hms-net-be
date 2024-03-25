namespace HMS.Services.FileService
{
    public interface IFileService
    {
        public Tuple<int, string> SaveFile(IFormFile file);

        public bool DeleteImage(string fileName);
    }
}
