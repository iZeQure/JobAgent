using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JobAgent.Data
{
    public class Company : BaseEntity
    {
        #region Attributes
        private string name;
        private Contract contractId;
        #endregion

        #region Properties
        public string Name { get { return name; } set { name = value; } }
        public Contract ContractId{ get { return contractId; } set { contractId = value; } }
        #endregion
    }
}
