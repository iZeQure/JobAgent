using Pocos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories.Base
{
    public interface IRepository<T> where T : BaseEntity
    {
        /// <summary>
        /// Create new <see cref="T"/>.
        /// </summary>
        /// <param name="create"><see cref="T"/> is the object to parse through.</param>
        Task Create(T create);

        /// <summary>
        /// Update existing <see cref="T"/>.
        /// </summary>
        /// <param name="update"><see cref="T"/> is the object to update.</param>
        Task Update(T update);

        /// <summary>
        /// Remove reference by id.
        /// </summary>
        /// <param name="id">Is the identifier of the reference to remove.</param>
        Task Remove(int id);

        /// <summary>
        /// Get <see cref="T"/> by id.
        /// </summary>
        /// <param name="id">Is the identifier of the reference to get.</param>
        /// <returns>a object of <see cref="T"/>.</returns>
        Task<T> GetById(int id);

        /// <summary>
        /// Get all <see cref="T"/>.
        /// </summary>
        /// <returns>an abstraction of a collection with <see cref="IEnumerable{T}"/>.</returns>
        Task<IEnumerable<T>> GetAll();
    }
}
