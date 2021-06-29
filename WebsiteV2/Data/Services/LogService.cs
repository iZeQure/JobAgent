using BlazorServerWebsite.Data.Services.Abstractions;
using ObjectLibrary.Logging;
using SqlDataAccessLibrary.Repositories.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BlazorServerWebsite.Data.Services
{
    public class LogService : BaseService<ILogRepository, Log>, ILogService
    {
        public LogService(ILogRepository logRepository) : base(logRepository)
        {

        }

        public override async Task<int> CreateAsync(Log createEntity, CancellationToken cancellation)
        {
            return await Repository.CreateAsync(createEntity, cancellation);
        }

        public override async Task<int> DeleteAsync(Log deleteEntity, CancellationToken cancellation)
        {
            return await Repository.DeleteAsync(deleteEntity, cancellation);
        }

        public override async Task<IEnumerable<Log>> GetAllAsync(CancellationToken cancellation)
        {
            return await Repository.GetAllAsync(cancellation);
        }

        public override async Task<Log> GetByIdAsync(int id, CancellationToken cancellation)
        {
            return await Repository.GetByIdAsync(id, cancellation);
        }

        public override async Task<int> UpdateAsync(Log updateEntity, CancellationToken cancellation)
        {
            return await Repository.UpdateAsync(updateEntity, cancellation);
        }
    }
}
