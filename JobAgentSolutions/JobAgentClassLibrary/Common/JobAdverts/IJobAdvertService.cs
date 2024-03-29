﻿using JobAgentClassLibrary.Common.JobAdverts.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JobAgentClassLibrary.Common.JobAdverts
{
    public interface IJobAdvertService
    {
        Task<IJobAdvert> CreateAsync(IJobAdvert entity);
        Task<List<IJobAdvert>> GetJobAdvertsAsync();
        Task<IJobAdvert> GetByIdAsync(int id);
        Task<int> GetJobAdvertCountByCategoryId(int id);
        Task<int> GetJobAdvertCountBySpecializationId(int id);
        Task<int> GetJobAdvertCountByUncategorized();
        Task<bool> RemoveAsync(IJobAdvert entity);
        Task<IJobAdvert> UpdateAsync(IJobAdvert entity);
    }
}
