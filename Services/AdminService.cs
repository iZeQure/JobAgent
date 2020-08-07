using JobAgent.Data;
using JobAgent.Data.Repository;
using JobAgent.Data.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace JobAgent.Services
{
    public class AdminService
    {
        public Task<User> GetUserByEmail(string userMail)
        {
            IUserRepository userRepository = new UserRepository();

            return Task.FromResult(userRepository.GetUserByEmail(userMail));
        }

        public Task<List<ConsultantArea>> GetAllConsultantAreas()
        {
            IConsultantAreaRepository consultantAreaRepository = new ConsultantAreaRepository();

            return Task.FromResult(consultantAreaRepository.GetAll().ToList());
        }

        public Task<List<Location>> GetAllLocations()
        {
            ILocationRepository locationRepository = new LocationRepository();

            return Task.FromResult(locationRepository.GetAll().ToList());
        }

        public void UpdateUserInformation(User user)
        {
            IUserRepository userRepository = new UserRepository();

            userRepository.Update(user);
        }
    }
}
