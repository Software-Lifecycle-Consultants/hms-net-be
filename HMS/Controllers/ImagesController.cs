using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HMS.Models;
using AutoMapper;
using HMS.DTOs;
using HMS.Services.Repository_Service;
using HMS.DTOs.Admin;
using HMS.Services.FileService;
using HMS.Services.Enums;

namespace HMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : HMSControllerBase<ImagesController, Image>
    {
        private readonly IFileService _imageFileService;

        public ImagesController(ILogger<ImagesController> logger, IRepositoryService<Image> repositoryService, IMapper mapper, IFileService fileService) : base(logger, repositoryService, mapper)
        {
            _imageFileService = fileService;
        }

        // GET: api/Images
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ImageDTO>>> GetImages()
        {
            try
            {
                _logger.LogInformation("Getting all images");
                var images = await _repositoryService.GetAllAsync();

                if (images == null || !images.Any())
                {
                    _logger.LogWarning("No images found");
                    return NotFound("No Images Found");
                }

                var imagesDTO = images.Select(i => _mapper.Map<ImageDTO>(i)).ToList();
                return Ok(imagesDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occured while fetching all Images.");
                return StatusCode(500, "An error occured while retrieving Images.");
            }
        }

        // GET: api/Images/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ImageDTO>> GetImage(Guid id)
        {

            try
            {
                _logger.LogInformation("Fetching Image by ID: {ImageID}", id);

                var image = await _repositoryService.GetByIdAsync(id);

                if (image == null)
                {
                    _logger.LogWarning("Image with ID {ImageId} not found", id);
                    return NotFound("Image not found.");
                }

                var imageDTO = _mapper.Map<ImageDTO>(image);
                return Ok(imageDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching Image by ID: {ContactId}", id);
                return StatusCode(500, "An error occurred while retrieving Image.");
            }
        }

        // PUT: api/Images/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutImage(Guid id, ImageDTO imageDto)
        {
            try
            {
                _logger.LogInformation("Updating Image with ID: {ImageId}", id);

                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid model state for updating Image with ID: {ImageId}", id);
                    return BadRequest(ModelState);
                }

                var existingImage = await _repositoryService.GetByIdAsync(id);
                if (existingImage == null)
                {
                    _logger.LogWarning("Image with ID: {ImageId} not found for update.", id);
                    return NotFound($"No Image found with ID {id}.");
                }

                _mapper.Map(imageDto, existingImage);
                existingImage.Id = id; // Explicitly set the Id just to assert control over it.

                _repositoryService.Update(existingImage);
                await _repositoryService.SaveAsync();

                _logger.LogInformation("Image with ID: {ImageID} updated successfully.", id);
                return NoContent();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex, "Concurrency conflict when updating Image with ID: {ImageId}", id);
                return StatusCode(409, "Concurrency conflict occurred.");
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database update error when updating Image with ID: {ImageId}", id);
                return StatusCode(500, "A database error occurred while deleting the contact.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating Image with ID: {ImageId}", id);
                return StatusCode(500, "An error occurred while updating the contact.");
            }
        }

        // POST: api/Images
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Image>> PostImage([FromForm] ImageDTO imageDto)
        {
            try
            {
                _logger.LogInformation("Attempting to upload a new photo.");

                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid model state for uploading a new photo.");
                    return BadRequest(ModelState);
                }

                Tuple<int, string,string> fileSaveResult;
                string? filePath = default;
                string? fileName = default;
                
                if (imageDto.File != null)
                {
                    fileSaveResult = _imageFileService.SaveFileFolder(imageDto.File, FolderName.Images);
                    if (fileSaveResult.Item1 == 1)
                    {
                        filePath = fileSaveResult.Item2;
                        fileName = fileSaveResult.Item3;
                    }
                    else
                    {
                        _logger.LogWarning("Image file unsaved.");
                        return BadRequest(ModelState);
                    }
                }
                Image image = _mapper.Map<Image>(imageDto);                
                image.FilePath = filePath ?? string.Empty; // Ensure filePath is not null
                image.Name = fileName;

                await _repositoryService.InsertAsync(image);
                return CreatedAtAction("GetImage", new { id = image.Id }, image);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex, "Concurrency conflict when uploading a new photo.");
                return StatusCode(409, "Concurrency conflict occurred.");
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database update error occurred while uploading a new photo.");
                return StatusCode(500, "A database error occurred while uploading the photo.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while uploading a new photo.");
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        // DELETE: api/Images/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteImage(Guid id)
        {
            try
            {
                _logger.LogInformation("Attempting to delete Image with ID: {ImageId}", id);
                var image = await _repositoryService.GetByIdAsync(id);

                if (image == null)
                {
                    _logger.LogWarning("Image with ID: {ImageId} not found for deletion.", id);
                    return NotFound($"No Image found with ID {id}.");
                }
                await _repositoryService.DeleteAsync(image);

                //delete from physical locatin
                _imageFileService.DeleteImage(image.FilePath);

                _logger.LogInformation("Image with ID: {ImageId} deleted successfully.", id);
                return NoContent();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex, "Concurrency conflict when deleting Image with ID: {ImageId}", id);
                return StatusCode(409, "Concurrency conflict occurred.");
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database update error occurred while deleting Image with ID: {ImageId}", id);
                return StatusCode(500, "A database error occurred while deleting the Image.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while deleting Image with ID: {ImageId}", id);
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

    }
}
