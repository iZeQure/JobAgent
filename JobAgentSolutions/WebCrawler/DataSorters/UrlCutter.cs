using System;
using System.Collections.Generic;
using System.Linq;
using WebCrawler.DataScrappers;
using static WebCrawler.DataScrappers.CrawlerSettings;

namespace WebCrawler.DataSorters
{
    public class UrlCutter
    {
        static char[] urlSymbolsToRemove = { '/', '.' };
        public static PageDefinitions GetPageDefinitionFromUrl(string url)
        {
            string result = null;
            // Splits the url into an array with base url components
            var urlArray = url.Split(urlSymbolsToRemove[0]);

            // Goes through the array to find the base fx  www.[google].com = google 
            foreach (var item in urlArray)
            {
                // Split the item to see with position the base url has 
                if (item.Split(urlSymbolsToRemove[1]).Length > 1)
                {
                    // Uses an enum with fixed base values to see if the base url is known by the crawler
                    foreach (var pageDefinition in Enum.GetNames(typeof(PageDefinitions)))
                    {
                        // Split to find the base url 
                        // so if the string array contains the page definition it will return it
                        result = item.Split(urlSymbolsToRemove[1]).FirstOrDefault(urlbase => urlbase.Contains(pageDefinition));
                        if(result is not null)
                        {
                            return Enum.GetValues(typeof(PageDefinitions)).Cast<PageDefinitions>().FirstOrDefault(x => x.ToString().Contains(result));
                        }

                    }
                }
            }

            throw new Exception("Nothing found");
        }

        public static string GetBaseUrl(string url)
        {
            // https://www.praktikpladsen.dk/

            var splitedUrlString = url.Split('/');
            var result = "";
            foreach (var item in splitedUrlString)
            {
                if (item.Contains("https:") || item.Length >= 2)
                { 
                    if(result.Length == 0)
                    {
                        result += item + "//";
                    }
                    else
                    {
                        result += item;
                    }
                    
                    if (item.EndsWith("dk") || item.EndsWith("com"))
                    {
                        return result;
                    }
                }
            }

            return result;
        }

        public static bool CheckIfLinkLeadsToAFile(string url)
        {
            if (url.Split('?')[0].EndsWith("xlsx")) 
            {
                return true;
            }

            return false;
        }

        public static bool CheckIfLinkExist(string url)
        {
            if (DoesLinkHaveProtocol(url) && DoesUrlHaveDomain(url))
            {
                return true;
            }

            return false;
        } 

        public static bool DoesLinkHaveProtocol(string url)
        {
            var urlString = url.Split('/');
            foreach (var item in urlString)
            {
                if(item.Contains("https:") || item.Contains("http:"))
                {
                    return true;
                }
            }

            return false;
        }

        public static bool DoesUrlHaveDomain(string url)
        {
            var baseUrl = GetBaseUrl(url);

            if (baseUrl.EndsWith("dk") || baseUrl.EndsWith("com"))
            {
                return true;
            }

            return false;
        }

        public static bool CheckIfUrlPathValid(string url)
        {
            if (url.StartsWith("/"))
            {
                if(!CheckIfLinkLeadsToAFile(url))
                {
                    if (!DoesLinkHaveProtocol(url))
                    {
                        return true;
                    }
                }

            }
            return false;

        }

        public static List<string> SortUrlPaths(List<string> urlsToSort)
        {
            List<string> result = urlsToSort.ToList();

            foreach (var item in urlsToSort)
            {
                if (!CheckIfUrlPathValid(item))
                {
                    result.Remove(item);
                }   
            }

            return result;
        }
    }
}
