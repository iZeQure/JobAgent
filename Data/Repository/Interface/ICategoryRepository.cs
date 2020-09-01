using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JobAgent.Data.Objects;

namespace JobAgent.Data.Repository.Interface
{
    interface ICategoryRepository : IRepository<Category>, ISpecializationRepository
    {
        /// <summary>
        /// Get all categories with specializations bound to.
        /// </summary>
        /// <returns>Returns a list of <see cref="Category"/>, with a list of <see cref="Specialization"/>, if the category owns a specialization.</returns>
        List<Category> GetAllCategoriesWithSpecializations();
    }
}
