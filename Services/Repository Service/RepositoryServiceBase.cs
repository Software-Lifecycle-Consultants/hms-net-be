using HMS.Models;
using Microsoft.EntityFrameworkCore;

namespace HMS.Services.Repository_Service
{
    public class RepositoryServiceBase<T> where T : class
    {
        private readonly HMSDBContext _dbContext;
        public RepositoryServiceBase(HMSDBContext context)
        {
            _dbContext = context;
        }

        public HMSDBContext DbContext { get { return _dbContext; } }

        
    }
}
