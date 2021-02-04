using DataAccess;
using JobAgent.Models;
using Pocos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JobAgent.Services
{
    public class AdminService
    {
        private DataAccessManager DataAccessManager { get; }

        public AdminService()
        {
            DataAccessManager = new DataAccessManager();
        }

        public async Task<User> GetUserByEmail(string userMail)
        {
            return await DataAccessManager.UserDataAccessManager().GetUserByEmail(userMail);
        }

        public Task<bool> UpdateUserInformation(AccountModel user)
        {
            try
            {
                DataAccessManager.UserDataAccessManager().Update(
                        new User()
                        {
                            Identifier = user.AccountId,
                            Email = user.Email,
                            FirstName = user.FirstName,
                            LastName = user.LastName,
                            ConsultantArea = new ConsultantArea() { Identifier = user.ConsultantAreaId },
                            Location = new Location() { Identifier = user.LocationId }
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

            string userSalt = await DataAccessManager.UserDataAccessManager().GetUserSaltByEmail(auth.Email);
            var hashPassword = await Task.FromResult(securityService.HashPasswordAsync(auth.Password, userSalt));

            auth.Password = hashPassword.Result;

            DataAccessManager.UserDataAccessManager().UpdateUserPassword(
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
                DataAccessManager.JobAdvertDataAccessManager().Update(
                    new JobAdvert()
                    {
                        Identifier = data.Id,
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
                            Identifier = data.CompanyId.Value
                        },

                        // Category Data
                        Category = new Category()
                        {
                            Identifier = data.CategoryId
                        },
                        Specialization = new Specialization()
                        {
                            Identifier = data.SpecializationId.Value
                        }
                    });

                return Task.FromResult(true);
            }
            catch (Exception)
            {
                return Task.FromResult(false);
            }
        }

        public async Task<IEnumerable<JobAdvert>> GetJobVacancies()
        {
            return await DataAccessManager.JobAdvertDataAccessManager().GetAllJobAdvertsForAdmins();
        }

        public async Task<JobAdvert> GetJobVacancyDetailsById(int id)
        {
            return await DataAccessManager.JobAdvertDataAccessManager().GetJobAdvertDetailsForAdminsById(id);
        }

        public void CreateJobVacancy(JobVacancyModel model)
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
                        Identifier = model.CompanyId.Value
                    },
                    Category = new Category()
                    {
                        Identifier = model.CategoryId
                    },
                    Specialization = new Specialization()
                    {
                        Identifier = model.SpecializationId.Value
                    }
                };

            DataAccessManager.JobAdvertDataAccessManager().Create(jobAdvert);
        }

        public void RemoveJobVacancyById(int id)
        {
            DataAccessManager.JobAdvertDataAccessManager().Remove(id);
        }

        public Task<bool> CreateContract(ContractModel model)
        {
            DataAccessManager.ContractDataAccessManager().Create(
                new Contract()
                {
                    ContactPerson = model.ContactPerson,
                    ContractName = model.ContractFileName,
                    ExpiryDate = model.ExpiryDate,
                    RegistrationDate = model.RegistrationDate,
                    SignedByUserId = new User()
                    {
                        Identifier = model.SignedByUser
                    },
                    Company = new Company()
                    {
                        Identifier = model.SignedWithCompany
                    }
                });

            return Task.FromResult(true);
        }

        public Task<bool> UpdateContract(ContractModel model)
        {
            try
            {
                DataAccessManager.ContractDataAccessManager().Update(
                        new Contract()
                        {
                            Identifier = model.Id,
                            Company = new Company()
                            {
                                Identifier = model.SignedWithCompany
                            },
                            SignedByUserId = new User()
                            {
                                Identifier = model.SignedByUser
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
            DataAccessManager.ContractDataAccessManager().Remove(id);
        }

        public async Task<IEnumerable<Contract>> GetContracts()
        {
            return await DataAccessManager.ContractDataAccessManager().GetAll();
        }

        public async Task<Contract> GetContractById(int contractId)
        {
            return await DataAccessManager.ContractDataAccessManager().GetById(contractId);
        }

        public void CreateCompany(CompanyModel model)
        {
            DataAccessManager.CompanyDataAccessManager().Create(
                new Company()
                {
                    CVR = model.CVR,
                    Name = model.Name,
                    URL = model.URL
                });
        }

        public void UpdateCompany(CompanyModel update)
        {
            DataAccessManager.CompanyDataAccessManager().Update(
                new Company()
                {
                    Identifier = update.Id,
                    CVR = update.CVR,
                    Name = update.Name,
                    URL = update.URL
                });
        }

        public void RemoveCompanyById(int id)
        {
            DataAccessManager.CompanyDataAccessManager().Remove(id);
        }

        public Task<bool> CreateSourceLink(SourceLinkModel model)
        {
            DataAccessManager.SourceLinkDataAccessManager().Create(
                new SourceLink()
                {
                    Company = new Company()
                    {
                        Identifier = model.CompanyId
                    },
                    Link = model.Link
                });

            return Task.FromResult(true);
        }

        public Task<bool> RemoveSourceLink(int id)
        {
            DataAccessManager.SourceLinkDataAccessManager().Remove(id);
            return Task.FromResult(true);
        }

        public Task<bool> UpdateSourceLink(SourceLinkModel model)
        {
            DataAccessManager.SourceLinkDataAccessManager().Update(
                new SourceLink()
                {
                    Identifier = model.Id,
                    Company = new Company() { Identifier = model.CompanyId },
                    Link = model.Link
                });

            return Task.FromResult(true);
        }
    }
}
