﻿using JobAgent.Data.Objects;
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
        private IRepository<User> UserRepository { get; }

        private IRepository<JobAdvert> JobRepository { get; }

        private IRepository<Contract> ContractRepository { get; }

        private IRepository<Company> CompanyRepository { get; }

        private ISourceLinkRepository<SourceLink> SourceLinkRepository { get; }

        public AdminService()
        {
            UserRepository = new UserRepository();
            JobRepository = new JobAdvertRepository();
            ContractRepository = new ContractRepository();
            CompanyRepository = new CompanyRepository();
            SourceLinkRepository = new SourceLinkRepository();
        }

        public Task<User> GetUserByEmail(string userMail)
        {
            return Task.FromResult(((IUserRepository)UserRepository).GetUserByEmail(userMail));
        }

        public Task<bool> UpdateUserInformation(AccountModel user)
        {
            try
            {
                UserRepository.Update(
                        new User()
                        {
                            Id = user.AccountId,
                            Email = user.Email,
                            FirstName = user.FirstName,
                            LastName = user.LastName,
                            ConsultantArea = new ConsultantArea() { Id = user.ConsultantAreaId },
                            Location = new Location() { Id = user.LocationId }
                        });

                return Task.FromResult(true);
            }
            catch (Exception)
            {
                return Task.FromResult(false);
            }
        }

        public async void UpdateUserPassword(ChangePasswordModel auth)
        {
            SecurityService securityService = new SecurityService();

            string userSalt = ((IUserRepository)UserRepository).GetUserSaltByEmail(auth.Email);
            var hashPassword = await Task.FromResult(securityService.HashPasswordAsync(auth.Password, userSalt));

            auth.Password = hashPassword.Result;

            ((IUserRepository)UserRepository).UpdateUserPassword(
                new User()
                {
                    Email = auth.Email,
                    Password = auth.Password
                });
        }

        public Task<bool> UpdateJobVacancy(JobVacancyModel data)
        {
            try
            {
                JobRepository.Update(
                    new JobAdvert()
                    {
                        Id = data.Id,
                        Title = data.Title,
                        Email = data.Email,
                        PhoneNumber = data.PhoneNumber,
                        JobDescription = data.Description,
                        JobLocation = data.Location,
                        JobRegisteredDate = data.RegisteredDate,
                        DeadlineDate = data.DeadlineDate,
                        SourceURL = data.SourceURL,

                        // Company Data
                        Company = new Company()
                        {
                            Id = data.CompanyId.Value
                        },

                        // Category Data
                        Category = new Category()
                        {
                            Id = data.CategoryId
                        },
                        Specialization = new Specialization()
                        {
                            Id = data.SpecializationId.Value
                        }
                    });

                return Task.FromResult(true);
            }
            catch (Exception)
            {
                return Task.FromResult(false);
            }
        }

        public Task<IEnumerable<JobAdvert>> GetJobVacancies()
        {
            return ((IJobAdvertRepository)JobRepository).GetAllJobAdvertsForAdmins();
        }

        public Task<JobAdvert> GetJobVacancyDetailsById(int id)
        {
            return ((IJobAdvertRepository) JobRepository).GetJobAdvertDetailsForAdminsById(id);
        }

        public Task CreateJobVacancy(JobVacancyModel model)
        {
            JobAdvert jobAdvert =
                new JobAdvert()
                {
                    Title = model.Title,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    JobDescription = model.Description,
                    JobLocation = model.Location,
                    JobRegisteredDate = model.RegisteredDate,
                    DeadlineDate = model.DeadlineDate,
                    SourceURL = model.SourceURL,
                    Company = new Company()
                    {
                        Id = model.CompanyId.Value
                    },
                    Category = new Category()
                    {
                        Id = model.CategoryId
                    },
                    Specialization = new Specialization()
                    {
                        Id = model.SpecializationId.Value
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

        public Task<bool> UpdateContract(ContractModel model)
        {
            try
            {
                ContractRepository.Update(
                        new Contract()
                        {
                            Id = model.Id,
                            Company = new Company()
                            {
                                Id = model.SignedWithCompany
                            },
                            SignedByUserId = new User()
                            {
                                Id = model.SignedByUser
                            },
                            ContactPerson = model.ContactPerson,
                            ContractName = model.ContractFileName,
                            RegistrationDate = model.RegistrationDate,
                            ExpiryDate = model.ExpiryDate
                        });

                return Task.FromResult(true);
            }
            catch (Exception)
            {
                return Task.FromResult(false);
            }            
        }

        public void RemoveContract(int id)
        {
            ContractRepository.Remove(id);
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

        public void UpdateCompany(CompanyModel update)
        {
            CompanyRepository.Update(
                new Company()
                {
                    Id = update.Id,
                    CVR = update.CVR,
                    Name = update.Name,
                    URL = update.URL
                });
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
