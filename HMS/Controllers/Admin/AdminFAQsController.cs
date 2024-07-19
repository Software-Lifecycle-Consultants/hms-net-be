using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HMS.Models;
using HMS.Models.Admin;
using HMS.Services.Repository_Service;
using HMS.Services.RepositoryService;
using AutoMapper;
using HMS.DTOs.Admin;
using HMS.DTOs;

namespace HMS.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminFAQsController : HMSControllerBase<AdminFAQsController, AdminFAQ>
    {
        public AdminFAQsController(ILogger<AdminFAQsController> logger, IRepositoryService<AdminFAQ> repositoryService, IMapper mapper) : base(logger, repositoryService, mapper) { } 

        // GET: api/AdminFAQs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AdminFAQ>>> GetAdminFAQ()
        {
            try
            {
                _logger.LogInformation("Fetching all AdminFAQs.");
                var AdminFAQs = await _repositoryService.GetAllAsync();
                if (AdminFAQs == null || !AdminFAQs.Any())
                {
                    _logger.LogInformation("No AdminFAQs found.");
                    return NotFound("No AdminFAQs available.");
                }
                var AdminFAQDTOs = AdminFAQs.Select(AdminFAQ => _mapper.Map<AdminFAQDTO>(AdminFAQ));
                return Ok(AdminFAQDTOs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching all AdminFAQs.");
                return StatusCode(500, "An error occurred while retrieving AdminFAQs.");
            }
        }

        // GET: api/AdminFAQs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AdminFAQ>> GetAdminFAQ(Guid id)
        {
            try
            {
                _logger.LogInformation("Fetching AdminFAQ by ID: {AdminFAQId}", id);
                var AdminFAQ = await _repositoryService.GetByIdAsync(id);

                if (AdminFAQ == null)
                {
                    _logger.LogWarning("AdminFAQ with ID {AdminId} not found", id);
                    return NotFound("AdminFAQ not found.");
                }
                var AdminFAQDTO = _mapper.Map<AdminFAQDTO>(AdminFAQ);
                return Ok(AdminFAQDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching AdminFAQ by ID: {FAQId}", id);
                return StatusCode(500, "An error occurred while retrieving AdminFAQ.");
            }
        }

        // PUT: api/AdminFAQs/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAdminFAQ(Guid id, AdminFAQDTO adminFAQDto)
        {
            try
            {
                _logger.LogInformation("Updating AdminFAQ with ID: {AdminFAQId}", id);

                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid model state for updating AdminFAQ with ID: {AdminFAQId}", id);
                    return BadRequest(ModelState);
                }

                var existingAdminFAQ = await _repositoryService.GetByIdAsync(id);
                if (existingAdminFAQ == null)
                {
                    _logger.LogWarning("AdminFAQ with ID: {AdminFAQId} not found for update.", id);
                    return NotFound($"No AdminFAQ found with ID {id}.");
                }

                _mapper.Map(adminFAQDto, existingAdminFAQ);
                existingAdminFAQ.Id = id; // Explicitly set the Id just to assert control over it.

                _repositoryService.Update(existingAdminFAQ);
                await _repositoryService.SaveAsync();

                _logger.LogInformation("AdminFAQ with ID: {AdminFAQId} updated successfully.", id);
                return NoContent();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex, "Concurrency conflict when updating AdminFAQ with ID: {AdminFAQId}", id);
                return StatusCode(409, "Concurrency conflict occurred.");
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database update error when updating AdminFAQ with ID: {AdminFAQId}", id);
                return StatusCode(500, "A database error occurred while deleting the FAQ.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating AdminFAQ with ID: {AdminFAQId}", id);
                return StatusCode(500, "An error occurred while updating the FAQ.");
            }
        }

        // POST: api/AdminFAQs
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<AdminFAQ>> PostAdminFAQ(AdminFAQDTO adminFAQDto)
        {
            try
            {
                _logger.LogInformation("Attempting to create a new AdminFAQ.");

                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid model state for creating a new AdminFAQ");
                    return BadRequest(ModelState);
                }

                AdminFAQ AdminFAQ = _mapper.Map<AdminFAQ>(adminFAQDto);

                await _repositoryService.InsertAsync(AdminFAQ);

                AdminFAQDTO resultDto = _mapper.Map<AdminFAQDTO>(AdminFAQ);
                _logger.LogInformation("Successfully created a new AdminFAQ with ID: {AdminFAQId}", AdminFAQ.Id);

                return CreatedAtAction("GetAdminFAQ", new { id = AdminFAQ.Id }, resultDto);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex, "Concurrency conflict when creating a new AdminFAQ.");
                return StatusCode(409, "Concurrency conflict occurred.");
            }
            catch (DbUpdateException ex)
            {
                // Log database update exceptions
                _logger.LogError(ex, "Database update error occurred while creating a new AdminFAQ.");
                return StatusCode(500, "A database error occurred while creating the AdminFAQ.");
            }
            catch (Exception ex)
            {
                // Log unexpected exceptions
                _logger.LogError(ex, "An unexpected error occurred while creating a new AdminFAQ.");
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        // DELETE: api/AdminFAQs/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAdminFAQ(Guid id)
        {
            try
            {
                _logger.LogInformation("Attempting to delete AdminFAQ with ID: {AdminFAQId}", id);

                var AdminFAQ = await _repositoryService.GetByIdAsync(id);
                if (AdminFAQ == null)
                {
                    _logger.LogWarning("AdminFAQ with ID: {AdminFAQId} not found", id);
                    return NotFound();
                }

                await _repositoryService.DeleteAsync(AdminFAQ);
                _logger.LogInformation("Successfully deleted AdminFAQ with ID: {AdminFAQId}", id);

                return NoContent();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex, "Concurrency conflict when deleting AdminFAQ with ID: {AdminFAQId}", id);
                return StatusCode(409, "Concurrency conflict occurred.");
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database update error when deleting AdminFAQ with ID: {AdminFAQId}", id);
                return StatusCode(500, "A database error occurred while deleting the FAQ.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred when deleting AdminFAQ with ID: {AdminFAQId}", id);
                return StatusCode(500, "An unexpected error occurred.");
            }
        }
    }
}
