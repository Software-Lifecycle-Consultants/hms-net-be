using HMS.Services.Repository_Service;
using HMS.Models.Admin;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AutoMapper;
using HMS.DTOs;
using HMS.Services.FileService;
using HMS.Services.Enums;
using static HMS.Services.FileService.ImageFileService;
using Microsoft.EntityFrameworkCore;
using HMS.DTOs.Admin;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using HMS.Services.RepositoryService;

namespace HMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminRoomImagesController : HMSControllerBase<AdminRoomImagesController, AdminRoomImage>
    {
        private readonly IFileService _imageFileService;

        public AdminRoomImagesController(ILogger<AdminRoomImagesController> logger, IRepositoryService<AdminRoomImage> repositoryService, IMapper mapper, IFileService fileService) : base(logger, repositoryService, mapper)
        {
            _imageFileService = fileService;
        }

        [HttpPost("Cover")]
        public async Task<ActionResult<AdminRoomImageReturnDTO>> PostCoverImage(AdminRoomCoverImageDTO coverImageDto)
        {
            try
            {
                _logger.LogInformation("Uploading cover image for room ID: {RoomId}", coverImageDto.RoomId);

                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid model state for cover image.");
                    return BadRequest(ModelState);
                }

                var fileSaveResult = _imageFileService.SaveFileFolder(coverImageDto.File, FolderName.Images);

                if (fileSaveResult.Item1 != 1)
                {
                    _logger.LogWarning($"Image file {fileSaveResult.Item3} could not be saved: {fileSaveResult.Item2}");
                    return BadRequest($"Image file {fileSaveResult.Item3} could not be saved: {fileSaveResult.Item2}");
                }

                AdminRoomImage adminRoomImage = new AdminRoomImage
                {
                    AdminRoomId = coverImageDto.RoomId,
                    IsCoverImage = true,
                    FilePath = fileSaveResult.Item2,
                    Name = fileSaveResult.Item3
                };

                await _repositoryService.InsertAsync(adminRoomImage);

                AdminRoomImageReturnDTO returnDto = _mapper.Map<AdminRoomImageReturnDTO>(adminRoomImage);
                
                return Ok(returnDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while uploading cover image.");
                return StatusCode(500, "An error occurred while uploading cover image.");
            }
        }

        [HttpPost("Gallery")]
        public async Task<ActionResult<IEnumerable<AdminRoomImageReturnDTO>>> PostAdminImages(AdminRoomGalleryImagesDTO adminRoomGalleryImagesDto)
        {
            try
            {
                _logger.LogInformation("Attempting to upload multiple photos.");

                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid model state for uploading photos.");
                    return BadRequest(ModelState);
                }

                var returnList = new List<AdminRoomImageReturnDTO>();
                var adminImages = new List<AdminRoomImage>();

                if (adminRoomGalleryImagesDto.Files != null && adminRoomGalleryImagesDto.Files.Count > 0)
                {
                    foreach (var file in adminRoomGalleryImagesDto.Files)
                    {
                        var fileSaveResult = _imageFileService.SaveFileFolder(file, FolderName.Images);

                        if (fileSaveResult.Item1 != 1)
                        {
                            _logger.LogWarning($"Image file {fileSaveResult.Item3} could not be saved: {fileSaveResult.Item2}");
                            continue; 
                        }

                        var adminImage = new AdminRoomImage
                        {
                            AdminRoomId = adminRoomGalleryImagesDto.RoomId,
                            IsCoverImage = false,
                            FilePath = fileSaveResult.Item2,
                            Name = fileSaveResult.Item3
                        };

                        _logger.LogInformation($"Prepared image for upload: {adminImage.Name}, IsCoverImage: {adminImage.IsCoverImage}, AdminRoomId: {adminImage.AdminRoomId}");

                        adminImages.Add(adminImage);
                    }

                    if (adminImages.Any())
                    {
                        try
                        {
                            foreach (var adminRoomImage in adminImages)
                            {
                                await _repositoryService.InsertAsync(adminRoomImage);

                                AdminRoomImageReturnDTO returnDto = _mapper.Map<AdminRoomImageReturnDTO>(adminRoomImage);
                                returnList.Add(returnDto);

                               
                            }
                        }
                        catch (Exception)
                        {

                            throw;
                        }
                        
                        returnList = adminImages.Select(img => _mapper.Map<AdminRoomImageReturnDTO>(img)).ToList();
                    }
                }

                if (!returnList.Any())
                {
                    _logger.LogWarning("No images were uploaded successfully.");
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
                _logger.LogError(ex, $"Database update error occurred while uploading photos. Inner exception: {ex.InnerException?.Message}");
                return StatusCode(500, "A database error occurred while uploading the photos.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An unexpected error occurred while uploading photos. Inner exception: {ex.InnerException?.Message}");
                return StatusCode(500, "An unexpected error occurred.");
            }
        }
    }
}
