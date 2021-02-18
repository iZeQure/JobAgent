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

        public async Task<User> GetUserByEmailAsync(string userMail)
        {
            return await DataAccessManager.UserDataAccessManager().GetUserByEmail(userMail);
        }

        public async Task<bool> UpdateUserInformationAsync(AccountModel accountModel)
        {
            var user = new User()
            {
                Identifier = accountModel.AccountId,
                Email = accountModel.Email,
                FirstName = accountModel.FirstName,
                LastName = accountModel.LastName,
                ConsultantArea = new ConsultantArea() { Identifier = accountModel.ConsultantAreaId },
                Location = new Location() { Identifier = accountModel.LocationId }
            };

            try
            {
                await DataAccessManager.UserDataAccessManager().Update(user);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task UpdateUserPasswordAsync(ChangePasswordModel auth)
        {
            SecurityService securityService = new SecurityService();

            string userSalt = await DataAccessManager.UserDataAccessManager().GetUserSaltByEmail(auth.Email);
            var hashPassword = await Task.FromResult(securityService.HashPasswordAsync(auth.Password, userSalt));

            auth.Password = hashPassword;

            var user = new User()
            {
                Email = auth.Email,
                Password = auth.Password
            };

            await DataAccessManager.UserDataAccessManager().UpdateUserPassword(user);
        }

        public async Task<bool> UpdateJobVacancyAsync(JobVacancyModel data)
        {
            var jobAdvert = new JobAdvert()
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
            };

            try
            {
                await DataAccessManager.JobAdvertDataAccessManager().Update(jobAdvert);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<IEnumerable<JobAdvert>> GetJobVacanciesAsync()
        {
            return await DataAccessManager.JobAdvertDataAccessManager().GetAllJobAdvertsForAdmins();
        }

        public async Task<JobAdvert> GetJobVacancyDetailsByIdAsync(int id)
        {
            return await DataAccessManager.JobAdvertDataAccessManager().GetJobAdvertDetailsForAdminsById(id);
        }

        public async Task CreateJobVacancyAsync(JobVacancyModel model)
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

            await DataAccessManager.JobAdvertDataAccessManager().Create(jobAdvert);
        }

        public async Task RemoveJobVacancyByIdAsync(int id)
        {
            await DataAccessManager.JobAdvertDataAccessManager().Remove(id);
        }

        public async Task<bool> CreateContractAsync(ContractModel contractModel)
        {
            var contract = new Contract()
            {
                ContactPerson = contractModel.ContactPerson,
                ContractName = contractModel.ContractFileName,
                ExpiryDate = contractModel.ExpiryDate,
                RegistrationDate = contractModel.RegistrationDate,
                SignedByUserId = new User()
                {
                    Identifier = contractModel.SignedByUser
                },
                Company = new Company()
                {
                    Identifier = contractModel.SignedWithCompany
                }
            };

            try
            {
                await DataAccessManager.ContractDataAccessManager().Create(contract);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> UpdateContractAsync(ContractModel contractModel)
        {
            var contract = new Contract()
            {
                Identifier = contractModel.Id,
                Company = new Company()
                {
                    Identifier = contractModel.SignedWithCompany
                },
                SignedByUserId = new User()
                {
                    Identifier = contractModel.SignedByUser
                },
                ContactPerson = contractModel.ContactPerson,
                ContractName = contractModel.ContractFileName,
                RegistrationDate = contractModel.RegistrationDate,
                ExpiryDate = contractModel.ExpiryDate
            };

            try
            {
                await DataAccessManager.ContractDataAccessManager().Update(contract);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task RemoveContractAsync(int id)
        {
            await DataAccessManager.ContractDataAccessManager().Remove(id);
        }

        public async Task<IEnumerable<Contract>> GetContractsAsync()
        {
            return await DataAccessManager.ContractDataAccessManager().GetAll();
        }

        public async Task<Contract> GetContractByIdAsync(int contractId)
        {
            return await DataAccessManager.ContractDataAccessManager().GetById(contractId);
        }

        public async Task CreateCompanyAsync(CompanyModel companyModel)
        {
            var company = new Company()
            {
                CVR = companyModel.CVR,
                Name = companyModel.Name,
                URL = companyModel.URL
            };

            await DataAccessManager.CompanyDataAccessManager().Create(company);
        }

        public async Task UpdateCompanyAsync(CompanyModel companyModel)
        {
            var company = new Company()
            {
                Identifier = companyModel.Id,
                CVR = companyModel.CVR,
                Name = companyModel.Name,
                URL = companyModel.URL
            };

            await DataAccessManager.CompanyDataAccessManager().Update(company);
        }

        public async Task RemoveCompanyByIdAsync(int id)
        {
            await DataAccessManager.CompanyDataAccessManager().Remove(id);
        }

        public async Task<bool> CreateSourceLinkAsync(SourceLinkModel sourceLinkModel)
        {
            var sourceLink = new SourceLink()
            {
                Company = new Company()
                {
                    Identifier = sourceLinkModel.CompanyId
                },
                Link = sourceLinkModel.Link
            };

            try
            {
                await DataAccessManager.SourceLinkDataAccessManager().Create(sourceLink);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> RemoveSourceLinkAsync(int id)
        {
            try
            {
                await DataAccessManager.SourceLinkDataAccessManager().Remove(id);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> UpdateSourceLinkAsync(SourceLinkModel sourceLinkModel)
        {
            var sourceLink = new SourceLink()
            {
                Identifier = sourceLinkModel.Id,
                Company = new Company() { Identifier = sourceLinkModel.CompanyId },
                Link = sourceLinkModel.Link
            };

            try
            {
                await DataAccessManager.SourceLinkDataAccessManager().Update(sourceLink);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
