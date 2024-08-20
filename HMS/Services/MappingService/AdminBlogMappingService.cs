using AutoMapper;
using HMS.DTOs.Admin;
using HMS.Models.Admin;
using HMS.Services.Enums;
using HMS.Services.FileService;
using HMS.Services.Repository_Service;
using HMS.Services.RepositoryService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static HMS.Services.FileService.ImageFileService;

namespace HMS.Services.MappingService
{
    public class AdminBlogMappingService
    {
        IRepositoryService<AdminBlog> _repositoryService;
        IFileService _imageFileService;
        ILogger<AdminBlogMappingService> _logger;
        IMapper _mapper;

        public AdminBlogMappingService(IFileService fileService, ILogger<AdminBlogMappingService> logger, IRepositoryService<AdminBlog> repositoryService, IMapper mapper)
        {
            _logger = logger;
            _repositoryService = repositoryService;
            _imageFileService = fileService;
            _mapper = mapper;
        }

        public async Task<IEnumerable<AdminBlogReturnDTO>> GetBlogs()
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

                var adminBlogReturnDTOs = blogs.Select(blog => _mapper.Map<AdminBlogReturnDTO>(blog));
                return adminBlogReturnDTOs;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching all Blogs.");
                throw;
            }
        }

        public async Task<AdminBlogReturnDTO> GetBlogById(Guid id)
        {
            try
            {
                _logger.LogInformation("Fetching Blog by ID: {BlogId}", id);

                var blog = await _repositoryService.GetByIdAsync(id);

                if (blog == null)
                {
                    _logger.LogWarning("Blog with ID {BlogId} not found", id);
                    return null;
                }

                var adminBlogReturnDTO = _mapper.Map<AdminBlogReturnDTO>(blog);
                return adminBlogReturnDTO;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching Blog by ID: {BlogId}", id);
                throw;
            }
        }

        public async Task<AdminBlogReturnDTO> PutBlog(Guid id, AdminBlogDTO adminBlogDto)
        {
            try
            {
                var existingBlog = await _repositoryService.GetByIdAsync(id);
                if (existingBlog == null)
                {
                    _logger.LogWarning("Blog with ID: {BlogID} not found for update.", id);
                    return null;
                }

                // Update Cover Image if needed
                if (adminBlogDto.CoverImage != null)
                {
                    var fileUpdateResult = _imageFileService.UpdateImageInPlace(adminBlogDto.CoverImage, existingBlog.CoverImagePath, FolderName.Blogs_CoverImages);
                    if (fileUpdateResult.Item1 != (int)FileStatus.Success)
                    {
                        _logger.LogWarning("Failed to update cover image for Blog with ID: {BlogID}", id);
                        throw new InvalidOperationException(fileUpdateResult.Item2);
                    }
                    existingBlog.CoverImagePath = fileUpdateResult.Item2;
                }

                // Update Author Image if needed
                if (adminBlogDto.AuthorImage != null)
                {
                    var fileUpdateResult = _imageFileService.UpdateImageInPlace(adminBlogDto.AuthorImage, existingBlog.AuthorImagePath, FolderName.Blogs_AuthorImages);
                    if (fileUpdateResult.Item1 != (int)FileStatus.Success)
                    {
                        _logger.LogWarning("Failed to update author image for Blog with ID: {BlogID}", id);
                        throw new InvalidOperationException(fileUpdateResult.Item2);
                    }
                    existingBlog.AuthorImagePath = fileUpdateResult.Item2;
                }

                _mapper.Map(adminBlogDto, existingBlog);
                existingBlog.Id = id; // Explicitly set the Id just to assert control over it.
                _repositoryService.Update(existingBlog);
                await _repositoryService.SaveAsync();

                _logger.LogInformation("Blog with ID: {BlogID} updated successfully.", id);

                return _mapper.Map<AdminBlogReturnDTO>(existingBlog);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex, "Concurrency conflict when updating Blog with ID: {BlogID}", id);
                throw new ApplicationException("Concurrency conflict occurred.");
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database update error when updating Blog with ID: {BlogID}", id);
                throw new ApplicationException("A database error occurred while updating the Blog.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating Blog with ID: {BlogID}", id);
                throw new ApplicationException("An error occurred while updating the Blog.");
            }
        }

        public async Task<AdminBlogReturnDTO> PostBlog(AdminBlogDTO adminBlogDto)
        {
            try
            {
                _logger.LogInformation("Attempting to create a new Blog.");

                string? coverImageFilePath = default;
                string? coverImageFileName = default;

                if (adminBlogDto.CoverImage != null)
                {
                    var fileSaveResult = _imageFileService.SaveFileFolder(adminBlogDto.CoverImage, FolderName.Blogs_CoverImages);
                    if (fileSaveResult.Item1 == (int)FileStatus.Success)
                    {
                        coverImageFilePath = fileSaveResult.Item2;
                        coverImageFileName = fileSaveResult.Item3;
                    }
                    else
                    {
                        _logger.LogWarning("Image file unsaved.");
                        throw new ApplicationException("Image file unsaved.");
                    }
                }

                string? authorImageFilePath = default;
                string? authorImageFileName = default;

                if (adminBlogDto.AuthorImage != null)
                {
                    var fileSaveResult = _imageFileService.SaveFileFolder(adminBlogDto.AuthorImage, FolderName.Blogs_AuthorImages);
                    if (fileSaveResult.Item1 == (int)FileStatus.Success)
                    {
                        authorImageFilePath = fileSaveResult.Item2;
                        authorImageFileName = fileSaveResult.Item3;
                    }
                    else
                    {
                        _logger.LogWarning("Image file unsaved.");
                        throw new ApplicationException("Image file unsaved.");
                    }
                }

                AdminBlog adminBlog = _mapper.Map<AdminBlog>(adminBlogDto);
                adminBlog.CoverImagePath = coverImageFilePath ?? string.Empty;
                adminBlog.AuthorImagePath = authorImageFilePath ?? string.Empty;
                await _repositoryService.InsertAsync(adminBlog);

                AdminBlogReturnDTO resultDto = _mapper.Map<AdminBlogReturnDTO>(adminBlog);
                _logger.LogInformation("Successfully created a new Blog with ID: {BlogId}", adminBlog.Id);

                return resultDto;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex, "Concurrency conflict when creating a new Blog.");
                throw new ApplicationException("Concurrency conflict occurred.");
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database update error occurred while creating a new Blog.");
                throw new ApplicationException("A database error occurred while creating the Blog.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while creating a new Blog.");
                throw new ApplicationException("An unexpected error occurred.");
            }
        }

        public async Task DeleteBlog(Guid id)
        {
            try
            {
                _logger.LogInformation("Attempting to delete Blog with ID: {BlogID}", id);

                var blog = await _repositoryService.GetByIdAsync(id);
                if (blog == null)
                {
                    _logger.LogWarning("Blog with ID: {BlogID} not found", id);
                    throw new ApplicationException("Blog not found.");
                }

                _imageFileService.DeleteImage(blog.AuthorImagePath);
                _imageFileService.DeleteImage(blog.CoverImagePath);

                await _repositoryService.DeleteAsync(blog);
                _logger.LogInformation("Successfully deleted Blog with ID: {BlogID}", id);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex, "Concurrency conflict when deleting Blog with ID: {BlogId}", id);
                throw new ApplicationException("Concurrency conflict occurred.");
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database update error when deleting Blog with ID: {BlogId}", id);
                throw new ApplicationException("A database error occurred while deleting the Blog.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred when deleting Blog with ID: {BlogId}", id);
                throw new ApplicationException("An unexpected error occurred.");
            }
        }
    }
}