﻿using JobAgent.Data.Objects;
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
        private protected IRepository<JobAdvert> JobAdvertRepository { get; }
        private protected IRepository<User> UserRepository { get; }
        private protected IRepository<ConsultantArea> ConsultantAreaRepository { get; }
        private protected IRepository<Location> LocationRepository { get; }
        private protected IRepository<Category> CategoryRepository { get; }
        private protected IRepository<Company> CompanyRepository { get; }
        private protected ISourceLinkRepository<SourceLink> SourceLinkRepository { get; set; }

        public DataService()
        {
            JobAdvertRepository = new JobAdvertRepository();
            UserRepository = new UserRepository();
            ConsultantAreaRepository = new ConsultantAreaRepository();
            LocationRepository = new LocationRepository();
            CategoryRepository = new CategoryRepository();
            CompanyRepository = new CompanyRepository();
            SourceLinkRepository = new SourceLinkRepository();
        }

        public Task<int> GetCountOfJobAdvertsByCategory(int id)
        {
            return ((IJobAdvertRepository)JobAdvertRepository).GetCountOfJobAdvertsByCategoryId(id);
        }

        public Task<int> GetCountOfJobAdvertsBySpecializationId(int id)
        {
            return ((IJobAdvertRepository)JobAdvertRepository).GetCountOfJobAdvertsBySpecializationId(id);
        }

        public Task<int> GetCountOfJobAdvertsUncategorized()
        {
            return ((IJobAdvertRepository)JobAdvertRepository).GetCountOfJobAdvertsUncategorized();
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
            return Task.FromResult(((ISpecializationRepository<Specialization>)CategoryRepository).GetAllSpecializations());
        }

        public Task<List<Company>> GetAllCompanies()
        {
            return Task.FromResult(CompanyRepository.GetAll().ToList());
        }

        public Task<IEnumerable<Company>> GetCompaniesWithContract()
        {
            return ((ICompanyRepository)CompanyRepository).GetCompaniesWithContract();
        }

        public Task<IEnumerable<Company>> GetCompaniesWithOutContract()
        {
            return ((ICompanyRepository)CompanyRepository).GetCompaniesWithOutContract();
        }

        public Task<Company> GetCompanyById(int id)
        {
            return Task.FromResult(CompanyRepository.GetById(id));
        }

        public Task<List<User>> GetUsers()
        {
            return Task.FromResult(UserRepository.GetAll().ToList());
        }

        public Task<List<SourceLink>> GetAllSourceLinksAsync()
        {
            if (SourceLinkRepository.GetAll().Result != null)
                return Task.FromResult(SourceLinkRepository.GetAll().Result.ToList());
            else
                return null;
        }

        public Task<SourceLink> GetSourceLinkById(int id)
        {
            return SourceLinkRepository.GetById(id);
        }

        public static string TruncateString(string value, int maxChars)
        {
            return value.Length <= maxChars ? value : value.Substring(0, maxChars) + " ...";
        }
    }
}
