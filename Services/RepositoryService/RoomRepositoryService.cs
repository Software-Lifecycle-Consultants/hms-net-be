using HMS.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace HMS.Services.Repository_Service
{
    public class RoomRepositoryService : RepositoryServiceBase<Room>, IRepositoryService<Room>
    {
        public RoomRepositoryService(HMSDBContext context) : base(context)
        {
        }

        public async Task DeleteAsync(Room dbObject)
        {
            try
            {

                DbContext.Rooms.Remove(dbObject);
                await SaveAsync();
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<IEnumerable<Room>> GetAllAsync()
        {
            try
            {
              return await DbContext.Rooms.ToListAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<Room?> GetByIdAsync(long id)
        {
            try
            {
                return await DbContext.Rooms.FindAsync(id);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task InsertAsync(Room dbObject)
        {
            try
            {
                await DbContext.Rooms.AddAsync(dbObject);
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
                return await DbContext.Rooms.AnyAsync(e=> e.Id == id);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task SaveAsync()
        {
            try
            {
                await DbContext.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void Update(Room dbObject)
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
