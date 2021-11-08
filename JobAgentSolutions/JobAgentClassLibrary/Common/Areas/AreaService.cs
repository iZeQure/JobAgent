using JobAgentClassLibrary.Common.Areas.Entities;
using JobAgentClassLibrary.Common.Areas.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JobAgentClassLibrary.Common.Areas
{
    public class AreaService : IAreaService
    {
        private readonly IAreaRepository _repository;

        public AreaService(IAreaRepository repository)
        {
            _repository = repository;
        }

        public async Task<IArea> CreateAsync(IArea entity)
        {
            return await _repository.CreateAsync(entity);
        }

        public async Task<List<IArea>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<IArea> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<bool> RemoveAsync(IArea entity)
        {
            return await _repository.RemoveAsync(entity);
        }

        public async Task<IArea> UpdateAsync(IArea entity)
        {
            return await _repository.UpdateAsync(entity);
        }
    }
}
