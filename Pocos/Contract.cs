using System;

namespace Pocos
{
    public class Contract : BaseEntity
    {
        #region Fields
        private string contactPerson;
        private string contractName;
        private DateTime expiryDate;
        private DateTime registrationDate;
        private User signedByUserId;
        private Company company;
        #endregion

        #region Properties
        public string ContactPerson { get { return contactPerson; } set { contactPerson = value; } }
        public string ContractName { get { return contractName; } set { contractName = value; } }
        public DateTime ExpiryDate { get { return expiryDate; } set { expiryDate = value; } }
        public DateTime RegistrationDate { get { return registrationDate; } set { registrationDate = value; } }
        public User SignedByUserId { get { return signedByUserId; } set { signedByUserId = value; } }
        public Company Company { get { return company; } set { company = value; } }
        #endregion
    }
}
