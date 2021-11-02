using JobAgentClassLibrary.Loggings.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobAgentClassLibrary.Loggings.Repositories
{
    public class LogRepository : ILoggingRepository
    {
        public Task<ILog> CreateAsync(ILog entity)
        {
            throw new NotImplementedException();
        }

        public Task<List<ILog>> GetAllAsync(ILog entity)
        {
            throw new NotImplementedException();
        }

        public Task<ILog> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveAsync(ILog entity)
        {
            throw new NotImplementedException();
        }

        public Task<ILog> UpdateAsync(ILog entity)
        {
            throw new NotImplementedException();
        }
    }
}
