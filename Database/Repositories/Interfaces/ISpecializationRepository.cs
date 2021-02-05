using DataAccess.Repositories.Base;
using Pocos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccess.Repositories.Interfaces
{
    public interface ISpecializationRepository : IRepository<Specialization>
    {
        /// <summary>
        /// Create new category specialization.
        /// </summary>
        /// <param name="create">Used to indentify the specialization information.</param>
        /// <param name="categoryId">Used to specify the category, the specialization is owned by.</param>
        void CreateSpecialization(Specialization create, int categoryId);

        /// <summary>
        /// Update an existing category specialization.
        /// </summary>
        /// <param name="update">Used to define the fields that needs an update - Leave empty if no new content.</param>
        void UpdateSpecialization(Specialization update);

        /// <summary>
        /// Remove an existing category specialization.
        /// </summary>
        /// <param name="id">Used to specify the data to remove.</param>
        void RemoveSpecialization(int id);

        /// <summary>
        /// Get category specialization by id.
        /// </summary>
        /// <param name="categoryId">Used to identify the category that owns the specialization.</param>
        /// <param name="specializationId">Used to indicate the specialization to return.</param>
        /// <returns>Returns <see cref="Specialization"/></returns>
        Task<Specialization> GetSpecializationById(int id);

        /// <summary>
        /// Get all category specializations.
        /// </summary>
        /// <returns>Returns a list of <see cref="Specialization"/></returns>
        Task<IEnumerable<Specialization>> GetAllSpecializations();
    }
}
