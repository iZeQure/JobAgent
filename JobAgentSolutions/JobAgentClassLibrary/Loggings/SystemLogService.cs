using JobAgentClassLibrary.Core.Entities;
using JobAgentClassLibrary.Loggings.Entities;
using JobAgentClassLibrary.Loggings.Factory;
using JobAgentClassLibrary.Loggings.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JobAgentClassLibrary.Loggings
{
    public class SystemLogService : ILogService
    {
        private readonly ILoggingRepository _repository;
        private readonly LogEntityFactory _logEntityFactory;

        public SystemLogService(ILoggingRepository repository, LogEntityFactory logEntityFactory)
        {
            _repository = repository;
            _logEntityFactory = logEntityFactory;
        }

        public async Task<ILog> CreateAsync(ILog entity)
        {
            return await _repository.CreateAsync(entity);
        }

        public async Task<List<ILog>> GetAllCrawlerLogsAsync()
        {
            return await _repository.GetAllCrawlerLogsAsync();
        }

        public async Task<List<ILog>> GetAllSystemLogsAsync()
        {
            return await _repository.GetAllSystemLogsAsync();
        }

        public async Task<ILog> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<ILog> LogError(Exception exception, string message, string action, string createdBy, LogType logType)
        {
            string msg = $"{message} : {exception.Message}";
            ILog log = (ILog)_logEntityFactory.CreateEntity("SystemLog", 0, LogSeverity.ERROR, msg, action, createdBy, DateTime.Now, logType);
            return await _repository.CreateAsync(log);
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
