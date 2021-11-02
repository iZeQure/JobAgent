using JobAgentClassLibrary.Common.Areas.Entities;
using JobAgentClassLibrary.Core.Database.Managers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JobAgentClassLibrary.Common.Areas.Repositories
{
    public class AreaRepository : IAreaRepository
    {
        private readonly ISqlDbManager _sqlDbManager;

        public AreaRepository(ISqlDbManager sqlDbManager)
        {
            _sqlDbManager = sqlDbManager;
        }

        public Task<IArea> CreateAsync(IArea entity)
        {
            throw new NotImplementedException();
        }

        public Task<List<IArea>> GetAllAsync(IArea entity)
        {
            throw new NotImplementedException();
        }

        public Task<IArea> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveAsync(IArea entity)
        {
            throw new NotImplementedException();
        }

        public Task<IArea> UpdateAsync(IArea entity)
        {
            throw new NotImplementedException();
        }
    }
}
