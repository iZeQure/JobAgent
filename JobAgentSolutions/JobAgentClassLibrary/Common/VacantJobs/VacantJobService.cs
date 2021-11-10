using JobAgentClassLibrary.Common.VacantJobs.Entities;
using JobAgentClassLibrary.Common.VacantJobs.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobAgentClassLibrary.Common.VacantJobs
{
    public class VacantJobService
    {
        private readonly IVacantJobRepository _repository;

        public VacantJobService(IVacantJobRepository repository)
        {
            _repository = repository;
        }

        public async Task<IVacantJob> CreateAsync(IVacantJob entity)
        {
            return await _repository.CreateAsync(entity);
        }

        public async Task<List<IVacantJob>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<IVacantJob> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<bool> RemoveAsync(IVacantJob entity)
        {
            return await _repository.RemoveAsync(entity);
        }

        public async Task<IVacantJob> UpdateAsync(IVacantJob entity)
        {
            return await _repository.UpdateAsync(entity);
        }


    }
}
