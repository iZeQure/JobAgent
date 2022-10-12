using JobAgentClassLibrary.Common.Categories.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WebCrawler.DataSorters
{
    public class UrlCutter
    {
        public static DateTime GetDateFromstring(string dataString)
        {
            var dataArray = dataString.Split(' ');
            DateTime date;
            foreach (var line in dataArray)
            {
                if (DateTime.TryParse(line, out date))
                {
                    date = DateTime.Parse(line);
                    return date;
                }
            }

            return DateTime.MinValue;
        }

        /// <summary>
        /// This method uses the string extention StartsWith to determine if it contains the category
        /// </summary>
        /// <param name="url"></param>
        /// <param name="categories"></param>
        /// <returns></returns>
        private static string TestStartsWith(string url, List<ICategory> categories)
        {
            var stringArray = url.Split('/');
            foreach (var category in categories)
            {
                var subString = category.Name.Substring(0, 4);
                var test = stringArray.FirstOrDefault(x => x.ToLower().StartsWith(subString.ToLower()));
                if (test is not null)
                {
                    return category.Name;
                }
            }

            return null;
        }

        /// <summary>
        /// This method uses the string extention EndsWith to determine if it contains the category
        /// </summary>
        /// <param name="url"></param>
        /// <param name="categories"></param>
        /// <returns></returns>
        private static string TestEndsWith(string url, List<ICategory> categories)
        {
            var stringArray = url.Split('/');
            foreach (var category in categories)
            {
                var subString = category.Name.Substring(category.Name.Length - 5, 5);
                var test = stringArray.FirstOrDefault(x => x.ToLower().EndsWith(subString.ToLower()));
                if (test is not null)
                {
                    return category.Name;
                }
            }

            return null;
        }


        /// <summary>
        /// This method is used to get the category for a job
        /// Checks the url for key categories from db
        /// </summary>
        /// <param name="url"></param>
        /// <param name="categories"></param>
        /// <returns></returns>
        public static string GetCategoryFromUrl(string url, List<ICategory> categories)
        {
            string result = TestStartsWith(url, categories);
            if (result is not null)
            {
                return result;
            }
            result = TestEndsWith(url, categories);
            if (result is not null)
            {
                return result;
            }
            return null;
        }


        /// <summary>
        /// Makes a new list with links that is not duplicated 
        /// </summary>
        /// <param name="links"></param>
        /// <returns></returns>
        public static List<string> CheckListForDuplicates(List<string> links)
        {
            List<string> sortedList = new();
            foreach (var item in links)
            {
                // Checks if the item exist in the new list
                if (!sortedList.Contains(item) && !item.EndsWith("main-content"))
                {
                    // if not the link is added to sortedList
                    sortedList.Add(item);
                }
            }

            return sortedList;
        }


    }
}
