using JobAgent.Data.DataAccess;
using JobAgent.Data.Objects;
using JobAgent.Data.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace JobAgent.Data.Repository
{
    public class ContractRepository : IContractRepository
    {
        /// <summary>
        /// Not Implemented
        /// </summary>
        /// <param name="create"></param>
        public void Create(Contract create)
        {
            // Initialzie command obj.
            using SqlCommand c = new SqlCommand()
            {
                CommandText = "CreateContract",
                CommandType = CommandType.StoredProcedure,
                Connection = SqlDataAccess.Instance.SqlConnection
            };

            // Parameters.
            c.Parameters.AddWithValue("@contactPerson", create.ContactPerson);
            c.Parameters.AddWithValue("@contractName", create.ContractName);
            c.Parameters.AddWithValue("@expiryDate", create.ExpiryDate);
            c.Parameters.AddWithValue("@registeredDate", create.RegistrationDate);
            c.Parameters.AddWithValue("@signedByUserId", create.SignedByUserId.Id);
            c.Parameters.AddWithValue("@companyId", create.Company.Id);

            // Open connection to database.
            SqlDataAccess.Instance.OpenConnection();

            try
            {
                c.ExecuteNonQuery();
            }
            finally
            {
                SqlDataAccess.Instance.CloseConnection();
            }
        }

        public IEnumerable<Contract> GetAll()
        {
            // Initialize command obj.
            using SqlCommand c = new SqlCommand()
            {
                CommandText = "GetAllContracts",
                CommandType = CommandType.StoredProcedure,
                Connection = SqlDataAccess.Instance.SqlConnection
            };

            // Open connection to database.
            SqlDataAccess.Instance.OpenConnection();

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
                            ContractName = r.GetString("ContractName"),
                            ExpiryDate = r.GetDateTime("ExpiryDate"),
                            RegistrationDate = r.GetDateTime("RegisteredDate"),
                            SignedByUserId = new User()
                            {
                                Id = r.GetInt32("UserId"),
                                FirstName = r.GetString("FirstName"),
                                LastName = r.GetString("LastName")
                            },
                            Company = new Company()
                            {
                                Id = r.GetInt32("Id"),
                                CVR = r.GetInt32("CVR"),
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
            // Initialize command obj.
            using SqlCommand c = new SqlCommand()
            {
                CommandText = "GetContractById",
                CommandType = CommandType.StoredProcedure,
                Connection = SqlDataAccess.Instance.SqlConnection
            };

            c.Parameters.AddWithValue("@id", id);

            SqlDataAccess.Instance.OpenConnection();

            // Initialzie data reader.
            using SqlDataReader r = c.ExecuteReader();

            // Temporary contract.
            Contract tempContract = new Contract();

            try
            {
                // Check for any data.
                if (r.HasRows)
                {
                    // Read data.
                    while (r.Read())
                    {
                        tempContract.Id = r.GetInt32("Id");
                        tempContract.ContactPerson = r.GetString("ContactPerson");
                        tempContract.ContractName = r.GetString("ContractName");
                        tempContract.ExpiryDate = r.GetDateTime("ExpiryDate");
                        tempContract.RegistrationDate = r.GetDateTime("RegisteredDate");
                        tempContract.SignedByUserId =
                            new User()
                            {
                                Id = r.GetInt32("UserId"),
                                FirstName = r.GetString("FirstName"),
                                LastName = r.GetString("LastName")
                            };
                        tempContract.Company =
                            new Company()
                            {
                                Id = r.GetInt32("CompanyId"),
                                CVR = r.GetInt32("CVR"),
                                Name = r.GetString("Name")
                            };

                        //tempContract = new Contract()
                        //{
                        //    Id = r.GetInt32("Id"),
                        //    ContactPerson = r.GetString("ContactPerson"),
                        //    ContractName = r.GetString("ContractName"),
                        //    ExpiryDate = r.GetDateTime("ExpiryDate"),
                        //    RegistrationDate = r.GetDateTime("RegisteredDate"),
                        //    SignedByUserId = new User()
                        //    {
                        //        Id = r.GetInt32("UserId"),
                        //        FirstName = r.GetString("FirstName"),
                        //        LastName = r.GetString("LastName")
                        //    },
                        //    Company = new Company()
                        //    {
                        //        Id = r.GetInt32("CompanyId"),
                        //        CVR = r.GetInt32("CVR"),
                        //        Name = r.GetString("Name")
                        //    }
                        //};
                    }
                }
            }
            finally
            {
                SqlDataAccess.Instance.CloseConnection();
            }

            return tempContract;
        }

        /// <summary>
        /// Not Implemented
        /// </summary>
        /// <param name="id"></param>
        public void Remove(int id)
        {
            using SqlCommand c = new SqlCommand()
            {
                CommandText = "RemoveContract",
                CommandTimeout = 15,
                CommandType = CommandType.StoredProcedure,
                Connection = SqlDataAccess.Instance.SqlConnection
            };

            c.Parameters.AddWithValue("@id", id);

            try
            {
                SqlDataAccess.Instance.OpenConnection();

                c.ExecuteNonQuery();
            }
            finally
            {
                SqlDataAccess.Instance.CloseConnection();
            }
        }

        /// <summary>
        /// Not Implemented
        /// </summary>
        /// <param name="update"></param>
        public void Update(Contract update)
        {
            // Initialize command obj.
            using SqlCommand c = new SqlCommand()
            {
                CommandText = "UpdateContract",
                CommandType = CommandType.StoredProcedure,
                Connection = SqlDataAccess.Instance.SqlConnection
            };

            // Parameters.
            c.Parameters.AddWithValue("@id", update.Id);
            c.Parameters.AddWithValue("@companyId", update.Company.Id);
            c.Parameters.AddWithValue("@signedByUserId", update.SignedByUserId.Id);
            c.Parameters.AddWithValue("@contactPerson", update.ContactPerson);
            c.Parameters.AddWithValue("@regDate", update.RegistrationDate);
            c.Parameters.AddWithValue("@expiryDate", update.ExpiryDate);

            // Open connection to database.
            SqlDataAccess.Instance.OpenConnection();

            try
            {
                c.ExecuteNonQuery();
            }
            finally
            {
                SqlDataAccess.Instance.CloseConnection();
            }
        }
    }
}
