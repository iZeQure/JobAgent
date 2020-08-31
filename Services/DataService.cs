using JobAgent.Data.Objects;
using JobAgent.Data.Repository;
using JobAgent.Data.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JobAgent.Services
{
    public class DataService
    {
        private protected IRepository<ConsultantArea> ConsultantAreaRepository { get; }
        private protected IRepository<Location> LocationRepository { get; }

        public DataService()
        {
            ConsultantAreaRepository = new ConsultantAreaRepository();
            LocationRepository = new LocationRepository();
        }

        public Task<List<ConsultantArea>> GetAllConsultantAreasTask()
        {
            return Task.FromResult(ConsultantAreaRepository.GetAll().ToList());
        }

        public Task<List<Location>> GetAllLocationsTask()
        {
            return Task.FromResult(LocationRepository.GetAll().ToList());
        }
    }
}
