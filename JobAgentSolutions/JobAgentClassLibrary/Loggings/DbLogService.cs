using JobAgentClassLibrary.Loggings.Entities;
using JobAgentClassLibrary.Loggings.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JobAgentClassLibrary.Loggings
{
    public class DbLogService : ILogService
    {
        private readonly ILoggingRepository _repository;

        public DbLogService(ILoggingRepository repository)
        {
            _repository = repository;
        }

        public async Task<ILog> CreateAsync(ILog entity)
        {
            return await _repository.CreateAsync(entity);
        }

        public async Task<List<ILog>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<ILog> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<bool> RemoveAsync(ILog entity)
        {
            return await _repository.RemoveAsync(entity);
        }

        public async Task<ILog> UpdateAsync(ILog entity)
        {
            return await _repository.UpdateAsync(entity);
        }
    }
}
