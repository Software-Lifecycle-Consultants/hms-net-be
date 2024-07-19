using HMS.Models;
using HMS.Models.Admin;
using HMS.Services.Repository_Service;
using Microsoft.EntityFrameworkCore;

namespace HMS.Services.RepositoryService
{
    public class AdminFAQRepositoryService : RepositoryServiceBase<AdminFAQ, AdminFAQRepositoryService>, IRepositoryService<AdminFAQ> 
    {
        public AdminFAQRepositoryService(HMSDBContext context, ILogger<AdminFAQRepositoryService>logger) : base(context, logger)
        {
        }
        public async Task DeleteAsync(AdminFAQ dbObject)
        {
            try
            {
                DbContext.AdminFAQs.Remove(dbObject);
                await SaveAsync();
            }
            catch (Exception ex)
            {
                RepoLogger.LogError("Exception at DeleteAsync: {0}", ex.Message);
            }
        }
        public async Task<IEnumerable<AdminFAQ>> GetAllAsync()
        {
            try
            {
                return await DbContext.AdminFAQs.ToListAsync();
            }
            catch (Exception ex)
            {
                RepoLogger.LogError("Exception at GetAllAsync: {0}", ex.Message);
                throw;
            }
        }

        public async Task<AdminFAQ?> GetByIdAsync(Guid id)
        {
            try
            {
                return await DbContext.AdminFAQs
                    .FirstOrDefaultAsync(ar => ar.Id == id);
            }
            catch (Exception ex)
            {
                RepoLogger.LogError("Exception at GetByIdAsync: {0}", ex.Message);
                throw;
            }
        }

        public async Task InsertAsync(AdminFAQ dbObject)
        {
            try
            {
                await DbContext.AdminFAQs.AddAsync(dbObject);
                await SaveAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> ItemExistsAsync(Guid id)
        {
            try
            {
                return await DbContext.AdminFAQs.AnyAsync(e => e.Id == id);
            }
            catch (Exception ex)
            {
                RepoLogger.LogError("Exception at ItemExistsAsync: {0}", ex.Message);
                throw;
            }
        }
    }
}
