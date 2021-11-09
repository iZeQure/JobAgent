using BlazorServerWebsite.Data.Services.Abstractions;
using ObjectLibrary.Common;
using SecurityLibrary.Providers;
using SqlDataAccessLibrary.Repositories.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BlazorServerWebsite.Data.Services
{
    public class MenuService : IMenuService
    {
        private readonly ICategoryRepository categoryRepository;
        private readonly ISpecializationRepository specializationRepository;

        public MenuService(ICategoryRepository categoryRepository, ISpecializationRepository specializationRepository)
        {
            this.categoryRepository = categoryRepository;
            this.specializationRepository = specializationRepository;
        }

        public async Task<IEnumerable<Category>> InitializeMenu(CancellationToken cancellation)
        {
            var categoriesTask = categoryRepository.GetAllAsync(cancellation);
            var specializationsTask = specializationRepository.GetAllAsync(cancellation);

            try
            {
                await TaskExtProvider.WhenAll(categoriesTask, specializationsTask);
            }
            catch (Exception)
            {
                throw;
            }

            var categories = categoriesTask.Result;
            var specializations = specializationsTask.Result;
            FindMatches(categories, specializations);

            return await Task.FromResult(categories);
        }

        private static void FindMatches(IEnumerable<Category> categories, IEnumerable<Specialization> specializations)
        {
            foreach (Category category in categories)
            {
                foreach (Specialization specialization in specializations)
                {
                    if (category.Id == specialization.Category.Id)
                    {
                        category.AssignSpecialization(specialization);
                    }
                }
            }
        }

        //private void GetCounts()
        //{
        //    try
        //    {
        //        foreach (Category category in _categories)
        //        {
        //            categoryDict.Add(category.Name, JobAdvertService.GetJobAdvertCountByCategoryId(category.Id, _tokenSource.Token));
        //        }
        //        _countByUncategorized = JobAdvertService.GetJobAdvertCountByUncategorized(_tokenSource.Token);

        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Sorting Error : {ex.Message}");
        //    }

        //}
    }
}
