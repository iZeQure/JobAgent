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
    public abstract class BaseEntity<EntityType> : IBaseEntity<EntityType>
    {
        private EntityType _id;

        protected BaseEntity(EntityType id)
        {
            _id = id;
        }

        /// <summary>
        /// Get the unique id associated with this entity.
        /// </summary>
        public EntityType Id { get { return _id; } set { _id = value; } }
    }
}
