using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectLibrary.Common
{
    /// <summary>
    /// Describes the base of an object entity.
    /// </summary>
    public abstract class BaseEntity : IBaseEntity
    {
        private int _id;

        protected BaseEntity(int id)
        {
            _id = id;
        }

        /// <summary>
        /// Get the unique id associated with this entity.
        /// </summary>
        public int Id { get { return _id; } set { _id = value; } }
    }
}
