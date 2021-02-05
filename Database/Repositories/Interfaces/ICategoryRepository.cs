using DataAccess.Repositories.Base;
using Pocos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories.Interfaces
{
    public interface ICategoryRepository : IRepository<Category>, ISpecializationRepository
    {
        /// <summary>
        /// Get all categories with specializations bound to.
        /// </summary>
        /// <returns>Returns a list of <see cref="Category"/>, with a list of <see cref="Specialization"/>, if the category owns a specialization.</returns>
        Task<IEnumerable<Category>> GetAllCategoriesWithSpecializations();

        Task<IEnumerable<Category>> GetAllCategories();
    }
}
