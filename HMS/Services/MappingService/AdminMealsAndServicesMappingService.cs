using AutoMapper;
using HMS.DTOs.Admin;
using HMS.Models.Admin;
using HMS.Services.Repository_Service;
using HMS.Services.RepositoryService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HMS.Services.MappingService
{
    public class AdminMealsAndServicesMappingService
    {
        IAdminMASRepositoryService _adminMASRepositoryService;
        ILogger<AdminMealsAndServicesMappingService> _logger;
        IMapper _mapper;
        public AdminMealsAndServicesMappingService(IAdminMASRepositoryService adminMASRepositoryService, IMapper mapper, ILogger<AdminMealsAndServicesMappingService> logger)
        {
            _adminMASRepositoryService = adminMASRepositoryService;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ActionResult<IEnumerable<AdminMealsAndServicesReturnDTO>>?> GetAdminMealsAndServicesAsync()
        {
            try
            {
                _logger.LogInformation("Fetching all AdminMealsAndServicesValues.");
                var adminMealsAndServices = await _adminMASRepositoryService.GetAllAsync();
                if (adminMealsAndServices == null || !adminMealsAndServices.Any())
                {
                    _logger.LogInformation("No AdminMealsAndServicesValues found.");
                    return null;
                }
                var adminMealsAndServicesReturnDTOs = new List<AdminMealsAndServicesReturnDTO>();

                foreach (var adminMealAndService in adminMealsAndServices)
                {
                    var adminMealAndServiceDto = _mapper.Map<AdminMealsAndServicesReturnDTO>(adminMealAndService);
                    adminMealAndServiceDto.AdminMealsAndServicesValues = await GetAdminMealsAndServicesValuesDTOs(adminMealAndService);
                    adminMealsAndServicesReturnDTOs.Add(adminMealAndServiceDto);
                }

                return adminMealsAndServicesReturnDTOs;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching all AdminMealsAndServicesValues.");
                throw;
            }
        }

        public async Task<AdminMealsAndServicesReturnDTO?> GetAdminMealsAndServicesByIdAsync(int id)
        {
            try
            {
                var adminMealsAndServices = await _adminMASRepositoryService.GetByIdAsync(id);

                if (adminMealsAndServices == null)
                {
                    _logger.LogWarning("AdminMealsAndServices with ID {AdminMealsAndServicesId} not found", id);
                    return null;
                }

                var adminMealsAndServicesDTO = _mapper.Map<AdminMealsAndServicesReturnDTO>(adminMealsAndServices);
                adminMealsAndServicesDTO.AdminMealsAndServicesValues = await GetAdminMealsAndServicesValuesDTOs(adminMealsAndServices);

                return adminMealsAndServicesDTO;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching AdminMealsAndServices by ID: {AdminMealsAndServicesId}", id);
                throw;
            }
        }


        public async Task<bool> PutAdminMealsAndServicesAsync(int id, AdminMealsAndServicesDTO adminMealsAndServicesDTO)
        {
            try
            {
                var existingAdminMealsAndServices = await _adminMASRepositoryService.GetByIdAsync(id);

                if (existingAdminMealsAndServices == null)
                {
                    _logger.LogWarning("AdminMealsAndServices with ID {AdminMealsAndServicesId} not found for update.", id);
                    return false;
                }

                _mapper.Map(adminMealsAndServicesDTO, existingAdminMealsAndServices);
                existingAdminMealsAndServices.Id = id; // Explicitly set the Id just to assert control over it.

                _adminMASRepositoryService.Update(existingAdminMealsAndServices);
                await _adminMASRepositoryService.SaveAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating AdminMealsAndServices with ID: {AdminMealsAndServicesId}", id);
                throw;
            }
        }

        public async Task<AdminMealsAndServicesReturnDTO> PostAdminMealsAndServicesAsync(AdminMealsAndServicesDTO adminMealsAndServicesDTO)
        {
            try
            {
                var adminMealsAndServices = _mapper.Map<AdminMealsAndServices>(adminMealsAndServicesDTO);

                List<AdminMealsAndServicesValue> mealsAndServicesValues = new List<AdminMealsAndServicesValue>();
                foreach (var item in adminMealsAndServicesDTO.AdminMealsAndServicesValues)
                {
                    AdminMealsAndServicesValue adminMealsAndServicesValue = _mapper.Map<AdminMealsAndServicesValue>(item);
                    mealsAndServicesValues.Add(adminMealsAndServicesValue);
                }

                adminMealsAndServices.AdminMealsAndServicesValue = mealsAndServicesValues;

                await _adminMASRepositoryService.InsertAsync(adminMealsAndServices);

                var resultDto = _mapper.Map<AdminMealsAndServicesReturnDTO>(adminMealsAndServices);
                return resultDto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a new AdminMealsAndServices.");
                throw;
            }
        }

        public async Task<bool> DeleteAdminMealsAndServicesAsync(int id)
        {
            try
            {
                var adminMealsAndServices = await _adminMASRepositoryService.GetByIdAsync(id);

                if (adminMealsAndServices == null)
                {
                    _logger.LogWarning("AdminMealsAndServices with ID {AdminMealsAndServicesId} not found", id);
                    return false;
                }

                await _adminMASRepositoryService.DeleteAsync(adminMealsAndServices);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting AdminMealsAndServices with ID: {AdminMealsAndServicesId}", id);
                throw;
            }
        }

        private async Task<List<AdminMealsAndServicesValuesDTO>> GetAdminMealsAndServicesValuesDTOs(AdminMealsAndServices adminMealsAndServices)
        {
            try
            {
                var mealsAndServicesValues = adminMealsAndServices.AdminMealsAndServicesValue;
                var mealsAndServicesValuesDTOs = _mapper.Map<List<AdminMealsAndServicesValuesDTO>>(mealsAndServicesValues);
                return await Task.FromResult(mealsAndServicesValuesDTOs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching AdminMealsAndServices values.");
                throw;
            }
        }

    }
}
