using System.IO;
using HMS.Services.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace HMS.Services.FileService
{
    public class ImageFileService : IFileService
    {
        private IWebHostEnvironment _environment;
        protected readonly ILogger<ImageFileService> _logger;

        public enum FileStatus
        {
            Fail = 0,
            Success

        }

        public ImageFileService(IWebHostEnvironment environment, ILogger<ImageFileService> logger)
        {
            _environment = environment;
            _logger = logger;
        }

        public bool DeleteImage(string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, "An error occured while deleting image file");
                return false;

            }
        }

        public Tuple<int, string, string> SaveFile(IFormFile file)
        {
            try
            {
                var contentPath = _environment.ContentRootPath;
                var path = Path.Combine(contentPath, "Uploads");
                return Save(file, path);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, "An error occured while saving image file");
                return new Tuple<int, string, string>((int)FileStatus.Fail, $"An error has occured while saving image file {ex.Message}", string.Empty);
            }


        }
        //public List<Tuple<int, string, string>> SaveFileToFolder(IEnumerable<IFormFile> files, FolderName folderName)
        //{
        //    var results = new List<Tuple<int, string, string>>();
        //    try
        //    {
        //        var contentPath = _environment.ContentRootPath;
        //        var path = Path.Combine(contentPath, "Uploads", folderName.ToString());

        //        foreach (var file in files)
        //        {
        //            var result = Save(file, path);
        //            results.Add(result);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        results.Add(new Tuple<int, string, string>(0, $"An error occurred while saving image file: {ex.Message}", string.Empty));
        //    }

        //    return results;
        //}
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
        private Tuple<int, string, string> Save(IFormFile file, string path)
        {
            try
            {
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                var ext = Path.GetExtension(file.FileName);
                var allowedExtensions = new string[] { ".jpg", ".png", ".jpeg" };

                if (!allowedExtensions.Contains(ext.ToLower()))
                {
                    string message = $"Only {string.Join(",", allowedExtensions)} types are allowed";
                    return new Tuple<int, string, string>((int)FileStatus.Fail, message, string.Empty);
                }

                var uniqeFileName = Guid.NewGuid().ToString() + ext;
                var uniqueFileWithPath = Path.Combine(path, uniqeFileName);

                using (var stream = new FileStream(uniqueFileWithPath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                return new Tuple<int, string, string>(1, uniqueFileWithPath, uniqeFileName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while saving image file");
                return new Tuple<int, string, string>(0, $"An error has occurred while saving image file {ex.Message}", string.Empty);
            }
        }

        public Tuple<int, string, string> SaveFileFolder(IFormFile file, FolderName folderName)
        {
            try
            {
                var contentPath = _environment.ContentRootPath;
                var path = Path.Combine(contentPath, "Uploads", folderName.ToString());

                return Save(file, path);

            }
            catch (Exception ex)
            {
                return new Tuple<int, string, string>(0, $"An error has occured while saving image file {ex.Message}", string.Empty);
            }
        }

        public Tuple<int, string, string> UpdateImageInPlace(IFormFile file, string existingFilePath, FolderName folderName)
        {
            try
            {
                if (file == null)
                {
                    _logger.LogWarning("No file provided for updating.");
                    return new Tuple<int, string, string>((int)FileStatus.Fail, "No file provided for updating.", string.Empty);
                }

                var ext = Path.GetExtension(file.FileName);
                var allowedExtensions = new string[] { ".jpg", ".png", ".jpeg" };

                if (!allowedExtensions.Contains(ext.ToLower()))
                {
                    string message = $"Only {string.Join(",", allowedExtensions)} types are allowed";
                    _logger.LogWarning("Invalid file extension: {FileExtension}", ext);
                    return new Tuple<int, string, string>((int)FileStatus.Fail, message, string.Empty);
                }

                var contentPath = _environment.ContentRootPath;
                var directoryPath = Path.Combine(contentPath, "Uploads", folderName.ToString());

                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                if (!string.IsNullOrEmpty(existingFilePath) && File.Exists(existingFilePath))
                {
                    using (var stream = new FileStream(existingFilePath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }

                    _logger.LogInformation("Image file updated successfully at path: {ExistingFilePath}", existingFilePath);
                    return new Tuple<int, string, string>((int)FileStatus.Success, existingFilePath, Path.GetFileName(existingFilePath));
                }
                else
                {
                    var uniqueFileName = GenerateUniqueFileName(directoryPath, file.FileName);
                    var newFilePath = Path.Combine(directoryPath, uniqueFileName);

                    using (var stream = new FileStream(newFilePath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }

                    _logger.LogInformation("Image file saved successfully at new path: {NewFilePath}", newFilePath);
                    return new Tuple<int, string, string>((int)FileStatus.Success, newFilePath, uniqueFileName);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating image file at path: {ExistingFilePath}", existingFilePath);
                return new Tuple<int, string, string>((int)FileStatus.Fail, $"An error has occurred while updating image file {ex.Message}", string.Empty);
            }
        }







    }
}