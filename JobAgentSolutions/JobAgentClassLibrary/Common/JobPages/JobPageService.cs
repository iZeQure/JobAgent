using JobAgentClassLibrary.Common.JobPages.Entities;
using JobAgentClassLibrary.Common.JobPages.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JobAgentClassLibrary.Common.JobPages
{
    public class JobPageService : IJobPageService
    {
        private readonly IJobPageRepository _jobPageRepository;

        public JobPageService(IJobPageRepository jobPageRepository)
        {
            _jobPageRepository = jobPageRepository;
        }

        /// <summary>
        /// Creates a new JobPage in the database
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>the created object</returns>
        public async Task<IJobPage> CreateAsync(IJobPage entity)
        {
            try
            {
                return await _jobPageRepository.CreateAsync(entity);
            }
            catch (System.Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Returns a specific JobPage from the database
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IJobPage> GetByIdAsync(int id)
        {
            try
            {
                return await _jobPageRepository.GetByIdAsync(id);
            }
            catch (System.Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Returns all JobPages in the database
        /// </summary>
        /// <returns></returns>
        public async Task<List<IJobPage>> GetJobPagesAsync()
        {
            try
            {
                return await _jobPageRepository.GetAllAsync();
            }
            catch (System.Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Removes a JobPage from the database
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<bool> RemoveAsync(IJobPage entity)
        {
            try
            {
                return await _jobPageRepository.RemoveAsync(entity);
            }
            catch (System.Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Updates a JobPage in the database
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<IJobPage> UpdateAsync(IJobPage entity)
        {
            try
            {
                return await _jobPageRepository.UpdateAsync(entity);
            }
            catch (System.Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                return null;
            }
        }
    }
}
