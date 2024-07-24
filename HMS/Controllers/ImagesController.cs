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
using static HMS.Services.FileService.ImageFileService;

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
        public async Task<ActionResult<IEnumerable<ImageReturnDTO>>> PostImages(ImageDTO imageDto)
        {
            try
            {
                _logger.LogInformation("Attempting to upload multiple photos.");

                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid model state for uploading photos.");
                    return BadRequest(ModelState);
                }

                List<ImageReturnDTO> returnList = new List<ImageReturnDTO>();
                if (imageDto.Files?.Count > 0)
                {
                    foreach (var file in imageDto.Files)
                    {
                        Tuple<int, string, string> fileSaveResult = _imageFileService.SaveFileFolder(file, FolderName.Images);

                        if (fileSaveResult.Item1 != 1)
                        {
                            _logger.LogWarning($"Image file {fileSaveResult.Item3} could not be saved: {fileSaveResult.Item2}");
                            returnList.Add(new ImageReturnDTO
                            {
                                FilePath = fileSaveResult.Item2,
                                Success = false,
                                ErrorMessage = $"Image file {fileSaveResult.Item3} could not be saved: {fileSaveResult.Item2}"
                            });
                            continue; // Skip to the next file if current one failed
                        }

                        Image image = _mapper.Map<Image>(imageDto);
                        image.FilePath = fileSaveResult.Item2;
                        image.Name = fileSaveResult.Item3;

                        try
                        {
                            await _repositoryService.InsertAsync(image);

                            // Generate ImageReturnDTO with ImageID
                            ImageReturnDTO returnObject = _mapper.Map<ImageReturnDTO>(image);
                            returnObject.Success = true;
                            returnList.Add(returnObject);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, $"Failed to save image details for file {fileSaveResult.Item3}");
                            returnList.Add(new ImageReturnDTO
                            {
                                FilePath = fileSaveResult.Item2,
                                Success = false,
                                ErrorMessage = $"Failed to save image details: {ex.Message}"
                            });
                        }
                    }
                }
                if (returnList.Count == 0)
                {
                    return BadRequest("No images were uploaded successfully.");
                }

                return Ok(returnList);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex, "Concurrency conflict when uploading photos.");
                return StatusCode(409, "Concurrency conflict occurred.");
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database update error occurred while uploading photos.");
                return StatusCode(500, "A database error occurred while uploading the photos.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while uploading photos.");
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