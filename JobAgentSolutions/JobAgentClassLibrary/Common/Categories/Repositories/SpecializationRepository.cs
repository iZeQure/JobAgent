using JobAgentClassLibrary.Common.Categories.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JobAgentClassLibrary.Common.Categories.Repositories
{
    public class SpecializationRepository : ISpecializationRepository
    {
        public Task<ISpecialization> CreateAsync(ISpecialization entity)
        {
            throw new NotImplementedException();
        }

        public Task<List<ISpecialization>> GetAllAsync(ISpecialization entity)
        {
            throw new NotImplementedException();
        }

        public Task<ISpecialization> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveAsync(ISpecialization entity)
        {
            throw new NotImplementedException();
        }

        public Task<ISpecialization> UpdateAsync(ISpecialization entity)
        {
            throw new NotImplementedException();
        }
    }
}
