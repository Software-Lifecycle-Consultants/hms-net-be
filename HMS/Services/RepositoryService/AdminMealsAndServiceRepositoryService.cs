using HMS.Models.Admin;
using HMS.Models;
using HMS.Services.Repository_Service;
using Microsoft.EntityFrameworkCore;

namespace HMS.Services.RepositoryService
{
    public class AdminMealsAndServiceRepositoryService : RepositoryServiceBase<AdminMealsAndServices, AdminMealsAndServiceRepositoryService>, IAdminMASRepositoryService
    {
        public AdminMealsAndServiceRepositoryService(HMSDBContext context, ILogger<AdminMealsAndServiceRepositoryService> logger) : base(context, logger)
        {
        }

        public async Task DeleteAsync(AdminMealsAndServices dbObject)
        {
            try
            {
                DbContext.AdminMealsAndServices.Remove(dbObject);
                await SaveAsync();
            }
            catch (Exception ex)
            {
                RepoLogger.LogError("Exception at DeleteAsync: {0}", ex.Message);
            }
        }

        public async Task<IEnumerable<AdminMealsAndServices>> GetAllAsync()
        {
            try
            {
                return await DbContext.AdminMealsAndServices.ToListAsync();
            }
            catch (Exception ex)
            {
                RepoLogger.LogError("Exception at GetAllAsync: {0}", ex.Message);
                throw;
            }
        }

        public async Task<AdminMealsAndServices?  > GetByIdAsync(int id)
        {
            try
            {
                return await DbContext.AdminMealsAndServices.FirstOrDefaultAsync(ar => ar.Id == id);
            }
            catch (Exception ex)
            {
                RepoLogger.LogError("Exception at GetByIdAsync: {0}", ex.Message);
                throw;
            }
        }
        public async Task InsertAsync(AdminMealsAndServices dbObject)
        {
            try
            {
                await DbContext.AdminMealsAndServices.AddAsync(dbObject);
                await SaveAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> ItemExistsAsync(int id)
        {
            try
            {
                return await DbContext.AdminMealsAndServices.AnyAsync(e => e.Id == id);
            }
            catch (Exception ex)
            {
                RepoLogger.LogError("Exception at ItemExistsAsync: {0}", ex.Message);
                throw;
            }
        }

        Task<AdminMealsAndServices?> IRepositoryService<AdminMealsAndServices>.GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        Task<bool> IRepositoryService<AdminMealsAndServices>.ItemExistsAsync(Guid id)
        {
            throw new NotImplementedException();
        }

    }
}
