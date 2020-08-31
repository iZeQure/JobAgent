using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using JobAgent.Data.Objects;

namespace JobAgent.Data.Repository.Interface
{
    interface IRepository<T> where T : BaseEntity
    {
        /// <summary>
        /// Create new <see cref="T"/>.
        /// </summary>
        /// <param name="create"><see cref="T"/> is the object to parse through.</param>
        void Create(T create);

        /// <summary>
        /// Update existing <see cref="T"/>.
        /// </summary>
        /// <param name="update"><see cref="T"/> is the object to update.</param>
        void Update(T update);

        /// <summary>
        /// Remove reference by id.
        /// </summary>
        /// <param name="id">Is the identifier of the reference to remove.</param>
        void Remove(int id);

        /// <summary>
        /// Get <see cref="T"/> by id.
        /// </summary>
        /// <param name="id">Is the identifier of the reference to get.</param>
        /// <returns>a object of <see cref="T"/>.</returns>
        T GetById(int id);

        /// <summary>
        /// Get all <see cref="T"/>.
        /// </summary>
        /// <returns>an abstraction of a collection with <see cref="IEnumerable{T}"/>.</returns>
        IEnumerable<T> GetAll();
    }
}
