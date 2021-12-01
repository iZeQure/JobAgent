using HtmlAgilityPack;
using JobAgentClassLibrary.Common.JobAdverts.Entities;
using JobAgentClassLibrary.Common.JobPages.Entities;
using System.Collections.Generic;

namespace WebCrawler.DataSorters
{
    public class HtmlSorter : IHtmlSorter
    {
        /// <summary>
        /// Returns a string array of a htmlpage 
        /// Splits the html on <
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        public string[] GetHtmlArray(HtmlDocument document)
        {
            // Doing a split for easier readability
            return document.Text.Split('<'); // returns a string array from the split
        }

        /// <summary>
        /// This method is used to split up a HtmlArray with a given char
        /// </summary>
        /// <param name="startsWith"></param>
        /// <param name="htmlArray"></param>
        /// <returns></returns>
        public IEnumerable<string[]> HtmlArraySplitOn(char startsWith, string[] htmlArray)
        {
            List<string[]> data = new List<string[]>();
            GetLinksFromDocument(htmlArray);

            foreach (var item in htmlArray)
            {
                if (item.Length > 5 && item.StartsWith(startsWith))
                    data.Add(item.Split('>'));
            }

            return data;
        }

        /// <summary>
        /// This method will check the htmlArray for a tags to find links
        /// </summary>
        /// <param name="htmlArray"></param>
        public List<string> GetLinksFromDocument(string[] htmlArray)
        {
            List<string> linksFromSite = new List<string>();
            foreach (var item in htmlArray)
            {
                if (item.Length > 7 && item.StartsWith('a'))
                {
                    var link = item.Split('"')[1];
                    linksFromSite.Add(link);
                }
            }
            return linksFromSite;
        }
        
        /// <summary>
        /// Should only return list with job posts
        /// </summary>
        /// <param name="urls"></param>
        /// <returns></returns>
        public List<string> GetJobListFromPraktikpladsen(List<string> urls)
        {
            List<string> jobList = new List<string>();
            return jobList;
        }

        /// <summary>
        /// Should only return job pages found
        /// </summary>
        /// <param name="urls"></param>
        /// <returns></returns>
        public List<string> GetJobPagesFromPraktikpladsen(List<string> urls)
        {
            List<string> jobPages = new List<string>();
            return jobPages;
        }
    }
}
