using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JobAgent.Data
{
    public abstract class BaseEntity
    {
        #region Attributes
        private int id;
        #endregion

        #region Properties
        public int Id { get { return id; } set { id = value; } }
        #endregion
    }
}
