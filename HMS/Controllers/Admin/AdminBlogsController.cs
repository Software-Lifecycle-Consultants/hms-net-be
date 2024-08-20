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
using static HMS.Services.FileService.ImageFileService;

namespace HMS.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminBlogsController : HMSControllerBase<AdminBlogsController, AdminBlog>
    {
        private readonly IFileService _imageFileService;
        private readonly AdminBlogMappingService _mappingService;
        

        public AdminBlogsController(AdminBlogMappingService mappingService, IFileService imageFileService,IMapper mapper, ILogger<AdminBlogsController> logger, IRepositoryService<AdminBlog> repositoryService) : base(logger, repositoryService, mapper)
        {
            _mappingService = mappingService;
            _imageFileService = imageFileService;

        }

        // GET: api/AdminBlogs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AdminBlogReturnDTO>>> GetBlogs()
        {
            var blogs = await _mappingService.GetBlogsAsync();
            if (!blogs.Any())
            {
                return NotFound("No Blogs available.");
            }
            return Ok(blogs);
        }

        // GET: api/AdminBlogs/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBlog(Guid id)
        {
            _logger.LogInformation("Fetching blog with ID: {BlogId}", id);

            var blogDto = await _mappingService.GetBlogByIdAsync(id);
            if (blogDto == null)
            {
                _logger.LogWarning("Blog with ID {BlogId} not found", id);
                return NotFound();
            }
            return Ok(blogDto);
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

                var existingBlog = await _repositoryService.GetByIdAsync(id);
                if (existingBlog == null)
                {
                    _logger.LogWarning("Blog with ID: {BlogID} not found for update.", id);
                    return NotFound($"No Blog found with ID {id}.");
                }

                // Delegate the image update and mapping logic to the mapping service
                await _mappingService.UpdateImagesAndMapAsync(adminBlogDto, existingBlog);

                _repositoryService.Update(existingBlog);
                await _repositoryService.SaveAsync();

                _logger.LogInformation("Blog with ID: {BlogID} updated successfully.", id);
                return NoContent();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex, "Concurrency conflict when updating Blog with ID: {BlogID}", id);
                return StatusCode(409, "Concurrency conflict occurred.");
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database update error when updating Blog with ID: {BlogID}", id);
                return StatusCode(500, "A database error occurred while updating the Blog.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating Blog with ID: {BlogID}", id);
                return StatusCode(500, "An error occurred while updating the Blog.");
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

                var adminBlog = await _mappingService.MapAndSaveImagesAsync(adminBlogDto);
                await _repositoryService.InsertAsync(adminBlog);

                AdminBlogReturnDTO resultDto = _mapper.Map<AdminBlogReturnDTO>(adminBlog);
                _logger.LogInformation("Successfully created a new AdminBlog with ID: {BlogId}", adminBlog.Id);

                return CreatedAtAction("GetBlog", new { id = adminBlog.Id }, resultDto);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex, "Concurrency conflict when creating a new Blog.");
                return StatusCode(409, "Concurrency conflict occurred.");
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database update error occurred while creating a new Blog.");
                return StatusCode(500, "A database error occurred while creating the Blog.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while creating a new Blog.");
                return StatusCode(500, "An unexpected error occurred.");
            }
        }


        // DELETE: api/AdminBlogs/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBlog(Guid id)
        {
            try
            {
                _logger.LogInformation("Attempting to delete Blog with ID: {BlogID}", id);

                var blog = await _repositoryService.GetByIdAsync(id);
                if (blog == null)
                {
                    _logger.LogWarning("Blog with ID: {BlogID} not found", id);
                    return NotFound();
                }

                // Delegate the image deletion logic to the mapping service
                await _mappingService.DeleteBlogMapAndImages(blog);

                await _repositoryService.DeleteAsync(blog);
                _logger.LogInformation("Successfully deleted Blog with ID: {BlogID}", id);

                return NoContent();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex, "Concurrency conflict when deleting Blog with ID: {BlogId}", id);
                return StatusCode(409, "Concurrency conflict occurred.");
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database update error when deleting Blog with ID: {BlogId}", id);
                return StatusCode(500, "A database error occurred while deleting the blog.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred when deleting Blog with ID: {BlogId}", id);
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

    }
}