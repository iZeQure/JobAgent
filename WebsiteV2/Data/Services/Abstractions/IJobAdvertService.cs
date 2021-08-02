using BlazorServerWebsite.Data.FormModels;
using ObjectLibrary.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BlazorServerWebsite.Data.Services.Abstractions
{
    public interface IJobAdvertService : IBaseService<JobAdvert>
    {
        Task<JobAdvertPaginationModel> JobAdvertPagination(CancellationToken cancellation, int page = 1);
        Task<JobAdvertPaginationModel> JobAdvertPagination(CancellationToken cancellation, int resultsPerPage, int page = 1);
        Task<JobAdvertPaginationModel> FilteredJobAdvertPagination(CancellationToken cancellation, int sortByCategoryId, int page = 1);
    }
}
