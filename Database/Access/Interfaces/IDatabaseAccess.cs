using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Database.Access.Interfaces
{
    public interface IDatabaseAccess : IDisposable
    {
        /// <summary>
        /// Open an asyncronous connection.
        /// </summary>
        /// <returns></returns>
        Task OpenConnectionAsync();

        /// <summary>
        /// Close the database access connection.
        /// </summary>
        void CloseConnection();
    }
}
