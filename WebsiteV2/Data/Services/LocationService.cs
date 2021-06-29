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
    public class LocationService : BaseService<ILocationRepository, Location>, ILocationService
    {
        public LocationService(ILocationRepository repository) : base(repository)
        {

        }

        public override Task<int> CreateAsync(Location createEntity, CancellationToken cancellation)
        {
            throw new NotImplementedException();
        }

        public override Task<int> DeleteAsync(Location deleteEntity, CancellationToken cancellation)
        {
            throw new NotImplementedException();
        }

        public override Task<IEnumerable<Location>> GetAllAsync(CancellationToken cancellation)
        {
            throw new NotImplementedException();
        }

        public override Task<Location> GetByIdAsync(int id, CancellationToken cancellation)
        {
            throw new NotImplementedException();
        }

        public override Task<int> UpdateAsync(Location updateEntity, CancellationToken cancellation)
        {
            throw new NotImplementedException();
        }
    }
}
