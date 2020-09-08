using JobAgent.Data.Objects;
using JobAgent.Data.Repository;
using JobAgent.Data.Repository.Interface;
using JobAgent.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace JobAgent.Services
{
    public class AdminService
    {
        private IRepository<User> UserRepository { get; } = new UserRepository();

        private IRepository<JobAdvert> JobRepository { get; } = new JobAdvertRepository();

        private IRepository<Contract> ContractRepository { get; } = new ContractRepository();

        private IRepository<Company> CompanyRepository { get; } = new CompanyRepository();

        private ISourceLinkRepository SourceLinkRepository { get; } = new SourceLinkRepository();

        public Task<User> GetUserByEmail(string userMail)
        {
            return Task.FromResult(((IUserRepository)UserRepository).GetUserByEmail(userMail));
        }

        public Task<bool> UpdateUserInformation(User user)
        {
            UserRepository.Update(user);

            return Task.FromResult(true);
        }

        public async void UpdateUserPassword(User auth)
        {
            SecurityService securityService = new SecurityService();

            string userSalt = ((IUserRepository)UserRepository).GetUserSaltByEmail(auth.Email);
            var hashPassword = await Task.FromResult(securityService.HashPasswordAsync(auth.Password, userSalt));

            auth.Password = hashPassword.Result;

            ((IUserRepository)UserRepository).UpdateUserPassword(auth);
        }

        public void UpdateJobVacancy(JobVacanciesAdminModel data)
        {
            JobAdvert jobData = new JobAdvert()
            {
                // Job Data
                Id = data.JobAdvert.Id,
                Title = data.JobAdvert.Title,
                Email = data.JobAdvert.Email,
                PhoneNumber = data.JobAdvert.PhoneNumber,
                JobDescription = data.JobAdvert.JobDescription,
                JobLocation = data.JobAdvert.JobLocation,
                JobRegisteredDate = data.JobAdvert.JobRegisteredDate,
                DeadlineDate = data.JobAdvert.DeadlineDate,
                SourceURL = data.JobAdvert.SourceURL,

                // Company Data
                Company = new Company()
                {
                    Id = data.Company.Id
                },

                // Category Data
                Category = new Category()
                {
                    Id = data.Category.Id
                },
                Specialization = new Specialization()
                {
                    Id = data.Specialization.Id
                }
            };

            JobRepository.Update(jobData);
        }

        public Task<List<JobVacanciesAdminModel>> GetJobVacancies()
        {
            return Task.FromResult(((IJobAdvertRepository)JobRepository).GetAllJobAdvertsForAdmins().ToList());
        }

        public Task<JobVacanciesAdminModel> GetJobVacancyDetailsById(int id)
        {
            return Task.FromResult(((IJobAdvertRepository)JobRepository).GetJobAdvertDetailsForAdminsById(id));
        }

        public Task CreateJobVacancy(RegisterJobAdvertModel model)
        {
            JobAdvert jobAdvert =
                new JobAdvert()
                {
                    Title = model.Title,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    JobDescription = model.Description,
                    JobLocation = model.Location,
                    JobRegisteredDate = model.RegistrationDate,
                    DeadlineDate = model.DeadlineDate,
                    SourceURL = model.SourceURL,
                    Company = new Company()
                    {
                        Id = model.CompanyId
                    },
                    Category = new Category()
                    {
                        Id = model.CategoryId
                    },
                    Specialization = new Specialization()
                    {
                        Id = model.SpecializationId
                    }
                };

            JobRepository.Create(jobAdvert);

            return Task.CompletedTask;
        }

        public void RemoveJobVacancyById(int id)
        {
            JobRepository.Remove(id);
        }

        public Task<bool> CreateContract(ContractModel model)
        {
            ContractRepository.Create(
                new Contract()
                {
                    ContactPerson = model.ContactPerson,
                    ContractName = model.ContractFileName,
                    ExpiryDate = model.ExpiryDate,
                    RegistrationDate = model.RegistrationDate,
                    SignedByUserId = new User()
                    {
                        Id = model.SignedByUser
                    },
                    Company = new Company()
                    {
                        Id = model.SignedWithCompany
                    }
                });

            return Task.FromResult(true);
        }

        public Task<bool> UpdateContract(Contract contract)
        {
            ContractRepository.Update(contract);

            return Task.FromResult(true);
        }

        public Task<List<Contract>> GetContracts()
        {
            return Task.FromResult(ContractRepository.GetAll().ToList());
        }

        public Task<Contract> GetContractById(int contractId)
        {
            return Task.FromResult(ContractRepository.GetById(contractId));
        }

        public void CreateCompany(CompanyModel model)
        {
            CompanyRepository.Create(
                new Company()
                {
                    CVR = model.CVR,
                    Name = model.Name,
                    URL = model.URL
                });
        }

        public void UpdateCompany(Company update)
        {
            CompanyRepository.Update(update);
        }

        public void RemoveCompanyById(int id)
        {
            CompanyRepository.Remove(id);
        }

        public Task<bool> CreateSourceLink(SourceLinkModel model)
        {
            return SourceLinkRepository.Create(
                new SourceLink()
                {
                    Company = new Company()
                    {
                        Id = model.CompanyId
                    },
                    Link = model.Link
                });
        }

        public Task<bool> RemoveSourceLink(int id)
        {
            return SourceLinkRepository.Remove(id);
        }

        public Task<bool> UpdateSourceLink(SourceLinkModel model)
        {
            return SourceLinkRepository.Update(
                new SourceLink()
                {
                    Id = model.Id,
                    Company = new Company() { Id = model.CompanyId },
                    Link = model.Link
                });
        }
    }
}
