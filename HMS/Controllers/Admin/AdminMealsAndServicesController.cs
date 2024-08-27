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
using HMS.Services.MappingService;

namespace HMS.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminMealsAndServicesController : HMSControllerBase<AdminMealsAndServicesController, AdminMealsAndServices>
    {
        private readonly AdminMealsAndServicesMappingService _mappingService;
        public AdminMealsAndServicesController(AdminMealsAndServicesMappingService mappingService, ILogger<AdminMealsAndServicesController>logger, IAdminMASRepositoryService adminMASRepositoryService, IMapper mapper) : base(logger, adminMASRepositoryService, mapper)
        {
            _mappingService = mappingService;
        }

        // GET: api/AdminMealsAndServices
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AdminMealsAndServicesReturnDTO>>> GetAdminMealsAndServicesAsync()
        {
            try
            {
                _logger.LogInformation("Fetching all AdminMealsAndServices.");
                var adminMealsAndServicesDTOs = await _mappingService.GetAdminMealsAndServicesAsync();

                if (adminMealsAndServicesDTOs == null || !adminMealsAndServicesDTOs.Any())
                {

                    return NotFound("AdminMealsAndServices available.");
                }

                return Ok(adminMealsAndServicesDTOs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching all AdminMealsAndServices.");
                return StatusCode(500, "An error occurred while retrieving AdminMealsAndServices.");
            }

        }

        // GET: api/AdminMealsAndServices/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AdminMealsAndServicesReturnDTO>> GetAdminMealsAndServices(int id)
        {
            try
            {
                _logger.LogInformation("Fetching AdminMealsAndServices by ID: {AdminMealsAndServicesId}", id);

                var adminMealsAndServicesDTO = await _mappingService.GetAdminMealsAndServicesByIdAsync(id);

                if (adminMealsAndServicesDTO == null)
                {
                    _logger.LogWarning("AdminMealsAndServices with ID {AdminMealsAndServicesId} not found.", id);
                    return NotFound("AdminMealsAndServices not found.");
                }

                return Ok(adminMealsAndServicesDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching AdminMealsAndServices by ID: {AdminMealsAndServicesId}", id);
                return StatusCode(500, "An error occurred while retrieving AdminMealsAndServices.");
            }
        }

        // PUT: api/AdminMealsAndServices/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAdminMealsAndServices(int id, AdminMealsAndServicesDTO adminMealsAndServicesDTO)
        {
            try
            {
                _logger.LogInformation("Updating AdminMealsAndServices with ID: {AdminMealsAndServicesId}", id);

                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid model state for updating AdminMealsAndServices with ID: {AdminMealsAndServicesId}", id);
                    return BadRequest(ModelState);
                }

                var updateSuccess = await _mappingService.PutAdminMealsAndServicesAsync(id, adminMealsAndServicesDTO);

                if (!updateSuccess)
                {
                    _logger.LogWarning("AdminMealsAndServices with ID: {AdminMealsAndServicesId} not found for update.", id);
                    return NotFound($"No AdminMealsAndServices found with ID {id}.");
                }

                _logger.LogInformation("AdminMealsAndServices with ID: {AdminMealsAndServicesId} updated successfully.", id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating AdminMealsAndServices with ID: {AdminMealsAndServicesId}", id);
                return StatusCode(500, "An error occurred while updating the AdminMealsAndServices.");
            }
        }

        // POST: api/AdminMealsAndServices
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<AdminMealsAndServicesReturnDTO>> PostAdminMealsAndServices(AdminMealsAndServicesDTO adminMealsAndServicesDTO)
        {
            try
            {
                _logger.LogInformation("Attempting to create a new AdminMealsAndServices.");

                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid model state for creating a new AdminMealsAndServices.");
                    return BadRequest(ModelState);
                }

                var createdAdminMealsAndServicesDTO = await _mappingService.PostAdminMealsAndServicesAsync(adminMealsAndServicesDTO);

                _logger.LogInformation("Successfully created a new AdminMealsAndServices with ID: {AdminMealsAndServicesId}", createdAdminMealsAndServicesDTO.Id);

                return CreatedAtAction("GetAdminMealsAndServices", new { id = createdAdminMealsAndServicesDTO.Id }, createdAdminMealsAndServicesDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a new AdminMealsAndServices.");
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        // DELETE: api/AdminMealsAndServices/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAdminMealsAndServices(int id)
        {
            try
            {
                _logger.LogInformation("Attempting to delete AdminMealsAndServices with ID: {AdminMealsAndServicesId}", id);

                var deleteSuccess = await _mappingService.DeleteAdminMealsAndServicesAsync(id);

                if (!deleteSuccess)
                {
                    _logger.LogWarning("AdminMealsAndServices with ID: {AdminMealsAndServicesId} not found.", id);
                    return NotFound();
                }

                _logger.LogInformation("Successfully deleted AdminMealsAndServices with ID: {AdminMealsAndServicesId}", id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting AdminMealsAndServices with ID: {AdminMealsAndServicesId}", id);
                return StatusCode(500, "An unexpected error occurred.");
            }
        }
    }
}
