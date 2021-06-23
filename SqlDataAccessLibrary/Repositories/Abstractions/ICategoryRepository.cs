using ObjectLibrary.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SqlDataAccessLibrary.Repositories.Abstractions
{
    public interface ICategoryRepository : IRepository<Category>
    {
        /// <summary>
        /// Get all categories with specializations bound to.
        /// </summary>
        /// <returns>Returns a list of <see cref="Category"/>, with a list of <see cref="Specialization"/>, if the category owns a specialization.</returns>
        Task<IEnumerable<Category>> GetAllCategoriesWithSpecializations(CancellationToken cancellation);

        Task<IEnumerable<Category>> GetAllCategories(CancellationToken cancellation);
    }
}
