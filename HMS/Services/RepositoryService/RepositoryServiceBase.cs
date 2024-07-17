using HMS.Models;
using Microsoft.EntityFrameworkCore;

namespace HMS.Services.Repository_Service
{
    public class RepositoryServiceBase<T,TRepoService> where T : class
    {
        private readonly HMSDBContext _dbContext;
        private readonly ILogger<TRepoService> _logger;
       

        public RepositoryServiceBase(HMSDBContext context, ILogger<TRepoService> logger)
        {
            _dbContext = context;           
            _logger = logger;            
        }
             

        public HMSDBContext DbContext { get { return _dbContext; } }

        public ILogger<TRepoService> RepoLogger { get { return _logger; } }

        //ensure that all derived repository classes inherit following common methods,adhere to DRY principle
        public async Task SaveAsync()
        {
            try
            {
                await DbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                RepoLogger.LogError("Exception at SaveAsync: {0}", ex.Message);
                throw;
            }
        }

        public void Update(T dbObject)
        {
            try
            {
                DbContext.Entry(dbObject).State = EntityState.Modified;
            }
            catch (Exception ex)
            {

                RepoLogger.LogError("Exception at Update: {0}", ex.Message);
                throw;
            }
        }

    }
}
