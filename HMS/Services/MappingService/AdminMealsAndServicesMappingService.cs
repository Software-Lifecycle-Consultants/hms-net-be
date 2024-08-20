using AutoMapper;
using HMS.DTOs.Admin;
using HMS.Models.Admin;
using HMS.Services.Repository_Service;
using HMS.Services.RepositoryService;
using Microsoft.AspNetCore.Mvc;

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

        public async Task<ActionResult<IEnumerable<AdminMealsAndServicesReturnDTO>>> GetAdminMealsAndServices()
        {
            try
            {
                _logger.LogInformation("Fetching all AdminMealsAndServicesValues.");
                var adminMealsAndServices = await _adminMASRepositoryService.GetAllAsync();
                if (adminMealsAndServices == null || !adminMealsAndServices.Any())
                {
                    _logger.LogInformation("No AdminMealsAndServicesValues found.");
                    return NotFound("No AdminMealsAndServicesValues available.");
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
                return StatusCode(500, "An error occurred while retrieving AdminMealsAndServicesValues.");
            }
        }

        private async Task<AdminMealsAndServicesValuesDTO?> GetAdminMealsAndServicesValuesDTOs(AdminMealsAndServices adminMealAndService)
        {
            throw new NotImplementedException();
        }

        private ActionResult<IEnumerable<AdminMealsAndServicesReturnDTO>> StatusCode(int v1, string v2)
        {
            throw new NotImplementedException();
        }

        private ActionResult<IEnumerable<AdminMealsAndServicesReturnDTO>> NotFound(string v)
        {
            throw new NotImplementedException();
        }
    }
}
