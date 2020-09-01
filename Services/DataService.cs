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
        private protected IRepository<User> UserRepository { get; }
        private protected IRepository<ConsultantArea> ConsultantAreaRepository { get; }
        private protected IRepository<Location> LocationRepository { get; }
        private protected IRepository<Category> CategoryRepository { get; }
        private protected IRepository<Company> CompanyRepository { get; }

        public DataService()
        {
            UserRepository = new UserRepository();
            ConsultantAreaRepository = new ConsultantAreaRepository();
            LocationRepository = new LocationRepository();
            CategoryRepository = new CategoryRepository();
            CompanyRepository = new CompanyRepository();
        }

        public Task<List<ConsultantArea>> GetAllConsultantAreasTask()
        {
            return Task.FromResult(ConsultantAreaRepository.GetAll().ToList());
        }

        public Task<List<Location>> GetAllLocationsTask()
        {
            return Task.FromResult(LocationRepository.GetAll().ToList());
        }

        public Task<List<Category>> GetAllCategories()
        {
            return Task.FromResult(CategoryRepository.GetAll().ToList());
        }
        public Task<List<Specialization>> GetAllSpecializations()
        {
            return Task.FromResult(((ISpecializationRepository)CategoryRepository).GetAllSpecializations());
        }

        public Task<List<Company>> GetAllCompanies()
        {
            return Task.FromResult(CompanyRepository.GetAll().ToList());
        }

        public Task<List<User>> GetUsers()
        {
            return Task.FromResult(UserRepository.GetAll().ToList());
        }
    }
}
