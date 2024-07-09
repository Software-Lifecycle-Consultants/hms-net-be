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
using HMS.Services.FileService;
using HMS.Services.Enums;

namespace HMS.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminRoomController : HMSControllerBase<AdminRoomController,AdminRoom>
    {
        private readonly IFileService _imageFileService;

        public AdminRoomController(IFileService imageFileService,ILogger<AdminRoomController> logger, IAdminRepositoryService repositoryService, IMapper mapper) : base(logger, repositoryService, mapper) 
        {
            _imageFileService = imageFileService;
        }
                
        // GET: api/AdminRooms
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AdminRoomReturnDTO>>> GetAdminRooms()
        {
            try
            {
                _logger.LogInformation("Fetching all AdminRooms.");

                var adminRooms = await _adminRepository.GetAllAsync();

                if (adminRooms == null || !adminRooms.Any())
                {
                    _logger.LogWarning("No AdminRooms found.");
                    return NotFound("No AdminRooms available.");
                }

                var adminRoomReturnDTOs = adminRooms.Select(adminRoom => _mapper.Map<AdminRoomReturnDTO>(adminRoom));
                return Ok(adminRoomReturnDTOs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching all AdminRooms.");
                return StatusCode(500, "An error occurred while retrieving AdminRooms.");
            }

        }

        // GET: api/AdminRooms/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AdminRoomReturnDTO>> GetAdminRoom(Guid id)
        {
            try
            {
                _logger.LogInformation("Fetching AdminRoom by ID: {AdminRoomId}", id);

                var adminRoom = await _adminRepository.GetByIdAsync(id);

                if (adminRoom == null)
                {
                    _logger.LogWarning("AdminRoom with ID {AdminRoomId} not found", id);
                    return NotFound("AdminRoom not found.");
                }

                var adminRoomReturnDTO = _mapper.Map<AdminRoomReturnDTO>(adminRoom);
                return Ok(adminRoomReturnDTO);
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

                var existingAdminRoom = await _adminRepository.GetByIdAsync(id);
                if (existingAdminRoom == null)
                {
                    _logger.LogWarning("AdminRoom with ID: {AdminRoomId} not found for update.", id);
                    return NotFound($"No AdminRoom found with ID {id}.");
                }

                _mapper.Map(adminRoomDto, existingAdminRoom);
                existingAdminRoom.Id = id; // Explicitly set the Id just to assert control over it.

                _adminRepository.Update(existingAdminRoom);
                await _adminRepository.SaveAsync();

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
        public async Task<ActionResult<AdminRoomReturnDTO>> PostAdminRoom(AdminRoomDTO adminRoomDto)
        {
            try
            {
                _logger.LogInformation("Attempting to create a new AdminRoom.");

                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid model state for creating a new AdminRoom");
                    return BadRequest(ModelState);
                }
                
                foreach (var item in adminRoomDto.CategoryValuesDictionary)
                {
                    var result = _adminRepository.MapAdminCategory(item.Key,item.Value);
                    adminRoomDto.AdminCategoryValues.Add(new AdminCategoryValueDTO { Value = item.Value, AdminCategoryId = result.Id,AdminCategory =result });
                }

                //Save cover-image
                //Tuple<int, string, string> fileSaveResult;
                //string coverImage = string.Empty;

                //if (adminRoomDto.CoverImage != null)
                //{
                //    fileSaveResult = _imageFileService.SaveFileFolder(adminRoomDto.CoverImage, FolderName.AdminRoom);
                //    if (fileSaveResult.Item1 == 1)
                //        coverImage = fileSaveResult.Item2;
                //}
               
                AdminRoom adminRoom = _mapper.Map<AdminRoom>(adminRoomDto);
                //adminRoom.CoverImagePath = coverImage;

                await _adminRepository.InsertAsync(adminRoom);

                AdminRoomReturnDTO resultDto = _mapper.Map<AdminRoomReturnDTO>(adminRoom);

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

                var adminRoom = await _adminRepository.GetByIdAsync(id);
                if (adminRoom == null)
                {
                    _logger.LogWarning("AdminRoom with ID: {AdminRoomId} not found", id);
                    return NotFound();
                }

                await _adminRepository.DeleteAsync(adminRoom);
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

        // GET: api/AdminRoom/Summary
        [HttpGet("Summary")]
        public async Task<ActionResult<IEnumerable<AdminRoomSummaryDTO>>> GetAdminRoomSummary()
        {
            try
            {
                _logger.LogInformation("Fetching all AdminRoom Summeries.");

                var adminRooms = await _adminRepository.GetAllAsync();

                if (adminRooms == null || !adminRooms.Any())
                {
                    _logger.LogWarning("No AdminRoom Summeries found.");
                    return NotFound("No AdminRoom Summeries available.");
                }

                var AdminRoomSummaryDTOs = adminRooms.Select(adminRoom => _mapper.Map<AdminRoomDTO>(adminRoom));
                return Ok(AdminRoomSummaryDTOs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching all AdminRoom Summeries.");
                return StatusCode(500, "An error occurred while retrieving AdminRoom Summeries.");
            }

        }

        // GET: api/AdminRooms/5/Summary
        [HttpGet("{id}/Summary")]
        public async Task<ActionResult<AdminRoomSummaryDTO>> GetAdminRoomSummary(Guid id)
        {
            try
            {
                _logger.LogInformation("Fetching AdminRoom Summary by ID: {AdminRoomId}", id);

                var adminRoom = await _adminRepository.GetByIdAsync(id);

                if (adminRoom == null)
                {
                    _logger.LogWarning("AdminRoom Summary with ID {AdminRoomId} not found", id);
                    return NotFound("AdminRoom Summary not found.");
                }

                var AdminRoomSummaryDTO = _mapper.Map<AdminRoomSummaryDTO>(adminRoom);
                return Ok(AdminRoomSummaryDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching AdminRoom Summary by ID: {ContactId}", id);
                return StatusCode(500, "An error occurred while retrieving AdminRoom Summary.");
            }
        }
    }
}
