using HMS.Services.Repository_Service;
using HMS.Models;
using Microsoft.EntityFrameworkCore;

namespace HMS.Services.RepositoryService
{
    public class ImageRepositoryService: RepositoryServiceBase<Image, ImageRepositoryService>, IRepositoryService<Image>
    {
        public ImageRepositoryService(HMSDBContext context, ILogger<ImageRepositoryService> logger) : base(context, logger)
        {
        }

        public async Task DeleteAsync(Image dbObject)
        {
            try
            {
                DbContext.Images.Remove(dbObject);
                await SaveAsync();
            }
            catch (Exception ex)
            {
                RepoLogger.LogError("Exception at DeleteAsync: {0}", ex.Message);
            }
        }

        public async Task<IEnumerable<Image>> GetAllAsync()
        {
            try
            {
                return await DbContext.Images.ToListAsync();
            }
            catch (Exception ex)
            {
                RepoLogger.LogError("Exception at GetAllAsync: {0}", ex.Message);
                throw;
            }
        }

        public async Task<Image?> GetByIdAsync(Guid id)
        {
            try
            {
                return await DbContext.Images.FindAsync(id);
            }
            catch (Exception ex)
            {
                RepoLogger.LogError("Exception at GetByIdAsync: {0}", ex.Message);
                throw;
            }
        }

        public async Task InsertAsync(Image dbObject)
        {
            try
            {
                await DbContext.Images.AddAsync(dbObject);
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
