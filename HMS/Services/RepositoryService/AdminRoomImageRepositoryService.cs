using HMS.Services.Repository_Service;
using HMS.Models;
using HMS.Models.Admin;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HMS.Services.RepositoryService
{
    public class AdminRoomImageRepositoryService : RepositoryServiceBase<AdminRoomImage, AdminRoomImageRepositoryService>, IRepositoryService<AdminRoomImage> /*IBulkRepositoryService<AdminRoomImage>*/
    {
        public AdminRoomImageRepositoryService(HMSDBContext context, ILogger<AdminRoomImageRepositoryService> logger) : base(context, logger)
        {
        }

        public async Task DeleteAsync(AdminRoomImage dbObject)
        {
            try
            {
                DbContext.AdminRoomImages.Remove(dbObject);
                await SaveAsync();
            }
            catch (Exception ex)
            {
                RepoLogger.LogError("Exception at DeleteAsync: {0}", ex.Message);
            }
        }

        public async Task<IEnumerable<AdminRoomImage>> GetAllAsync()
        {
            try
            {
                return await DbContext.AdminRoomImages.ToListAsync();
            }
            catch (Exception ex)
            {
                RepoLogger.LogError("Exception at GetAllAsync: {0}", ex.Message);
                throw;
            }
        }

        public async Task<AdminRoomImage?> GetByIdAsync(Guid id)
        {
            try
            {
                return await DbContext.AdminRoomImages.FindAsync(id);
            }
            catch (Exception ex)
            {
                RepoLogger.LogError("Exception at GetByIdAsync: {0}", ex.Message);
                throw;
            }
        }

        public async Task InsertAsync(AdminRoomImage dbObject)
        {
            try
            {
                await DbContext.AdminRoomImages.AddAsync(dbObject);
                await SaveAsync();
            }
            catch (Exception ex)
            {
                RepoLogger.LogError("Exception at InsertAsync: {0}", ex.Message);
                throw;
            }
        }

        public async Task<bool> ItemExistsAsync(Guid id)
        {
            try
            {
                return await DbContext.AdminRoomImages.AnyAsync(e => e.Id == id);
            }
            catch (Exception ex)
            {
                RepoLogger.LogError("Exception at ItemExistsAsync: {0}", ex.Message);
                throw;
            }
        }
    }
}
