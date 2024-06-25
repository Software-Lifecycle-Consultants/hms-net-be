using HMS.Models;
using HMS.Models.Admin;
using HMS.Services.Repository_Service;
using Microsoft.EntityFrameworkCore;

namespace HMS.Services.RepositoryService
{
    public class AdminContactRepositoryService : RepositoryServiceBase<AdminContact,AdminContactRepositoryService>, IRepositoryService<AdminContact>
    {
        public AdminContactRepositoryService(HMSDBContext context, ILogger<AdminContactRepositoryService> logger) : base(context, logger)
        {
        }

        public async Task DeleteAsync(AdminContact dbObject)
        {
            try
            {
                DbContext.AdminContacts.Remove(dbObject);
                await SaveAsync();
            }
            catch (Exception ex)
            {
                RepoLogger.LogError("Exception at DeleteAsync: {0}", ex.Message);
            }
        }

        public async Task<IEnumerable<AdminContact>> GetAllAsync()
        {
            try
            {
                return await DbContext.AdminContacts.ToListAsync();
            }
            catch(Exception ex) 
            {
                RepoLogger.LogError("Exception at GetAllAsync: {0}", ex.Message);
                throw;
            }
        }

        public async Task<AdminContact?> GetByIdAsync(Guid id)
        {
            try
            {
                return await DbContext.AdminContacts
                    .FirstOrDefaultAsync(ar => ar.Id == id);
            }
            catch(Exception ex) 
            {
                RepoLogger.LogError("Exception at GetByIdAsync: {0}", ex.Message);
                throw;
            }
        }

        public async Task InsertAsync(AdminContact dbObject)
        {
            try
            {
                await DbContext.AdminContacts.AddAsync(dbObject);
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
                return await DbContext.AdminContacts.AnyAsync(e => e.Id == id);
            }
            catch (Exception ex)
            {
                RepoLogger.LogError("Exception at ItemExistsAsync: {0}", ex.Message);
                throw;
            }
        }
    }
}
