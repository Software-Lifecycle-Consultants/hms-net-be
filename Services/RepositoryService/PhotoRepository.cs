using HMS.Services.Repository_Service;
using HMS.Models;
using Microsoft.EntityFrameworkCore;

namespace HMS.Services.RepositoryService
{
    public class PhotoRepository: RepositoryServiceBase<Photo, PhotoRepository>, IRepositoryService<Photo>
    {
        public PhotoRepository(HMSDBContext context, ILogger<PhotoRepository> logger) : base(context, logger)
        {
        }

        public async Task DeleteAsync(Photo dbObject)
        {
            try
            {
                DbContext.Photos.Remove(dbObject);
                await SaveAsync();
            }
            catch (Exception ex)
            {
                RepoLogger.LogError("Exception at DeleteAsync: {0}", ex.Message);
            }
        }

        public async Task<IEnumerable<Photo>> GetAllAsync()
        {
            try
            {
                return await DbContext.Photos.ToListAsync();
            }
            catch (Exception ex)
            {
                RepoLogger.LogError("Exception at GetAllAsync: {0}", ex.Message);
                throw;
            }
        }

        public async Task<Photo?> GetByIdAsync(Guid id)
        {
            try
            {
                return await DbContext.Photos.FindAsync(id);
            }
            catch (Exception ex)
            {
                RepoLogger.LogError("Exception at GetByIdAsync: {0}", ex.Message);
                throw;
            }
        }

        public async Task InsertAsync(Photo dbObject)
        {
            try
            {
                await DbContext.Photos.AddAsync(dbObject);
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
                return await DbContext.Rooms.AnyAsync(e => e.Id == id);
            }
            catch (Exception)
            {

                throw;
            }
        }


    }
}
