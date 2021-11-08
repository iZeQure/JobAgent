using System;
using JobAgentClassLibrary.Core.Entities;
using System.Data.SqlClient;

namespace JobAgentClassLibrary.Core.Database.Managers
{
    public interface ISqlDbManager
    {
        /// <summary>
        /// Get an <see cref="SqlConnection"/>.
        /// </summary>
        /// <param name="connectionType">Defines the credentials used for the sql connection.</param>
        /// <returns>An initialized sql connection.</returns>
        /// <exception cref="ArgumentException">Is thrown whenever a credential type isn't supported.</exception>
        SqlConnection GetSqlConnection(DbCredentialType connectionType);
    }
}