using HMS.Models;
using HMS.Models.Admin;
using HMS.Services.Repository_Service;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace HMS.Services.RepositoryService
{
    public class AdminRoomRepositoryService : RepositoryServiceBase<AdminRoom, AdminRoomRepositoryService>, IAdminRepositoryService
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
                    .Include(ar => ar.CategoryValues)
                        .ThenInclude(cv => cv.AdminCategoryValues) // Include AdminCategory inside CategoryValues
                          .ThenInclude(acv=> acv.AdminCategory)
                    .Include(ar => ar.ServiceAddons)                    
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
                var result =
                await DbContext.AdminRooms
                    .Include(ar => ar.CategoryValues)
                        .ThenInclude(cv => cv.AdminCategoryValues) // Include AdminCategory inside CategoryValues
                          .ThenInclude(acv => acv.AdminCategory)
                    .Include(ar => ar.ServiceAddons)
                    .FirstOrDefaultAsync(ar => ar.Id == id);
                return result;
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

        public AdminCategory MapAdminCategory(string key, string? value)
        {
            var category = DbContext.AdminCategories.FirstOrDefaultAsync(k => k.Title == key);         

            return category.Result != null ? category.Result : new AdminCategory();
        }

        public async Task<Tuple<int, int>> MapAdminCategory(Guid key)
        {
            try
            {
                var result = await DbContext.CategoryValues
                .Include(cv => cv.AdminCategoryValues)
                  .ThenInclude(acv => acv.AdminCategory)
                .FirstOrDefaultAsync(cv => cv.Id == key);

                Tuple<int, int> categoryValueResult = new Tuple<int, int>(result!.AdminCategoryValuesId, result!.AdminCategoryValues.AdminCategoryId);
                return categoryValueResult;
            }
            catch (Exception ex)
            {
                RepoLogger.LogError("Exception at MapAdminCategory: {0}", ex.Message);
                throw;
            }
            
             
        }

        public Dictionary<string, string> GetCategoryKeyValuePairs(int categoryId, int categoryValueId)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> CategoryValueExists(int categoryValueId)
        {
            try
            {
                return await DbContext.AdminCategoryValues.AnyAsync(e => e.Id == categoryValueId);
            }
            catch (Exception ex)
            {

                RepoLogger.LogError("Exception at CategoryValueExists: {0}", ex.Message);
                throw;
            }
        
        }
    }
}
