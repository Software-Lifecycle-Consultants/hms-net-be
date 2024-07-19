using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HMS.Models;
using HMS.Models.Admin;
using HMS.DTOs.Admin;
using HMS.Services.Repository_Service;
using HMS.Services.RepositoryService;
using AutoMapper;
using HMS.DTOs;

namespace HMS.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminContactsController : HMSControllerBase<AdminContactsController, AdminContact>
    {
        public AdminContactsController(ILogger<AdminContactsController> logger, IRepositoryService<AdminContact> repositoryService, IMapper mapper) : base(logger, repositoryService, mapper) { } 

        // GET: api/AdminContacts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AdminContact>>> GetAdminContact()
        {
            try
            {
                _logger.LogInformation("Fetching all AdminContacts.");
                var adminContacts = await _repositoryService.GetAllAsync();
                if (adminContacts == null || !adminContacts.Any())
                {
                    _logger.LogInformation("No AdminContacts found.");
                    return NotFound("No AdminContacts available.");
                }
                var adminContactDTOs = adminContacts.Select(adminContact => _mapper.Map<AdminContactDTO>(adminContact));
                return Ok(adminContactDTOs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching all AdminContacts.");
                return StatusCode(500, "An error occurred while retrieving AdminContacts.");
            }
        }

        // GET: api/AdminContacts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AdminContact>> GetAdminContact(Guid id)
        {
            try
            {
                _logger.LogInformation("Fetching AdminContact by ID: {AdminContactId}", id);
                var adminContact = await _repositoryService.GetByIdAsync(id);

                if (adminContact == null)
                {
                    _logger.LogWarning("AdminContact with ID {AdminConatctId} not found", id);
                    return NotFound("AdminContact not found.");
                }
                var adminContactDTO = _mapper.Map<AdminContactDTO>(adminContact);
                return Ok(adminContactDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching AdminContact by ID: {ContactId}", id);
                return StatusCode(500, "An error occurred while retrieving AdminContact.");
            }

        }

        // PUT: api/AdminContacts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAdminContact(Guid id, AdminContactDTO adminContactDto)
        {
            try
            {
                _logger.LogInformation("Updating AdminContact with ID: {AdminContactId}", id);

                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid model state for updating AdminContact with ID: {AdminContactId}", id);
                    return BadRequest(ModelState);
                }

                var existingAdminContact = await _repositoryService.GetByIdAsync(id);
                if (existingAdminContact == null)
                {
                    _logger.LogWarning("AdminContact with ID: {AdminContactId} not found for update.", id);
                    return NotFound($"No AdminContact found with ID {id}.");
                }

                _mapper.Map(adminContactDto, existingAdminContact);
                existingAdminContact.Id = id; // Explicitly set the Id just to assert control over it.

                _repositoryService.Update(existingAdminContact);
                await _repositoryService.SaveAsync();

                _logger.LogInformation("AdminContact with ID: {AdminContactId} updated successfully.", id);
                return NoContent();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex, "Concurrency conflict when updating AdminContact with ID: {AdminContactId}", id);
                return StatusCode(409, "Concurrency conflict occurred.");
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database update error when updating AdminContact with ID: {AdminContactId}", id);
                return StatusCode(500, "A database error occurred while deleting the contact.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating AdminContact with ID: {AdminContactId}", id);
                return StatusCode(500, "An error occurred while updating the contact.");
            }
        }

        // POST: api/AdminContacts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<AdminContactDTO>> PostAdminContact(AdminContactDTO adminContactDto)
        {
            try
            {
                _logger.LogInformation("Attempting to create a new AdminContact.");

                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid model state for creating a new AdminContact");
                    return BadRequest(ModelState);
                }

                AdminContact adminContact = _mapper.Map<AdminContact>(adminContactDto);

                await _repositoryService.InsertAsync(adminContact);

                AdminContactDTO resultDto = _mapper.Map<AdminContactDTO>(adminContact);
                _logger.LogInformation("Successfully created a new AdminContact with ID: {AdminContactId}", adminContact.Id);

                return CreatedAtAction("GetAdminContact", new { id = adminContact.Id }, resultDto);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex, "Concurrency conflict when creating a new AdminContact.");
                return StatusCode(409, "Concurrency conflict occurred.");
            }
            catch (DbUpdateException ex)
            {
                // Log database update exceptions
                _logger.LogError(ex, "Database update error occurred while creating a new AdminContact.");
                return StatusCode(500, "A database error occurred while creating the AdminContact.");
            }
            catch (Exception ex)
            {
                // Log unexpected exceptions
                _logger.LogError(ex, "An unexpected error occurred while creating a new AdminContact.");
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        // DELETE: api/AdminContacts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAdminContact(Guid id)
        {
            try
            {
                _logger.LogInformation("Attempting to delete AdminContact with ID: {AdminContactId}", id);

                var adminContact = await _repositoryService.GetByIdAsync(id);
                if (adminContact == null)
                {
                    _logger.LogWarning("AdminContact with ID: {AdminContactId} not found", id);
                    return NotFound();
                }

                await _repositoryService.DeleteAsync(adminContact);
                _logger.LogInformation("Successfully deleted AdminContact with ID: {AdminContactId}", id);

                return NoContent();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex, "Concurrency conflict when deleting AdminContact with ID: {AdminContactId}", id);
                return StatusCode(409, "Concurrency conflict occurred.");
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database update error when deleting AdminContact with ID: {AdminContactId}", id);
                return StatusCode(500, "A database error occurred while deleting the contact.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred when deleting AdminContact with ID: {AdminContactId}", id);
                return StatusCode(500, "An unexpected error occurred.");
            }
        }
    }
}
