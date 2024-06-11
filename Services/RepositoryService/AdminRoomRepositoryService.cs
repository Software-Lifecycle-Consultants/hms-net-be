using HMS.Models;
using HMS.Models.Admin;
using HMS.Services.Repository_Service;
using Microsoft.EntityFrameworkCore;

namespace HMS.Services.RepositoryService
{
    public class AdminRoomRepositoryService : RepositoryServiceBase<AdminRoom, AdminRoomRepositoryService>, IRepositoryService<AdminRoom>
    {
        public AdminRoomRepositoryService(HMSDBContext context, ILogger<AdminRoomRepositoryService> logger) : base(context, logger)
        {
        }

        public async Task DeleteAsync(AdminRoom dbObject)
        {
            try
            {
                DbContext.AdminRooms.Remove(dbObject);
                await SaveAsync();
            }
            catch (Exception ex)
            {

                RepoLogger.LogError("Exception at DeleteAsync: {0}", ex.Message);

            }
        }

        public async Task<IEnumerable<AdminRoom>> GetAllAsync()
        {
            try
            {
                return await DbContext.AdminRooms
                    .Include(ar => ar.Categories)
                    .Include(ar => ar.ServiceAddons)
                    .Include(ar => ar.AdditionalInfo)
                    .ToListAsync();
            }
            catch (Exception ex)
            {

                RepoLogger.LogError("Exception at GetAllAsync: {0}", ex.Message);
                throw;
            }
        }

        public async Task<AdminRoom?> GetByIdAsync(Guid id)
        {
            try
            {
                return await DbContext.AdminRooms
                    .Include(ar => ar.Categories)
                    .Include(ar => ar.ServiceAddons)
                    .Include(ar => ar.AdditionalInfo)
                    .FirstOrDefaultAsync(ar => ar.Id == id);
            }
            catch (Exception ex)
            {
                RepoLogger.LogError("Exception at GetByIdAsync: {0}", ex.Message);
                throw;
            }
        }

        public async Task InsertAsync(AdminRoom dbObject)
        {
            try
            {
                await DbContext.AdminRooms.AddAsync(dbObject);
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
                return await DbContext.AdminRooms.AnyAsync(e => e.Id == id);
            }
            catch (Exception ex)
            {

                RepoLogger.LogError("Exception at ItemExistsAsync: {0}", ex.Message);
                throw;
            }
        }

    }
}
