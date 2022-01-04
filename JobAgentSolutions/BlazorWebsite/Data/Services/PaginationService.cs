using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BlazorWebsite.Data.Services
{
    public class PaginationService : ComponentBase
    {
        public int CurrentPage { get; set; } = 1;

        public int ResultsPerPage { get; set; } = 25;

        public int PageCount<TPagingModel>(IEnumerable<TPagingModel> data)
        {
            if (data is null) return 0;

            double smallestValue = Math.Ceiling(data.Count() / (double)ResultsPerPage);

            return Convert.ToInt32(smallestValue);
        }

        public IEnumerable<TPagingModel> CreatePaging<TPagingModel>(IEnumerable<TPagingModel> data)
        {
            int start = (CurrentPage - 1) * ResultsPerPage;

            IEnumerable<TPagingModel> paginatedResults =
                data.Skip(start)
                    .Take(ResultsPerPage);

            return paginatedResults;
        }

        public void ChangePage(int page = 1)
        {
            CurrentPage = page;
        }

        public void ResetDefaults()
        {
            CurrentPage = 1;
            ResultsPerPage = 25;
        }
    }
}
