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
using HMS.Services.MappingService;

namespace HMS.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminRoomController : HMSControllerBase<AdminRoomController,AdminRoom>
    {
        private readonly IFileService _imageFileService;
        private readonly AdminRoomMappingService _mappingService; //see if you can take this to baseclass with an interface

        public AdminRoomController(AdminRoomMappingService mappingService,IFileService imageFileService,ILogger<AdminRoomController> logger, IAdminRepositoryService repositoryService, IMapper mapper) : base(logger, repositoryService, mapper)
        {
            _imageFileService = imageFileService;
            _mappingService = mappingService;
        }

        // GET: api/AdminRooms
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AdminRoomReturnDTO>>> GetAdminRooms()
        {
            try
            {
                var adminRoomReturnDTOs = await _mappingService.GetAdminRooms();

                if (!adminRoomReturnDTOs.Any())
                {
                    return NotFound("No AdminRooms available.");
                }

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

                var adminRoomReturnDTO = await _mappingService.GetAdminRoomById(id);

                if (adminRoomReturnDTO == null)
                {
                    _logger.LogWarning("AdminRoom with ID {AdminRoomId} not found", id);
                    return NotFound("AdminRoom not found.");
                }

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
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _mappingService.PutAdminRoom(id, adminRoomDto);
                if (result == null)
                {
                    return NotFound($"No AdminRoom found with ID {id}.");
                }
                return Ok(result);
            }
            catch (ApplicationException ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // POST: api/AdminRooms
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<AdminRoomReturnDTO>> PostAdminRoom(AdminRoomDTO adminRoomDto)
        {
            try
            {
                var result = await _mappingService.PostAdminRoom(adminRoomDto);
                return CreatedAtAction(nameof(GetAdminRoom), new { id = result.Id }, result);
            }
            catch (ApplicationException ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // DELETE: api/AdminRooms/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAdminRoom(Guid id)
        {
            try
            {
                _logger.LogInformation("Attempting to delete AdminRoom with ID: {AdminRoomId}", id);
                await _mappingService.DeleteAdminRoom(id);
                return Ok($"AdminRoom with ID {id} deleted successfully.");
            }
            catch (ApplicationException ex)
            {
                _logger.LogError(ex, "An error occurred while deleting.");
                return StatusCode(500, "An error occurred while while deleting.");
            }
        }

        // GET: api/AdminRoom/Summary
        [HttpGet("Summary")]
        public async Task<ActionResult<IEnumerable<AdminRoomSummaryDTO>>> GetAdminRoomSummary()
        {
            try
            {
                var summaries = await _mappingService.GetAdminRoomSummary();
                return Ok(summaries);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching all AdminRoom Summaries.");
                return StatusCode(500, "An error occurred while retrieving AdminRoom Summaries.");
            }

        }

        // GET: api/AdminRooms/5/Summary
        [HttpGet("{id}/Summary")]
        public async Task<ActionResult<AdminRoomSummaryDTO>> GetAdminRoomSummary(Guid id)
        {
            try
            {
                var summery = await _mappingService.GetAdminRoomSummaryById(id);
                var AdminRoomSummaryDTO = _mapper.Map<AdminRoomSummaryDTO>(summery);
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
