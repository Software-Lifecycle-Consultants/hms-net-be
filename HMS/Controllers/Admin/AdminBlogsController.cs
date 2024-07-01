using AutoMapper;
using HMS.DTOs.Admin;
using HMS.Models.Admin;
using HMS.Services.Repository_Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HMS.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminBlogsController : HMSControllerBase<AdminBlogsController, AdminBlog>
    {
        public AdminBlogsController(ILogger<AdminBlogsController> logger, IRepositoryService<AdminBlog> repositoryService, IMapper mapper) : base(logger, repositoryService, mapper) { }

        // GET: api/AdminBlogs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AdminBlogDTO>>> GetBlogs()
        {
            try
            {
                _logger.LogInformation("Fetching all Blogs.");

                var blogs = await _repositoryService.GetAllAsync();

                if (blogs == null || !blogs.Any())
                {
                    _logger.LogWarning("No Blogs found.");
                    return NotFound("No Blogs available.");
                }

                var adminBlogDTOs = blogs.Select(blog => _mapper.Map<AdminBlogDTO>(blog));
                return Ok(adminBlogDTOs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching all Blogs.");
                return StatusCode(500, "An error occurred while retrieving Blogs.");
            }

        }

        // GET: api/AdminBlogs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AdminBlogDTO>> GetBlog(Guid id)
        {
            try
            {
                _logger.LogInformation("Fetching Blog by ID: {BlogID}", id);

                var blog = await _repositoryService.GetByIdAsync(id);

                if (blog == null)
                {
                    _logger.LogWarning("Blog with ID {BlogID} not found", id);
                    return NotFound("Blog not found.");
                }

                var adminBlogDTO = _mapper.Map<AdminBlogDTO>(blog);
                return Ok(adminBlogDTO);
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
                _logger.LogInformation("Updating Blog with ID: {BlogID}", id);

                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid model state for updating Blog with ID: {BlogID}", id);
                    return BadRequest(ModelState);
                }

                var existingBlog = await _repositoryService.GetByIdAsync(id);
                if (existingBlog == null)
                {
                    _logger.LogWarning("Blog with ID: {BlogID} not found for update.", id);
                    return NotFound($"No Blog found with ID {id}.");
                }

                _mapper.Map(adminBlogDto, existingBlog);
                existingBlog.Id = id; // Explicitly set the Id just to assert control over it.

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
                return StatusCode(500, "A database error occurred while deleting the contact.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating Blog with ID: {BlogID}", id);
                return StatusCode(500, "An error occurred while updating the contact.");
            }
        }

        // POST: api/AdminBlogs
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<AdminBlogDTO>> PostBlog([FromForm] AdminBlogDTO adminBlogDto)
        {
            try
            {
                _logger.LogInformation("Attempting to create a new Blog.");

                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid model state for creating a new Blog");
                    return BadRequest(ModelState);
                }

                AdminBlog adminBlog = _mapper.Map<AdminBlog>(adminBlogDto);

                await _repositoryService.InsertAsync(adminBlog);

                AdminBlogDTO resultDto = _mapper.Map<AdminBlogDTO>(adminBlog);
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
                // Log database update exceptions
                _logger.LogError(ex, "Database update error occurred while creating a new Blog.");
                return StatusCode(500, "A database error occurred while creating the Blog.");
            }
            catch (Exception ex)
            {
                // Log unexpected exceptions
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
                return StatusCode(500, "A database error occurred while deleting the contact.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred when deleting Blog with ID: {BlogId}", id);
                return StatusCode(500, "An unexpected error occurred.");
            }
        }
    }
}
