using AutoMapper;
using HMS.Controllers.Admin;
using HMS.DTOs.Admin;
using HMS.Models.Admin;
using HMS.Services.RepositoryService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HMS.Services.MappingService
{
    public class AdminRoomMappingService
    {
        IAdminRepositoryService _adminRepository;
        ILogger<AdminRoomMappingService> _logger;
        IMapper _mapper;
        public AdminRoomMappingService(IAdminRepositoryService adminRepository, IMapper mapper, ILogger<AdminRoomMappingService> logger)
        {
            _adminRepository = adminRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<AdminRoomReturnDTO>> GetAdminRooms()
        {
            try
            {
                _logger.LogInformation("Fetching all AdminRooms.");

                var adminRooms = await _adminRepository.GetAllAsync();

                if (adminRooms == null || !adminRooms.Any())
                {
                    _logger.LogWarning("No AdminRooms found.");
                    return Enumerable.Empty<AdminRoomReturnDTO>();
                }

                var adminRoomReturnDTOs = new List<AdminRoomReturnDTO>();

                foreach (var adminRoom in adminRooms)
                {
                    if (!IsValidPrice(adminRoom.Price))
                    {
                        decimal correctedPrice = Math.Round(adminRoom.Price,2);
                        adminRoom.Price = correctedPrice;
                        _adminRepository.Update(adminRoom);
                        await _adminRepository.SaveAsync();
                        //A function should be written to correct if the minus value is entered for the price
                    }
                    var adminRoomDto = _mapper.Map<AdminRoomReturnDTO>(adminRoom);
                    adminRoomDto.AdminCategoryValues = await GetCategoryValueDTOs(adminRoom);
                    adminRoomReturnDTOs.Add(adminRoomDto);
                }

                return adminRoomReturnDTOs;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching all AdminRooms.");
                throw; // Optionally rethrow the exception or handle it as needed.
            }
        }

        public async Task<AdminRoomReturnDTO?> GetAdminRoomById(Guid id)
        {
            try
            {
                _logger.LogInformation("Fetching AdminRoom by ID: {AdminRoomId}", id);
                var adminRoom = await _adminRepository.GetByIdAsync(id);
                if (adminRoom == null)
                {
                    _logger.LogWarning("AdminRoom with ID {AdminRoomId} not found", id);
                    return null;
                }

                if (!IsValidPrice(adminRoom.Price))
                {
                    decimal correctedPrice = Math.Round(adminRoom.Price, 2);
                    adminRoom.Price = correctedPrice;
                    _adminRepository.Update(adminRoom);
                    await _adminRepository.SaveAsync();
                }

                var adminRoomDto = _mapper.Map<AdminRoomReturnDTO>(adminRoom);
                adminRoomDto.AdminCategoryValues = await GetCategoryValueDTOs(adminRoom);

                return adminRoomDto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching AdminRoom by ID: {AdminRoomId}", id);
                throw;
            }
        }

        public async Task<AdminRoomReturnDTO?> PutAdminRoom(Guid id, AdminRoomDTO adminRoomDto)
        {
            try
            {
                _logger.LogInformation("Updating AdminRoom with ID: {AdminRoomId}", id);

                var existingAdminRoom = await _adminRepository.GetByIdAsync(id);
                if (existingAdminRoom == null)
                {
                    _logger.LogWarning("AdminRoom with ID: {AdminRoomId} not found for update.", id);
                    return null;
                }

                _mapper.Map(adminRoomDto, existingAdminRoom);
                existingAdminRoom.Id = id; // Explicitly set the Id just to assert control over it.

                // Update CategoryValues
                AdminRoom adminRoom = _mapper.Map<AdminRoom>(adminRoomDto);
                List<CategoryValue> categoryValues = new List<CategoryValue>();

                foreach (var item in adminRoomDto.CategoryValuesDictionary)
                {
                    //before assigning AdminCategoryValuesId, we need to see if that entry exists in the DB, if not it throws an exception with FK mapping
                    if (await _adminRepository.CategoryValueExists(item.Value))
                    {
                        categoryValues.Add(new CategoryValue { AdminCategoryValuesId = item.Value, AdminRoomId = adminRoom.Id }); ;
                    }

                }
                adminRoom.CategoryValues = categoryValues;

                _adminRepository.Update(existingAdminRoom);
                await _adminRepository.SaveAsync();

                _logger.LogInformation("AdminRoom with ID: {AdminRoomId} updated successfully.", id);

                // Map the updated AdminRoom to AdminRoomReturnDTO
                var updatedAdminRoomDto = _mapper.Map<AdminRoomReturnDTO>(existingAdminRoom);
                updatedAdminRoomDto.AdminCategoryValues = await GetCategoryValueDTOs(existingAdminRoom);

                return updatedAdminRoomDto;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex, "Concurrency conflict when updating AdminRoom with ID: {AdminRoomId}", id);
                throw new ApplicationException("Concurrency conflict occurred.", ex);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database update error when updating AdminRoom with ID: {AdminRoomId}", id);
                throw new ApplicationException("A database error occurred while updating the AdminRoom.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating AdminRoom with ID: {AdminRoomId}", id);
                throw new ApplicationException("An error occurred while updating the contact.");
            }

        }

        public async Task<AdminRoomReturnDTO> PostAdminRoom(AdminRoomDTO adminRoomDto)
        {
            try
            {
                _logger.LogInformation("Attempting to create a new AdminRoom.");

                AdminRoom adminRoom = _mapper.Map<AdminRoom>(adminRoomDto);
                List<CategoryValue> categoryValues = new List<CategoryValue>();

                foreach (var item in adminRoomDto.CategoryValuesDictionary)
                {
                    //before assigning AdminCategoryValuesId, we need to see if that entry exists in the DB, if not it throws an exception with FK mapping
                    if (await _adminRepository.CategoryValueExists(item.Value))
                    {
                        categoryValues.Add(new CategoryValue { AdminCategoryValuesId = item.Value, AdminRoomId = adminRoom.Id }); ;
                    }

                    //var result = _adminRepository.MapAdminCategory(item.Key,item.Value);
                    //adminRoomDto.AdminCategoryValues.Add(new AdminCategoryValueDTO { Value = item.Value, AdminCategoryId = result.Id,AdminCategory =result });
                }
                adminRoom.CategoryValues = categoryValues;

                //Save cover-image
                //Tuple<int, string, string> fileSaveResult;
                //string coverImage = string.Empty;

                //if (adminRoomDto.CoverImage != null)
                //{
                //    fileSaveResult = _imageFileService.SaveFileFolder(adminRoomDto.CoverImage, FolderName.AdminRoom);
                //    if (fileSaveResult.Item1 == 1)
                //        coverImage = fileSaveResult.Item2;
                //}

                //AdminRoom adminRoom = _mapper.Map<AdminRoom>(adminRoomDto);
                //adminRoom.CategoryValues.Add(new CategoryValue { });
                //adminRoom.CoverImagePath = coverImage;

                await _adminRepository.InsertAsync(adminRoom);

                AdminRoomReturnDTO resultDto = _mapper.Map<AdminRoomReturnDTO>(adminRoom);
                resultDto.AdminCategoryValues = await GetCategoryValueDTOs(adminRoom);

                _logger.LogInformation("Successfully created a new AdminRoom with ID: {AdminRoomId}", adminRoom.Id);

                return resultDto;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex, "Concurrency conflict when creating a new AdminRoom.");
                throw new ApplicationException("Concurrency conflict occurred.", ex);
            }
            catch (DbUpdateException ex)
            {
                // Log database update exceptions
                _logger.LogError(ex, "Database update error occurred while creating a new AdminRoom.");
                throw new ApplicationException("A database error occurred while creating the AdminRoom.", ex);
            }
            catch (Exception ex)
            {
                // Log unexpected exceptions
                _logger.LogError(ex, "An unexpected error occurred while creating a new AdminRoom.");
                throw new ApplicationException("A database error occurred while creating the AdminRoom.", ex);
            }
        }

        public async Task DeleteAdminRoom(Guid id)
        {
            try
            {
                var adminRoom = await _adminRepository.GetByIdAsync(id);
                if (adminRoom == null)
                {
                    _logger.LogWarning("AdminRoom with ID: {AdminRoomId} not found", id);
                    return;
                }

                await _adminRepository.DeleteAsync(adminRoom);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex, "Concurrency conflict when deleting AdminRoom with ID: {AdminRoomId}", id);
                throw new ApplicationException("Concurrency conflict occurred.", ex);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database update error when deleting AdminRoom with ID: {AdminRoomId}", id);
                throw new ApplicationException("A database error occurred while deleting the contact.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred when deleting AdminRoom with ID: {AdminRoomId}", id);
                throw new ApplicationException("An unexpected error occurred.", ex);
            }
        }

        public async Task<IEnumerable<AdminRoomSummaryDTO>> GetAdminRoomSummary()
        {
            try
            {
                _logger.LogInformation("Fetching all AdminRoom Summaries.");
                var adminRooms = await _adminRepository.GetAllAsync();

                if (adminRooms == null || !adminRooms.Any())
                {
                    _logger.LogWarning("No AdminRoom Summaries found.");
                    throw new InvalidOperationException("No AdminRoom Summaries available.");
                }

                var adminRoomSummaryDTOs = adminRooms.Select(adminRoom => _mapper.Map<AdminRoomSummaryDTO>(adminRoom));
                return adminRoomSummaryDTOs;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching all AdminRoom Summaries.");
                throw; 
            }
        }

        public async Task<AdminRoomSummaryDTO?> GetAdminRoomSummaryById(Guid id)
        {
            try
            {
                _logger.LogInformation("Fetching AdminRoom Summary by ID: {AdminRoomId}", id);

                var adminRoom = await _adminRepository.GetByIdAsync(id);

                if (adminRoom == null)
                {
                    _logger.LogWarning("AdminRoom Summary with ID {AdminRoomId} not found", id);
                    return null;
                }

                var AdminRoomSummaryDTO = _mapper.Map<AdminRoomSummaryDTO>(adminRoom);
                return AdminRoomSummaryDTO;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching AdminRoom Summary by ID: {ContactId}", id);
                throw;
            }
        }

        private bool IsValidPrice(decimal Price)
        {
            if (Math.Round(Price,2) != Price)
            {
                return false;
            }
            return true;
        }

        private async Task<List<CategoryValueDTO>> GetCategoryValueDTOs(AdminRoom adminRoom) 
        {
            try
            {
                List<CategoryValueDTO> categoryValueDTOs = new List<CategoryValueDTO>();
                foreach (var item in adminRoom.CategoryValues)
                {
                    var result = await _adminRepository.MapAdminCategory(item.Id);
                    CategoryValueDTO categoryValueDTO = new CategoryValueDTO();
                    categoryValueDTO.CatergoryValueID = item.Id;
                    categoryValueDTO.AdminCategoryId = result!.Item1;
                    categoryValueDTO.AdminCategoryValue = result!.Item2;
                    categoryValueDTOs.Add(categoryValueDTO);
                }

                return categoryValueDTOs;
            }
            catch (Exception)
            {
                //handle here
                throw;
            }
           
        }

        
    }
}
