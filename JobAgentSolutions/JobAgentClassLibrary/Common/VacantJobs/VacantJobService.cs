using JobAgentClassLibrary.Common.VacantJobs.Entities;
using JobAgentClassLibrary.Common.VacantJobs.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobAgentClassLibrary.Common.VacantJobs
{
    public class VacantJobService : IVacantJobService
    {
        private readonly IVacantJobRepository _repository;

        public VacantJobService(IVacantJobRepository repository)
        {
            _repository = repository;
        }

        public async Task<IVacantJob> CreateAsync(IVacantJob entity)
        {
            try
            {
                return await _repository.CreateAsync(entity);
            }
            catch (System.Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<List<IVacantJob>> GetAllAsync()
        {
            try
            {
                return await _repository.GetAllAsync();
            }
            catch (System.Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<IVacantJob> GetByIdAsync(int id)
        {
            try
            {
                return await _repository.GetByIdAsync(id);
            }
            catch (System.Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<bool> RemoveAsync(IVacantJob entity)
        {
            try
            {
                return await _repository.RemoveAsync(entity);
            }
            catch (System.Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                return false;
            }
        }

        public async Task<IVacantJob> UpdateAsync(IVacantJob entity)
        {
            try
            {
                return await _repository.UpdateAsync(entity);
            }
            catch (System.Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                return null;
            }
        }


    }
}
