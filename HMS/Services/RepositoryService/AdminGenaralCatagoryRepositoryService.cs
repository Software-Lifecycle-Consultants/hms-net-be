using HMS.Models;
using HMS.Models.Admin;
using HMS.Services.Repository_Service;
using Microsoft.EntityFrameworkCore;

namespace HMS.Services.RepositoryService
{
    public class AdminGenaralCatagoryRepositoryService : RepositoryServiceBase<AdminGenaralCatagory, AdminGenaralCatagoryRepositoryService>
    {
        public AdminGenaralCatagoryRepositoryService(HMSDBContext context, ILogger<AdminGenaralCatagoryRepositoryService>logger) : base(context, logger) 
        { 
        }
        public async Task DeleteAsync(AdminGenaralCatagory dbObject)
        {
            try
            {
                DbContext.AdminGenaralCatagories.Remove(dbObject);
                await SaveAsync();
            }
            catch (Exception ex)
            {
                RepoLogger.LogError("Exception at DeleteAsync: {0}", ex.Message);    
            }
        }
        public async Task<IEnumerable<AdminGenaralCatagory>> GetAllAsync()
        {
            try
            {
                return await DbContext.AdminGenaralCatagories
                    .Include(agc => agc.AdminCategories)
                    .ThenInclude(ac => ac.AdminCategoryValues)
                    .ToListAsync();
  
            }
            catch (Exception ex)
            {
                RepoLogger.LogError("Exception at GetAllAsync: {0}", ex.Message);
                throw;
            }
        }
        public async Task<AdminGenaralCatagory?> GetByIdAsync(int id)
        {
            try
            {
               var result = await DbContext.AdminGenaralCatagories
                    .Include(agc => agc.AdminCategories)
                    .ThenInclude(ac => ac.AdminCategoryValues)
                    .FirstOrDefaultAsync(agc => agc.Id == id);
                return result;
            }
            catch (Exception ex)
            {
                RepoLogger.LogError("Exception at GetByIdAsync: {0}", ex.Message);
                throw;
            }
        }

        public async Task InsertAsync(AdminGenaralCatagory dbObject)
        {
            try
            {
                await DbContext.AdminGenaralCatagories.AddAsync(dbObject);
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
                return await DbContext.AdminGenaralCatagories.AnyAsync(e => e.Id == id);
            }
            catch (Exception ex)
            {
                RepoLogger.LogError("Exception at ItemExistsAsync: {0}", ex.Message);
                throw;
            }
        }
    }
}
