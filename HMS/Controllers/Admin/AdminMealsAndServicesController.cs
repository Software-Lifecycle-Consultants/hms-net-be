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
        public async Task<ActionResult<IEnumerable<AdminMealsAndServicesReturnDTO>>> GetAdminMealsAndServices()
        {
            try
            {
                var adminMealsAndServicesReturnDTOs = await _mappingService.GetAdminMealsAndServices();

                if (!adminMealsAndServicesReturnDTOs.Any())
                {
                    return NotFound("No AdminRooms available.");
                }

                return Ok(adminMealsAndServicesReturnDTOs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching all AdminRooms.");
                return StatusCode(500, "An error occurred while retrieving AdminRooms.");
            }

        }

        // GET: api/AdminMealsAndServices/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AdminMealsAndServicesDTO>> GetAdminMealsAndServices(int id)
        {
            try
            {
                _logger.LogInformation("Fetching AdminMealsAndServicesValues by ID: {AdminMealsAndServicesId}", id);
                var adminMealsAndServices = await _adminMASRepositoryService.GetByIdAsync(id);

                if (adminMealsAndServices == null)
                {
                    _logger.LogWarning("AdminMealsAndServicesValues with ID {AdminMealsAndServicesId} not found", id);
                    return NotFound("AdminMealsAndServicesValues not found.");
                }
                var adminMealsAndServicesDTO = _mapper.Map<AdminMealsAndServicesDTO>(adminMealsAndServices);
                return Ok(adminMealsAndServicesDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching AdminMealsAndServicesValues by ID: {AdminMealsAndServicesId}", id);
                return StatusCode(500, "An error occurred while retrieving AdminMealsAndServicesValues.");
            }
        }

        // PUT: api/AdminMealsAndServices/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAdminMealsAndServices(int id, AdminMealsAndServicesDTO adminMealsAndServicesDTO)
        {
            try
            {
                _logger.LogInformation("Updating AdminMealsAndServicesValues with ID: {AdminMealsAndServicesId}", id);

                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid model state for updating AdminMealsAndServicesValues with ID: {AdminMealsAndServicesId}", id);
                    return BadRequest(ModelState);
                }

                var existingAdminMealsAndServices = await _adminMASRepositoryService.GetByIdAsync(id);
                if (existingAdminMealsAndServices == null)
                {
                    _logger.LogWarning("AdminMealsAndServicesValues with ID: {AdminMealsAndServicesId} not found for update.", id);
                    return NotFound($"No AdminMealsAndServicesValues found with ID {id}.");
                }

                _mapper.Map(adminMealsAndServicesDTO, existingAdminMealsAndServices);
                existingAdminMealsAndServices.Id = id; // Explicitly set the Id just to assert control over it.

                _adminMASRepositoryService.Update(existingAdminMealsAndServices);
                await _adminMASRepositoryService.SaveAsync();

                _logger.LogInformation("AdminMealsAndServicesValues with ID: {AdminMealsAndServicesId} updated successfully.", id);
                return NoContent();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex, "Concurrency conflict when updating AdminMealsAndServicesValues with ID: {AdminMealsAndServicesId}", id);
                return StatusCode(409, "Concurrency conflict occurred.");
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database update error when updating AdminMealsAndServicesValues with ID: {AdminMealsAndServicesId}", id);
                return StatusCode(500, "A database error occurred while updating the AdminMealsAndServicesValues.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating AdminMealsAndServicesValues with ID: {AdminMealsAndServicesId}", id);
                return StatusCode(500, "An error occurred while updating the AdminMealsAndServicesValues.");
            }
        }

        // POST: api/AdminMealsAndServices
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<AdminMealsAndServices>> PostAdminMealsAndServices(AdminMealsAndServicesDTO adminMealsAndServicesDTO)
        {
            try
            {
                _logger.LogInformation("Attempting to create a new AdminMealsAndServicesValues.");

                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid model state for creating a new AdminMealsAndServicesValues");
                    return BadRequest(ModelState);
                }

                AdminMealsAndServices adminMealsAndServices = _mapper.Map<AdminMealsAndServices>(adminMealsAndServicesDTO);
                List<AdminMealsAndServicesValue> mealsandservicesValues = new List<AdminMealsAndServicesValue>();
                foreach (var item in adminMealsAndServicesDTO.AdminMealsAndServicesValues)
                {
                    AdminMealsAndServicesValue adminMealsAndServicesvalue = _mapper.Map<AdminMealsAndServicesValue>(item);
                    mealsandservicesValues.Add(adminMealsAndServicesvalue);
                }
                
                adminMealsAndServices.AdminMealsAndServicesValue = mealsandservicesValues;
                await _adminMASRepositoryService.InsertAsync(adminMealsAndServices);

                var resultDto = _mapper.Map<AdminMealsAndServicesDTO>(adminMealsAndServices);
                _logger.LogInformation("Successfully created a new AdminMealsAndServicesValues with ID: {AdminMealsAndServicesId}", adminMealsAndServices.Id);

                return CreatedAtAction("GetAdminMealsAndServices", new { id = adminMealsAndServices.Id }, resultDto);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex, "Concurrency conflict when creating a new AdminMealsAndServicesValues.");
                return StatusCode(409, "Concurrency conflict occurred.");
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database update error occurred while creating a new AdminMealsAndServicesValues.");
                return StatusCode(500, "A database error occurred while creating the AdminMealsAndServicesValues.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while creating a new AdminMealsAndServicesValues.");
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        // DELETE: api/AdminMealsAndServices/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAdminMealsAndServices(int id)
        {
            try
            {
                _logger.LogInformation("Attempting to delete AdminMealsAndServicesValues with ID: {AdminMealsAndServicesId}", id);

                var adminMealsAndServices = await _adminMASRepositoryService.GetByIdAsync(id);
                if (adminMealsAndServices == null)
                {
                    _logger.LogWarning("AdminMealsAndServicesValues with ID: {AdminMealsAndServicesId} not found", id);
                    return NotFound();
                }

                await _adminMASRepositoryService.DeleteAsync(adminMealsAndServices);
                _logger.LogInformation("Successfully deleted AdminMealsAndServicesValues with ID: {AdminMealsAndServicesId}", id);

                return NoContent();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex, "Concurrency conflict when deleting AdminMealsAndServicesValues with ID: {AdminMealsAndServicesId}", id);
                return StatusCode(409, "Concurrency conflict occurred.");
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database update error when deleting AdminMealsAndServicesValues with ID: {AdminMealsAndServicesId}", id);
                return StatusCode(500, "A database error occurred while deleting the AdminMealsAndServicesValues.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred when deleting AdminMealsAndServicesValues with ID: {AdminMealsAndServicesId}", id);
                return StatusCode(500, "An unexpected error occurred.");
            }
        }
    }
}
