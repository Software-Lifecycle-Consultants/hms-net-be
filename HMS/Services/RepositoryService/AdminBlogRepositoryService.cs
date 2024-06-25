using HMS.Models;
using HMS.Models.Admin;
using HMS.Services.Repository_Service;
using Microsoft.EntityFrameworkCore;

namespace HMS.Services.RepositoryService
{
    public class AdminBlogRepositoryService : RepositoryServiceBase<AdminBlog, AdminBlogRepositoryService>, IRepositoryService<AdminBlog>
    {
        public AdminBlogRepositoryService(HMSDBContext context, ILogger<AdminBlogRepositoryService> logger) : base(context, logger)
        {
        }

        public async Task DeleteAsync(AdminBlog dbObject)
        {
            try
            {
                DbContext.AdminBlogs.Remove(dbObject);
                await SaveAsync();
            }

            catch (Exception ex)
            {
                RepoLogger.LogError("Exception at DeleteAsync: {0}", ex.Message);
            }
        }

        public async Task<IEnumerable<AdminBlog>> GetAllAsync()
        {
            try
            {
                return await DbContext.AdminBlogs.ToListAsync();
            }
            catch (Exception ex)
            {

                RepoLogger.LogError("Exception at GetAllAsync: {0}", ex.Message);
                throw;
            }
        }

        public async Task<AdminBlog?> GetByIdAsync(Guid id)
        {
            try
            {
                return await DbContext.AdminBlogs.FindAsync(id);
            }
            catch (Exception ex)
            {
                RepoLogger.LogError("Exception at GetByIdAsync: {0}", ex.Message);
                throw;
            }
        }

        public async Task InsertAsync(AdminBlog dbObject)
        {
            try
            {
                await DbContext.AdminBlogs.AddAsync(dbObject);
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
                return await DbContext.AdminBlogs.AnyAsync(e => e.Id == id);
            }
            catch (Exception ex)
            {

                RepoLogger.LogError("Exception at ItemExistsAsync: {0}", ex.Message);
                throw;
            }
        }
    }
}
