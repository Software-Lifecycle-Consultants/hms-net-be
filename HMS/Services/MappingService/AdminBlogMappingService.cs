using AutoMapper;
using HMS.DTOs.Admin;
using HMS.Models.Admin;
using HMS.Services.Repository_Service;
using HMS.Services.RepositoryService;

namespace HMS.Services.MappingService
{
    public class AdminBlogMappingService
    {
        IRepositoryService<AdminBlog> _repositoryService;
        ILogger<AdminBlogMappingService> _logger;
        IMapper _mapper;
        public AdminBlogMappingService(IRepositoryService<AdminBlog> repositoryService, IMapper mapper, ILogger<AdminBlogMappingService> logger)
        {
            _repositoryService = repositoryService;
            _mapper = mapper;
            _logger = logger;
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
    }

}