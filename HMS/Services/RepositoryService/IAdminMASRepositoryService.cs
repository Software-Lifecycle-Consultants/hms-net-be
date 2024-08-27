using HMS.Models.Admin;

namespace HMS.Services.Repository_Service
{
    public interface IAdminMASRepositoryService : IRepositoryService<AdminMealsAndServices>
    {
        Task<AdminMealsAndServices?> GetByIdAsync(int id);
        Task<bool> ItemExistsAsync(int id);
    }
}