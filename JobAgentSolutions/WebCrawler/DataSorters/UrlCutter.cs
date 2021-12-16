using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WebCrawler.DataAccess;

namespace WebCrawler.DataSorters
{
    public class UrlCutter
    {
        /// <summary>
        /// Returns a new list with the sorted job links 
        /// No dublicates is returned 
        /// </summary>
        /// <param name="linksToSort"></param>
        /// <returns></returns>
        public static List<string> GetJobLinks(List<string> linksToSort)
        {
            List<string> sortedList = new();
            string[] splitedLink;

            foreach (var item in linksToSort)
            {
                splitedLink = item.Split('/');
                foreach (var part in splitedLink)
                {
                    // checks if there is a (vis) reference 
                    // /vis is before all links to the jobpage on praktikpladsen
                    if (part.StartsWith("vis"))
                    {
                        sortedList.Add(item);
                    }
                }
            }
            // Checks for dublicates 
            return CheckListForDublicates(sortedList);
        }

        /// <summary>
        /// Makes a new list with links that is not dublicated 
        /// </summary>
        /// <param name="links"></param>
        /// <returns></returns>
        public static List<string> CheckListForDublicates(List<string> links)
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

        /// <summary>
        /// Only returns the links that matches the main path to job link list on praktikpladsen
        /// </summary>
        /// <param name="links"></param>
        /// <returns></returns>
        public static List<string> GetLinkLists(List<string> links)
        {
            List<string> sortedList = new();
            string[] splitedLink;
            foreach (var item in links)
            {
                splitedLink = item.Split('/');

                foreach (var part in splitedLink)
                {
                    bool isInt = false;
                    if (splitedLink.Length > 4)
                    {
                        isInt = int.TryParse(splitedLink[4], out _);

                    }

                    if (part.StartsWith("soeg") &&  isInt == true )
                    {
                        sortedList.Add(item);
                    }
                }
            }
            return CheckListForDublicates(sortedList);
        }
    }
}
