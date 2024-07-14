using HMS.Models.Admin;
using HMS.Services.Repository_Service;

namespace HMS.Services.RepositoryService
{
    public interface IAdminRepositoryService  : IRepositoryService<AdminRoom>
    {
        AdminCategory MapAdminCategory(string key, string? value);
        Task<Tuple<int,int>> MapAdminCategory(Guid id);
        Dictionary<string, string> GetCategoryKeyValuePairs(int categoryId, int categoryValueId);      
        Task<bool> CategoryValueExists(int categoryValueId);
    }
}
