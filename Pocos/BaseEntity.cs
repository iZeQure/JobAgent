using System;
using System.Collections.Generic;
using System.Text;

namespace Pocos
{
    /// <summary>
    /// This class defines the base of an entity.
    /// </summary>
    public abstract class BaseEntity
    {
        #region Fields
        private int _identifier;
        #endregion

        #region Properties
        /// <summary>
        /// The unique id of the entity.
        /// </summary>
        public int Identifier { get => _identifier; set => _identifier = value; }
        #endregion
    }
}
