using System;
using System.Collections.Generic;
using System.Linq;
using WebCrawler.DataScrappers;
using static WebCrawler.DataScrappers.CrawlerSettings;

namespace WebCrawler.DataSorters
{
    public class UrlCutter
    {
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
