using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BlazorWebsite.Data.Services
{
    public class PaginationService : ComponentBase
    {
        public int CurrentPage { get; set; } = 1;

        public int PageSize { get; set; } = 10;

        public int TotalItems { get; set; }

        public int MaxPages { get; private set; }


        private int PageCount<TPagingModel>(IEnumerable<TPagingModel> data)
        {
            if (data is null) return 0;

            double smallestValue = Math.Ceiling(data.Count() / (double)PageSize);

            return Convert.ToInt32(smallestValue);
        }

        public IEnumerable<TPagingModel> CreatePaging<TPagingModel>(IEnumerable<TPagingModel> data)
        {
            TotalItems = data.Count();
            MaxPages = PageCount(data);
            int start = (CurrentPage - 1) * PageSize;

            IEnumerable<TPagingModel> paginatedResults =
                data.Skip(start)
                    .Take(PageSize);

            return paginatedResults;
        }

        public void ChangePage(int page = 1)
        {
            if(CurrentPage < 1)
            {
                CurrentPage = 1;
                return;
            }

            if(CurrentPage >= MaxPages)
            {

            }

            CurrentPage = page;
        }

        public void ResetDefaults()
        {
            CurrentPage = 1;
            PageSize = 25;
        }
    }
}
