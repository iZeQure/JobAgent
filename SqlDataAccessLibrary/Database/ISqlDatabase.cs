using System;
using System.Threading;
using System.Threading.Tasks;

namespace SqlDataAccessLibrary.Database
{
    public interface ISqlDatabase : IDisposable
    {
        Task OpenConnectionAsync(CancellationToken cancellation);
    }
}