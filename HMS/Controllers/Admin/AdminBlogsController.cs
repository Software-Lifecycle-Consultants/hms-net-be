using AutoMapper;
using HMS.DTOs.Admin;
using HMS.Models.Admin;
using HMS.Services.Enums;
using HMS.Services.FileService;
using HMS.Services.MappingService;
using HMS.Services.Repository_Service;
using HMS.Services.RepositoryService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using static HMS.Services.FileService.ImageFileService;

namespace HMS.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminBlogsController : HMSControllerBase<AdminBlogsController, AdminBlog>
    {
        private readonly IFileService _imageFileService;
        private readonly AdminBlogMappingService _mappingService;
        public AdminBlogsController(AdminBlogMappingService mappingService, ILogger<AdminBlogsController> logger, IRepositoryService<AdminBlog> repositoryService, IMapper mapper, IFileService fileService) : base(logger, repositoryService, mapper)
        {
            _imageFileService = fileService;
            _mappingService = mappingService;
        }

        // GET: api/AdminBlogs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AdminBlogReturnDTO>>> GetBlogs()
        {
            try
            {
                _logger.LogInformation("Fetching all Blogs.");

                var blogs = await _repositoryService.GetAllAsync();

                if (!blogs.Any())
                {
                    _logger.LogWarning("No Blogs found.");
                    return NotFound("No Blogs available.");
                }
                return Ok(blogs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching all Blogs.");
                return StatusCode(500, "An error occurred while retrieving Blogs.");
            }

        }

        // GET: api/AdminBlogs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AdminBlogReturnDTO>> GetBlog(Guid id)
        {
            try
            {
                _logger.LogInformation("Fetching Blog by ID: {BlogID}", id);

                var blog = await _repositoryService.GetByIdAsync(id);

                if (blog == null)
                {
                    _logger.LogWarning("Blog with ID {BlogID} not found", id);
                    return NotFound("No Blog found.");
                }

                return Ok(blog);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching Blog by ID: {BlogID}", id);
                return StatusCode(500, "An error occurred while retrieving Blog.");
            }
        }

        // PUT: api/AdminBlogs/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBlog(Guid id, AdminBlogDTO adminBlogDto)
        {
            try
            {
                _logger.LogInformation("Updating Blog with ID: {BlogId}", id);

                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid model state for updating Blog with ID: {BlogId}", id);
                    return BadRequest(ModelState);
                }

                var result = await _mappingService.PutBlog(id, adminBlogDto);
                if (result == null)
                {
                    _logger.LogWarning("Blog with ID: {BlogId} not found for update.", id);
                    return NotFound("No Blog found for update.");
                }
                return Ok(result);
            }
            catch (ApplicationException ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // POST: api/AdminBlogs
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<AdminBlogReturnDTO>> PostBlog(AdminBlogDTO adminBlogDto)
        {
            try
            {
                _logger.LogInformation("Attempting to create a new Blog.");

                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid model state for creating a new Blog");
                    return BadRequest(ModelState);
                }

                var result = await _mappingService.PostBlog(adminBlogDto);
                return CreatedAtAction("GetBlog", new { id = result.Id }, result);
            }
            catch (ApplicationException ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // DELETE: api/AdminBlogs/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBlog(Guid id)
        {
            try
            {
                _logger.LogInformation("Attempting to delete Blog with ID: {BlogId}", id);
                await _mappingService.DeleteBlog(id);
                return Ok($"Blog with ID {id} deleted successfully.");
            }
            catch (ApplicationException ex)
            {
                _logger.LogError(ex, "An error occurred while deleting Blog with ID: {BlogId}", id);
                return StatusCode(500, "An error occurred while while deleting Blog.");
            }
        }
    }
}