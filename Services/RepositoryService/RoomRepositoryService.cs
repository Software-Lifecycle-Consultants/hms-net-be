using HMS.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace HMS.Services.Repository_Service
{
    public class RoomRepositoryService : RepositoryServiceBase<Room, RoomRepositoryService>, IRepositoryService<Room>
    {
        public RoomRepositoryService(HMSDBContext context, ILogger<RoomRepositoryService> logger) : base(context,logger)
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

        public async Task<Room?> GetByIdAsync(Guid id)
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

        public async Task<bool> ItemExistsAsync(Guid id)
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

       

    }
}
