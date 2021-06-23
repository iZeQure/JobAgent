﻿using ObjectLibrary.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SqlDataAccessLibrary.Repositories.Abstractions
{
    public interface ISpecializationRepository : IRepository<Specialization>
    {
        /// <summary>
        /// Create new category specialization.
        /// </summary>
        /// <param name="create">Used to indentify the specialization information.</param>
        /// <param name="categoryId">Used to specify the category, the specialization is owned by.</param>
        Task CreateSpecialization(Specialization create, int categoryId, CancellationToken cancellation);

        /// <summary>
        /// Update an existing category specialization.
        /// </summary>
        /// <param name="update">Used to define the fields that needs an update - Leave empty if no new content.</param>
        Task UpdateSpecialization(Specialization update, CancellationToken cancellation);

        /// <summary>
        /// Remove an existing category specialization.
        /// </summary>
        /// <param name="id">Used to specify the data to remove.</param>
        Task RemoveSpecialization(int id, CancellationToken cancellation);

        /// <summary>
        /// Get category specialization by id.
        /// </summary>
        /// <param name="categoryId">Used to identify the category that owns the specialization.</param>
        /// <param name="specializationId">Used to indicate the specialization to return.</param>
        /// <returns>Returns <see cref="Specialization"/></returns>
        Task<Specialization> GetSpecializationById(int id, CancellationToken cancellation);

        /// <summary>
        /// Get all category specializations.
        /// </summary>
        /// <returns>Returns a list of <see cref="Specialization"/></returns>
        Task<IEnumerable<Specialization>> GetAllSpecializations(CancellationToken cancellation);
    }
}
