using HMS.Models.Admin;
using HMS.Services.Repository_Service;

namespace HMS.Services.RepositoryService
{
    public interface IAdminRepositoryService  : IRepositoryService<AdminRoom>
    {
        AdminCategory MapAdminCategory(string key, string? value);
    }
}
