using AutoMapper;
using HMS.Controllers.Admin;
using HMS.DTOs.Admin;
using HMS.Models.Admin;
using HMS.Services.RepositoryService;
using Microsoft.AspNetCore.Mvc;

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
