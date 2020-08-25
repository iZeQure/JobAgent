﻿using JobAgent.Data;
using JobAgent.Data.Repository;
using JobAgent.Data.Repository.Interface;
using JobAgent.Data.Security;
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
        private IRepository<JobAdvert> JobRepository { get; } = new JobAdvertRepository();

        private IRepository<JobAdvertCategory> CategoryRepository { get; } = new JobAdvertCategoryRepository();

        public Task<User> GetUserByEmail(string userMail)
        {
            IRepository<User> userRepository = new UserRepository();

            return Task.FromResult(((IUserRepository)userRepository).GetUserByEmail(userMail));
        }        

        public Task<bool> UpdateUserInformation(User user)
        {
            IRepository<User> repository = new UserRepository();

            repository.Update(user);

            return Task.FromResult(true);
        }

        public async void UpdateUserPassword(User auth)
        {
            IRepository<User> repository = new UserRepository();
            SecurityService securityService = new SecurityService();

            string userSalt = ((IUserRepository)repository).GetUserSaltByEmail(auth.Email);
            var hashPassword = await Task.FromResult(securityService.HashPasswordAsync(auth.Password, userSalt));

            auth.Password = hashPassword.Result;

            ((IUserRepository)repository).UpdateUserPassword(auth);
        }

        public Task<List<JobVacanciesAdminModel>> GetJobVacancies()
        {
            return Task.FromResult(((IJobAdvertRepository)JobRepository).GetJobAdvertsForAdmins().ToList());
        }

        public Task<JobVacanciesAdminModel> GetJobVacancyDetailsById(int id)
        {
            return Task.FromResult(((IJobAdvertRepository)JobRepository).GetJobAdvertDetailsForAdminsById(id));
        }

        public Task<List<JobAdvertCategory>> GetAllJobAdvertCategories()
        {
            return Task.FromResult(CategoryRepository.GetAll().ToList());
        }

        public Task<List<JobAdvertCategorySpecialization>> GetAllJobAdvertCategorySpecializations()
        {
            return Task.FromResult(((IJobAdvertCategoryRepository)CategoryRepository).GetAllJobAdvertCategorySpecializations());
        }
    }
}
