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
    public class AdminGenaralCatagoriesController : HMSControllerBase<AdminGenaralCatagoriesController, AdminGenaralCatagory>
    {
        public AdminGenaralCatagoriesController(ILogger<AdminGenaralCatagoriesController> logger, IRepositoryService<AdminGenaralCatagory> repositoryService, IMapper mapper) : base(logger, repositoryService, mapper) { }

 

        // GET: api/AdminGenaralCatagories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AdminGenaralCatagory>>> GetAdminGenaralCatagories()
        {
            try
            {
                _logger.LogInformation("Fetching all AdminGenaralCatagories.");
                var AdminGenaralCatagories = await _repositoryService.GetAllAsync();
                if (AdminGenaralCatagories == null || !AdminGenaralCatagories.Any())
                {
                    _logger.LogInformation("No AdminGenaralCatagories found.");
                    return NotFound("No AdminGenaralCatagories available.");
                }
                var AdminGenaralCatagoryDTOs = AdminGenaralCatagories.Select(AdminGenaralCatagory => _mapper.Map<AdminGenaralCatagoryDTO>(AdminGenaralCatagory));
                return Ok(AdminGenaralCatagoryDTOs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching all AdminGenaralCatagories.");
                return StatusCode(500, "An error occurred while retrieving AdminGenaralCatagories.");
            }
        }

        // GET: api/AdminGenaralCatagories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AdminGenaralCatagory>> GetAdminGenaralCatagory(Guid id)
        {
            try
            {
                _logger.LogInformation("Fetching AdminGenaralCatagory by ID: {AdminGenaralCatagoryId}", id);
                var AdminGenaralCatagory = await _repositoryService.GetByIdAsync(id);
                if (AdminGenaralCatagory == null)
                {
                    _logger.LogWarning("AdminGenaralCatagory with ID {AdminGenaralCatagoryId} not found", id);
                    return NotFound("AdminGenaralCatagory not found.");
                }
                var AdminGenaralCatagoryDTO = _mapper.Map<AdminGenaralCatagoryDTO>(AdminGenaralCatagory);
                return Ok(AdminGenaralCatagoryDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching AdminGenaralCatagory by ID: {AdminGenaralCatagoryId}", id);
                return StatusCode(500, "An error occurred while retrieving AdminGenaralCatagory.");
            }
        }

        // PUT: api/AdminGenaralCatagories/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutAdminGenaralCatagory(Guid id, AdminGenaralCatagoryDTO adminGenaralCatagoryDto)
        //{
        //    try
        //    {
        //        _logger.LogInformation("Updating AdminGenaralCatagory with ID: {AdminGenaralCatagoryId}", id);
        //        if (!ModelState.IsValid)
        //        {
        //            _logger.LogWarning("Invalid model state for the AdminGenaralCatagory ID {AdminGenaralCatagoryId}", id);
        //            return BadRequest(ModelState);
        //        }
        //        var existingAdminGenaralCatagory = await _repositoryService.GetByIdAsync(id);
        //        if (existingAdminGenaralCatagory == null)
        //        {
        //            _logger.LogWarning("AdminGenaralCatagory with ID {AdminGenaralCatagoryId} not found", id);
        //            return NotFound("AdminGenaralCatagory not found.");
        //        }
        //        _mapper.Map(adminGenaralCatagoryDto, existingAdminGenaralCatagory);
        //        existingAdminGenaralCatagory.Id = id;

        //        _repositoryService.Update(existingAdminGenaralCatagory);
        //        await _repositoryService.SaveAsync();
        //        _logger.LogInformation("AdminGenaralCatagory updated successfully with ID: {AdminGenaralCatagoryId}", id);
        //        return NoContent();
        //    }
        //    catch (DbUpdateConcurrencyException ex)
        //    {
        //        _logger.LogError(ex, "Concurrency conflict when updating AdminGenaralCatagory with ID: {AdminGenaralCatagoryId}", id);
        //        return StatusCode(409, "Concurrency conflict occurred.");
        //    }
        //    catch (DbUpdateException ex)
        //    {
        //        _logger.LogError(ex, "Database update error occurred while updating AdminGenaralCatagory with ID: {AdminGenaralCatagoryId}", id);
        //        return StatusCode(500, "A database error occurred while updating the AdminGenaralCatagory.");
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "An error occurred while updating AdminGenaralCatagory with ID: {AdminGenaralCatagoryId}", id);
        //        return StatusCode(500, "An error occurred while updating the AdminGenaralCatagory.");
        //    }
        //}
        

            // POST: api/AdminGenaralCatagories
            // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
            [HttpPost]
        public async Task<ActionResult<AdminGenaralCatagory>> PostAdminGenaralCatagory(AdminGenaralCatagoryDTO adminGenaralCatagoryDto)
        {
            try
            {
                _logger.LogInformation("Creating a new AdminGenaralCatagory");
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid model state for the AdminGenaralCatagory");
                    return BadRequest(ModelState);
                }

                AdminGenaralCatagory AdminGenaralCatagory = _mapper.Map<AdminGenaralCatagory>(adminGenaralCatagoryDto);
                await _repositoryService.InsertAsync(AdminGenaralCatagory);
                AdminGenaralCatagoryDTO resultDto = _mapper.Map<AdminGenaralCatagoryDTO>(AdminGenaralCatagory);
                _logger.LogInformation("AdminGenaralCatagory created successfully");
                return CreatedAtAction("GetAdminGenaralCatagory", new { id = AdminGenaralCatagory.Id }, resultDto);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex, "Concurrency conflict when creating a new admin catagory .");
                return StatusCode(409, "Concurrency conflict occurred.");
            }
            catch (DbUpdateException ex)
            {
                // Log database update exceptions
                _logger.LogError(ex, "Database update error occurred while creating a new admin catagory .");
                return StatusCode(500, "A database error occurred while creating the admin catagory .");
            }
            catch (Exception ex)
            {
                // Log unexpected exceptions
                _logger.LogError(ex, "An unexpected error occurred while creating a new admin catagory .");
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        // DELETE: api/AdminGenaralCatagories/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAdminGenaralCatagory(Guid id)
        {
            try
            {
                _logger.LogInformation("Deleting AdminGenaralCatagory with ID: {AdminGenaralCatagoryId}", id);
                var AdminGenaralCatagory = await _repositoryService.GetByIdAsync(id);
                if (AdminGenaralCatagory == null)
                {
                    _logger.LogWarning("AdminGenaralCatagory with ID {AdminGenaralCatagoryId} not found", id);
                    return NotFound($"AdminGenaralCatagory not found ID {id}.");
                }
                await _repositoryService.DeleteAsync(AdminGenaralCatagory);
                _logger.LogInformation("AdminGenaralCatagory deleted successfully with ID: {AdminGenaralCatagoryId}", id);
                return NoContent();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex, "Concurrency conflict when deleting AdminGenaralCatagory with ID: {AdminGenaralCatagoryId}", id);
                return StatusCode(409, "Concurrency conflict occurred.");
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database update error occurred while deleting AdminGenaralCatagory with ID: {AdminGenaralCatagoryId}", id);
                return StatusCode(500, "A database error occurred while deleting the AdminGenaralCatagory.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting AdminGenaralCatagory with ID: {AdminGenaralCatagoryId}", id);
                return StatusCode(500, "An error occurred while deleting the AdminGenaralCatagory.");
            }
        }
          
    }
}
