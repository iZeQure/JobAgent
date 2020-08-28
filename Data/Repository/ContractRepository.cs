using JobAgent.Data.DB;
using JobAgent.Data.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace JobAgent.Data.Repository
{
    public class ContractRepository : IContractRepository
    {
        public void Create(Contract create)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Contract> GetAll()
        {
            // Initialize command obj.
            using SqlCommand c = new SqlCommand()
            {
                CommandText = "GetAllContracts",
                CommandType = CommandType.StoredProcedure,
                Connection = Database.Instance.SqlConnection
            };

            Database.Instance.OpenConnection();

            // Initialzie data reader.
            using SqlDataReader r = c.ExecuteReader();

            // Temporary list.
            List<Contract> tempContracts = new List<Contract>();

            // Check for any data.
            if (r.HasRows)
            {
                // Read data.
                while (r.Read())
                {
                    tempContracts.Add(
                        new Contract()
                        {
                            Id = r.GetInt32("Id"),
                            ContactPerson = r.GetString("ContactPerson"),
                            ContractLocation = r.GetString("ContractLocation"),
                            ExpiryDate = r.GetDateTime("ExpiryDate"),
                            RegistrationDate = r.GetDateTime("RegisteredDate"),
                            SignedByUserId = new User()
                            {
                                FirstName = r.GetString("FirstName"),
                                LastName = r.GetString("LastName")
                            },
                            CompanyCVR = new Company()
                            {
                                Id = r.GetInt32("CVR"),
                                Name = r.GetString("Name")
                            }
                        });
                }
            }

            // Return data list.
            return tempContracts;
        }

        public Contract GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Remove(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(Contract update)
        {
            throw new NotImplementedException();
        }
    }
}
