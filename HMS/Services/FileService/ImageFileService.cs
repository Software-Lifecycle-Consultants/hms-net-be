using System.IO;
using HMS.Services.Enums;

namespace HMS.Services.FileService
{
    public class ImageFileService : IFileService
    {
        private IWebHostEnvironment _environment;

        public ImageFileService(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public bool DeleteImage(string fileName)
        {
            try
            {
                var rootPath = _environment.WebRootPath;
                var path = Path.Combine(rootPath, "Uploads\\", fileName);
                if (File.Exists(path))
                {
                    File.Delete(path);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
                //handle this exception gracefully

            }
        }

        public Tuple<int, string> SaveFile(IFormFile file)
        {
            try
            {
                var contentPath = _environment.ContentRootPath;
                var path = Path.Combine(contentPath, "Uploads");
                return Save(file, path);
            }
            catch (Exception ex)
            {

                return new Tuple<int, string>(0, $"An error has occured while saving image file {ex.Message}");
            }


        }
        public Tuple<int, string> SaveFileFolder(IFormFile file, FolderName folderName)
        {

            try
            {
                var contentPath = _environment.ContentRootPath;
                var path = Path.Combine(contentPath, "Uploads", folderName.ToString());

                return Save(file, path);

            }
            catch (Exception ex)
            {

                return new Tuple<int, string>(0, $"An error has occured while saving image file {ex.Message}");
            }
        }


        public string GenerateUniqueFileName(string directoryPath, string imageName)
        {
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(imageName);
            string fileExtension = Path.GetExtension(imageName);
            string uniqueFileName = $"{fileNameWithoutExtension}_{DateTime.Now:yyyyMMddHHmmssfff}{fileExtension}";

            string fullPath = Path.Combine(directoryPath, uniqueFileName);

            int counter = 1;
            while (File.Exists(fullPath))
            {
                uniqueFileName = $"{fileNameWithoutExtension}_{DateTime.Now:yyyyMMddHHmmssfff}_{counter}{fileExtension}";
                fullPath = Path.Combine(directoryPath, uniqueFileName);
                counter++;
            }

            return uniqueFileName;
        }
        private Tuple<int, string> Save(IFormFile file, string directoryPath)
        {
            try
            {
                if (!Directory.Exists(directoryPath))
                    Directory.CreateDirectory(directoryPath);

                var ext = Path.GetExtension(file.FileName);
                var allowedExtensions = new string[] { ".jpg", ".png", ".jpeg" };
                if (!allowedExtensions.Contains(ext))
                {
                    string message = $"Only {string.Join(",", allowedExtensions)} types are allowed";
                    return new Tuple<int, string>(0, message);
                }

                //  string uniqueString = Guid.NewGuid().ToString();
                var uniqeFileName = Guid.NewGuid().ToString() + ext;
                var uniqueFileWithPath = Path.Combine(directoryPath, uniqeFileName);

                using (var stream = new FileStream(uniqueFileWithPath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                return new Tuple<int, string>(1, uniqeFileName);

            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
