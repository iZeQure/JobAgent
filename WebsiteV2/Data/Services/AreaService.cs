using BlazorServerWebsite.Data.Services.Abstractions;
using ObjectLibrary.Common;
using SqlDataAccessLibrary.Repositories.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BlazorServerWebsite.Data.Services
{
    public class AreaService : BaseService<IAreaRepository, Area>, IAreaService
    {
        public AreaService(IAreaRepository areaRepository) : base(areaRepository)
        {

        }

        public override async Task<int> CreateAsync(Area createEntity, CancellationToken cancellation)
        {
            return await Repository.CreateAsync(createEntity, cancellation);
        }

        public override async Task<int> DeleteAsync(Area deleteEntity, CancellationToken cancellation)
        {
            return await Repository.DeleteAsync(deleteEntity, cancellation);
        }

        public override async Task<IEnumerable<Area>> GetAllAsync(CancellationToken cancellation)
        {
            return await Repository.GetAllAsync(cancellation);
        }

        public override async Task<Area> GetByIdAsync(int id, CancellationToken cancellation)
        {
            return await Repository.GetByIdAsync(id, cancellation);
        }

        public override async Task<int> UpdateAsync(Area updateEntity, CancellationToken cancellation)
        {
            return await Repository.UpdateAsync(updateEntity, cancellation);
        }
    }
}
