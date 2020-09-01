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
        private IRepository<User> UserRepository { get; } = new UserRepository();

        private IRepository<JobAdvert> JobRepository { get; } = new JobAdvertRepository();

        private IRepository<Contract> ContractRepository { get; } = new ContractRepository();        

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
                CompanyCVR = new Company()
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

        public void RemoveJobVacancyById(int id)
        {
            JobRepository.Remove(id);
        }

        public Task<List<Contract>> GetContracts()
        {
            return Task.FromResult(ContractRepository.GetAll().ToList());
        }

        public Task<Contract> GetContractById(int contractId)
        {
            return Task.FromResult(ContractRepository.GetById(contractId));
        }
    }
}
