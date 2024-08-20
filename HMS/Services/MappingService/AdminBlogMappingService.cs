using AutoMapper;
using HMS.DTOs.Admin;
using HMS.Models.Admin;
using HMS.Services.Enums;
using HMS.Services.FileService;
using HMS.Services.Repository_Service;
using HMS.Services.RepositoryService;
using static HMS.Services.FileService.ImageFileService;

namespace HMS.Services.MappingService
{
    public class AdminBlogMappingService
    {
        IRepositoryService<AdminBlog> _repositoryService;
        ILogger<AdminBlogMappingService> _logger;
        IMapper _mapper;
        IFileService _imageFileService;
        public AdminBlogMappingService(IRepositoryService<AdminBlog> repositoryService, IMapper mapper, ILogger<AdminBlogMappingService> logger, IFileService imageFileService)
        {
            _repositoryService = repositoryService;
            _mapper = mapper;
            _logger = logger;
            _imageFileService = imageFileService;
        }

        public async Task<IEnumerable<AdminBlogReturnDTO>> GetBlogsAsync()
        {
            try
            {
                _logger.LogInformation("Fetching all Blogs.");

                var blogs = await _repositoryService.GetAllAsync();

                if (blogs == null || !blogs.Any())
                {
                    _logger.LogWarning("No Blogs found.");
                    return Enumerable.Empty<AdminBlogReturnDTO>();
                }

                return blogs.Select(blog => _mapper.Map<AdminBlogReturnDTO>(blog));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching all Blogs.");
                throw;
            }
        }

        public async Task<AdminBlogReturnDTO?> GetBlogByIdAsync(Guid id)
        {
            try
            {
                _logger.LogInformation("Fetching Blog by ID: {BlogID}", id);

                var blog = await _repositoryService.GetByIdAsync(id);

                if (blog == null)
                {
                    _logger.LogWarning("Blog with ID {BlogID} not found", id);
                    return null;
                }

                return _mapper.Map<AdminBlogReturnDTO>(blog);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching Blog by ID: {BlogID}", id);
                throw;
            }
        }

        public async Task<AdminBlog> MapAndSaveImagesAsync(AdminBlogDTO adminBlogDto)
        {
            try
            {
                string? coverImageFilePath = null;
                string? authorImageFilePath = null;

                if (adminBlogDto.CoverImage != null)
                {
                    var fileSaveResult = _imageFileService.SaveFileFolder(adminBlogDto.CoverImage, FolderName.Blogs_CoverImages);
                    if (fileSaveResult.Item1 == (int)FileStatus.Success)
                    {
                        coverImageFilePath = fileSaveResult.Item2;
                    }
                    else
                    {
                        _logger.LogWarning("Failed to save cover image.");
                        throw new Exception("Failed to save cover image.");
                    }
                }

                if (adminBlogDto.AuthorImage != null)
                {
                    var fileSaveResult = _imageFileService.SaveFileFolder(adminBlogDto.AuthorImage, FolderName.Blogs_AuthorImages);
                    if (fileSaveResult.Item1 == (int)FileStatus.Success)
                    {
                        authorImageFilePath = fileSaveResult.Item2;
                    }
                    else
                    {
                        _logger.LogWarning("Failed to save author image.");
                        throw new Exception("Failed to save author image.");
                    }
                }

                var adminBlog = _mapper.Map<AdminBlog>(adminBlogDto);
                adminBlog.CoverImagePath = coverImageFilePath ?? string.Empty;
                adminBlog.AuthorImagePath = authorImageFilePath ?? string.Empty;

                return adminBlog;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while mapping and saving images.");
                throw;
            }
        }

        public async Task<AdminBlog> UpdateImagesAndMapAsync(AdminBlogDTO adminBlogDto, AdminBlog existingBlog)
        {
            try
            {
                // Update Cover Image if needed
                if (adminBlogDto.CoverImage != null)
                {
                    var fileUpdateResult = _imageFileService.UpdateImageInPlace(adminBlogDto.CoverImage, existingBlog.CoverImagePath, FolderName.Blogs_CoverImages);
                    if (fileUpdateResult.Item1 != (int)FileStatus.Success)
                    {
                        _logger.LogWarning("Failed to update cover image for Blog with ID: {BlogID}", existingBlog.Id);
                        throw new Exception("Failed to update cover image.");
                    }
                    existingBlog.CoverImagePath = fileUpdateResult.Item2;
                }

                // Update Author Image if needed
                if (adminBlogDto.AuthorImage != null)
                {
                    var fileUpdateResult = _imageFileService.UpdateImageInPlace(adminBlogDto.AuthorImage, existingBlog.AuthorImagePath, FolderName.Blogs_AuthorImages);
                    if (fileUpdateResult.Item1 != (int)FileStatus.Success)
                    {
                        _logger.LogWarning("Failed to update author image for Blog with ID: {BlogID}", existingBlog.Id);
                        throw new Exception("Failed to update author image.");
                    }
                    existingBlog.AuthorImagePath = fileUpdateResult.Item2;
                }

                // Map the updated fields from DTO to the existing blog entity
                _mapper.Map(adminBlogDto, existingBlog);

                return existingBlog;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating images and mapping Blog with ID: {BlogID}", existingBlog.Id);
                throw;
            }
        }

        public async Task DeleteBlogMapAndImages(AdminBlog blog)
        {
            try
            {
                _logger.LogInformation("Deleting images for Blog with ID: {BlogID}", blog.Id);

                if (!string.IsNullOrEmpty(blog.AuthorImagePath))
                {
                    _imageFileService.DeleteImage(blog.AuthorImagePath);
                }

                if (!string.IsNullOrEmpty(blog.CoverImagePath))
                {
                    _imageFileService.DeleteImage(blog.CoverImagePath);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting images for Blog with ID: {BlogID}", blog.Id);
                throw;
            }
        }


    }

}