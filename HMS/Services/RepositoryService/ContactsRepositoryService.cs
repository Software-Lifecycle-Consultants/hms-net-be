using HMS.Models;
using Microsoft.EntityFrameworkCore;

namespace HMS.Services.Repository_Service
{
    public class ContactsRepositoryService : RepositoryServiceBase<Contact, ContactsRepositoryService>, IRepositoryService<Contact>
    {
        public ContactsRepositoryService(HMSDBContext context, ILogger<ContactsRepositoryService> logger) : base(context, logger)
        {

        }
        public async Task DeleteAsync(Contact dbObject)
        {
            try
            {
                DbContext.Contacts.Remove(dbObject);
                await SaveAsync();
            }
            catch (Exception ex)
            {
                RepoLogger.LogError("Exception at DeleteAsync: {0}", ex.Message);

            }
        }

        public async Task<IEnumerable<Contact>> GetAllAsync()
        {
            try
            {
                return await DbContext.Contacts.ToListAsync();
            }
            catch (Exception ex)
            {
                RepoLogger.LogError("Exception at GetAllAsync: {0}", ex.Message);
                throw;
            }
        }

        public async Task<Contact?> GetByIdAsync(Guid id)
        {
            try
            {
                return await DbContext.Contacts.FindAsync(id);
            }
            catch (Exception ex)
            {
                RepoLogger.LogError("Exception at GetByIdAsync: {0}", ex.Message);
                throw;
            }
        }

        public async Task InsertAsync(Contact dbObject)
        {
            try
            {
                await DbContext.Contacts.AddAsync(dbObject);
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
                return await DbContext.Contacts.AnyAsync(e => e.Id == id);
            }
            catch (Exception ex)
            {

                RepoLogger.LogError("Exception at ItemExistsAsync: {0}", ex.Message);
                throw;
            }
        }


    }
}
