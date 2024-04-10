using HMS.Models;
using Microsoft.EntityFrameworkCore;

namespace HMS.Services.Repository_Service
{
    public class ContactsRepositoryService: RepositoryServiceBase<Contact>, IRepositoryService<Contact>
    {
        public ContactsRepositoryService(HMSDBContext context): base(context)
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

                throw;
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

                throw;
            }
        }

        public async Task<Contact?> GetByIdAsync(long id)
        {
            try
            {
              return await DbContext.Contacts.FindAsync(id);
            }
            catch (Exception ex)
            {

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
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> ItemExistsAsync(long id)
        {
            try
            {
               return await DbContext.Contacts.AnyAsync(e => e.Id == id);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task  SaveAsync()
        {
            try
            {
                await DbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public void Update(Contact dbObject)
        {
            try
            {
                DbContext.Entry(dbObject).State = EntityState.Modified;
            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}
