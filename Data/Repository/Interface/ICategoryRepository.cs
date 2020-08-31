using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JobAgent.Data.Objects;

namespace JobAgent.Data.Repository.Interface
{
    interface ICategoryRepository : IRepository<Category>
    {
        /// <summary>
        /// Create new category specialization.
        /// </summary>
        /// <param name="create">Used to indentify the specialization information.</param>
        /// <param name="categoryId">Used to specify the category, the specialization is owned by.</param>
        void CreateCategorySpecialization(Specialization create, int categoryId);

        /// <summary>
        /// Update an existing category specialization.
        /// </summary>
        /// <param name="update">Used to define the fields that needs an update - Leave empty if no new content.</param>
        void UpdateCategorySpecialization(Specialization update);

        /// <summary>
        /// Remove an existing category specialization.
        /// </summary>
        /// <param name="id">Used to specify the data to remove.</param>
        void RemoveCategorySpecialization(int id);

        /// <summary>
        /// Get category specialization by id.
        /// </summary>
        /// <param name="categoryId">Used to identify the category that owns the specialization.</param>
        /// <param name="specializationId">Used to indicate the specialization to return.</param>
        /// <returns>Returns <see cref="Specialization"/></returns>
        Specialization GetCategorySpecializationById(int id);

        /// <summary>
        /// Get all category specializations.
        /// </summary>
        /// <returns>Returns a list of <see cref="Specialization"/></returns>
        List<Specialization> GetAllCategorySpecializations();

        /// <summary>
        /// Get all categories with specializations bound to.
        /// </summary>
        /// <returns>Returns a list of <see cref="Category"/>, with a list of <see cref="Specialization"/>, if the category owns a specialization.</returns>
        List<Category> GetAllCategoriesWithSpecializations();
    }
}
