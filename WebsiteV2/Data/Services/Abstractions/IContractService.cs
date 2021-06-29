using ObjectLibrary.Common;
using SecurityLibrary.Interfaces;

namespace BlazorServerWebsite.Data.Services.Abstractions
{
    public interface IContractService : IBaseService<Contract>, IFileAccess
    {
    }
}
