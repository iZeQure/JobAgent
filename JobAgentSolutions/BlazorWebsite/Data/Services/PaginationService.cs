using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorWebsite.Data.Services
{
    public class PaginationService : ComponentBase
    {
        public event Func<Task> OnPageChange;
        public event Func<Task> OnPageSizeChange;
        private int _pageSize = 25;
        private int _currentPage = 1;

        public int CurrentPage { get => _currentPage; set { _currentPage = value; OnPageChange?.Invoke(); } }

        public int PageSize
        {
            get => _pageSize;

            set
            {
                _currentPage = 1;
                _pageSize = value;
                OnPageSizeChange?.Invoke();
            }
        }

        public int TotalItems { get; set; }

        public int MaxPages { get; private set; }


        private int PageCount<TPagingModel>(IEnumerable<TPagingModel> data)
        {
            if (data is null)
            {
                return 0;
            }

            double smallestValue = Math.Ceiling(data.Count() / (double)_pageSize);

            return Convert.ToInt32(smallestValue);
        }

        public IEnumerable<TPagingModel> CreatePaging<TPagingModel>(IEnumerable<TPagingModel> data)
        {
            TotalItems = data.Count();
            MaxPages = PageCount(data);
            int start = (_currentPage - 1) * _pageSize;

            IEnumerable<TPagingModel> paginatedResults =
                data.Skip(start)
                    .Take(_pageSize);

            return paginatedResults;
        }

        public void ChangePage(int page = 1)
        {
            if (_currentPage < 1)
            {
                CurrentPage = 1;
                return;
            }

            if (_currentPage > MaxPages)
            {
                CurrentPage = MaxPages;
                return;
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
