using ObjectLibrary.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BlazorServerWebsite.Data.Services.Abstractions
{
    public interface IMenuService
    {
        Task<IEnumerable<Category>> InitializeMenu(CancellationToken cancellation);
    }
}
