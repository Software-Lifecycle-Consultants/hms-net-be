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
    public class AdminRoomController : HMSControllerBase<AdminRoomController,AdminRoom>
    {
        
        public AdminRoomController(ILogger<AdminRoomController> logger, IRepositoryService<AdminRoom> repositoryService, IMapper mapper) : base(logger, repositoryService, mapper) { }
        

        // GET: api/AdminRooms
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AdminRoomDTO>>> GetAdminRooms()
        {
            try
            {
                _logger.LogInformation("Fetching all AdminRooms.");

                var adminRooms = await _repositoryService.GetAllAsync();

                if (adminRooms == null || !adminRooms.Any())
                {
                    _logger.LogWarning("No AdminRooms found.");
                    return NotFound("No AdminRooms available.");
                }

                var adminRoomDTOs = adminRooms.Select(adminRoom => _mapper.Map<AdminRoomDTO>(adminRoom));
                return Ok(adminRoomDTOs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching all AdminRooms.");
                return StatusCode(500, "An error occurred while retrieving AdminRooms.");
            }

        }

        // GET: api/AdminRooms/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AdminRoomDTO>> GetAdminRoom(Guid id)
        {
            try
            {
                _logger.LogInformation("Fetching AdminRoom by ID: {AdminRoomId}", id);

                var adminRoom = await _repositoryService.GetByIdAsync(id);

                if (adminRoom == null)
                {
                    _logger.LogWarning("AdminRoom with ID {AdminRoomId} not found", id);
                    return NotFound("AdminRoom not found.");
                }

                var adminRoomDTO = _mapper.Map<AdminRoomDTO>(adminRoom);
                return Ok(adminRoomDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching AdminRoom by ID: {ContactId}", id);
                return StatusCode(500, "An error occurred while retrieving AdminRoom.");
            }
        }

        // PUT: api/AdminRooms/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAdminRoom(Guid id, AdminRoomDTO adminRoomDto)
        {
            try
            {
                _logger.LogInformation("Updating AdminRoom with ID: {AdminRoomId}", id);

                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid model state for updating AdminRoom with ID: {AdminRoomId}", id);
                    return BadRequest(ModelState);
                }

                var existingAdminRoom = await _repositoryService.GetByIdAsync(id);
                if (existingAdminRoom == null)
                {
                    _logger.LogWarning("AdminRoom with ID: {AdminRoomId} not found for update.", id);
                    return NotFound($"No AdminRoom found with ID {id}.");
                }

                _mapper.Map(adminRoomDto, existingAdminRoom);
                existingAdminRoom.Id = id; // Explicitly set the Id just to assert control over it.

                _repositoryService.Update(existingAdminRoom);
                await _repositoryService.SaveAsync();

                _logger.LogInformation("AdminRoom with ID: {AdminRoomId} updated successfully.", id);
                return NoContent();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex, "Concurrency conflict when updating AdminRoom with ID: {AdminRoomId}", id);
                return StatusCode(409, "Concurrency conflict occurred.");
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database update error when updating AdminRoom with ID: {AdminRoomId}", id);
                return StatusCode(500, "A database error occurred while deleting the contact.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating AdminRoom with ID: {AdminRoomId}", id);
                return StatusCode(500, "An error occurred while updating the contact.");
            }
        }

        // POST: api/AdminRooms
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<AdminRoomDTO>> PostAdminRoom([FromForm]AdminRoomDTO adminRoomDto)
        {
            try
            {
                _logger.LogInformation("Attempting to create a new AdminRoom.");

                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid model state for creating a new AdminRoom");
                    return BadRequest(ModelState);
                }

                AdminRoom adminRoom = _mapper.Map<AdminRoom>(adminRoomDto);

                await _repositoryService.InsertAsync(adminRoom);

                ContactDTO resultDto = _mapper.Map<ContactDTO>(adminRoom);
                _logger.LogInformation("Successfully created a new AdminRoom with ID: {AdminRoomId}", adminRoom.Id);

                return CreatedAtAction("GetAdminRoom", new { id = adminRoom.Id }, resultDto);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex, "Concurrency conflict when creating a new AdminRoom.");
                return StatusCode(409, "Concurrency conflict occurred.");
            }
            catch (DbUpdateException ex)
            {
                // Log database update exceptions
                _logger.LogError(ex, "Database update error occurred while creating a new AdminRoom.");
                return StatusCode(500, "A database error occurred while creating the AdminRoom.");
            }
            catch (Exception ex)
            {
                // Log unexpected exceptions
                _logger.LogError(ex, "An unexpected error occurred while creating a new AdminRoom.");
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        // DELETE: api/AdminRooms/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAdminRoom(Guid id)
        {
            try
            {
                _logger.LogInformation("Attempting to delete AdminRoom with ID: {AdminRoomId}", id);

                var adminRoom = await _repositoryService.GetByIdAsync(id);
                if (adminRoom == null)
                {
                    _logger.LogWarning("AdminRoom with ID: {AdminRoomId} not found", id);
                    return NotFound();
                }

                await _repositoryService.DeleteAsync(adminRoom);
                _logger.LogInformation("Successfully deleted AdminRoom with ID: {AdminRoomId}", id);

                return NoContent();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex, "Concurrency conflict when deleting AdminRoom with ID: {AdminRoomId}", id);
                return StatusCode(409, "Concurrency conflict occurred.");
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database update error when deleting AdminRoom with ID: {AdminRoomId}", id);
                return StatusCode(500, "A database error occurred while deleting the contact.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred when deleting AdminRoom with ID: {AdminRoomId}", id);
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

       
    }
}
