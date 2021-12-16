﻿using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WebCrawler.DataAccess;

namespace WebCrawler.DataSorters
{
    public class UrlCutter
    {
        //public static string GetBaseUrl(string url)
        //{
        //    // https://www.praktikpladsen.dk/

        //    var splitedUrlString = url.Split('/');
        //    var result = "";
        //    foreach (var item in splitedUrlString)
        //    {
        //        if (item.Contains("https:") || item.Length >= 2)
        //        {
        //            if (result.Length == 0)
        //            {
        //                result += item + "//";
        //            }
        //            else
        //            {
        //                result += item;
        //            }

        //            if (item.EndsWith("dk") || item.EndsWith("com"))
        //            {
        //                return result;
        //            }
        //        }
        //    }

        //    return result;
        //}

        //public static bool CheckIfLinkLeadsToAFile(string url)
        //{
        //    if (url.Split('?')[0].EndsWith("xlsx"))
        //    {
        //        return true;
        //    }

        //    return false;
        //}

        //public static bool CheckIfLinkExist(string url)
        //{
        //    if (DoesLinkHaveProtocol(url) && DoesUrlHaveDomain(url))
        //    {
        //        return true;
        //    }

        //    return false;
        //}

        //public static bool DoesLinkHaveProtocol(string url)
        //{
        //    var urlString = url.Split('/');
        //    foreach (var item in urlString)
        //    {
        //        if (item.Contains("https:") || item.Contains("http:"))
        //        {
        //            return true;
        //        }
        //    }

        //    return false;
        //}

        //public static bool DoesUrlHaveDomain(string url)
        //{
        //    var baseUrl = GetBaseUrl(url);

        //    if (baseUrl.EndsWith("dk") || baseUrl.EndsWith("com"))
        //    {
        //        return true;
        //    }

        //    return false;
        //}

        //public static bool CheckIfUrlPathValid(string url)
        //{
        //    if (url.StartsWith("/"))
        //    {
        //        if (!CheckIfLinkLeadsToAFile(url))
        //        {
        //            if (!DoesLinkHaveProtocol(url))
        //            {
        //                return true;
        //            }
        //        }

        //    }
        //    return false;

        //}

        //public static List<string> SortUrlPaths(List<string> urlsToSort)
        //{
        //    List<string> result = urlsToSort.ToList();

        //    foreach (var item in urlsToSort)
        //    {
        //        if (!CheckIfUrlPathValid(item))
        //        {
        //            result.Remove(item);
        //        }
        //    }

        //    return result;
        //}

        //public async Task<string> GetCategoryFromLink(string url)
        //{
        //    string result = "";
            
        //    var categories = await dbCommunicator.GetCategoriesAsync();

        //    foreach (var item in url.Split('/'))
        //    {
        //        var c = item.Split('%');
        //        foreach (var str in c)
        //        {

        //            if (str.StartsWith("20") || str.StartsWith("Data"))
        //            {
        //                if (!str.StartsWith("Data"))
        //                {
        //                    var testString = str.Remove(0, 2);
        //                    if (testString.Contains("kommunikationsuddannelsen"))
        //                    {
        //                        result += str.Remove(0, 2);
        //                        if (categories.FirstOrDefault(x => x.Name == result).Name is not null)
        //                        {
        //                            return result;
        //                        }
        //                    }
        //                    else
        //                    {
        //                        result += str.Remove(0, 2) + " ";
        //                    }
        //                }
        //                else
        //                {
        //                    result = str + " ";
        //                }
        //            }

        //            if (result.Contains("kommunikationsuddannelsen"))
        //            {
        //                Debug.WriteLine("Found");
        //            }

        //        }

        //    }

        //    return result;
        //}

        //public static string GetSpecialization(string url)
        //{
        //    string result = "";
        //    foreach (var item in url.Split('/'))
        //    {

        //    }

        //    return result;
        //}

        /// <summary>
        /// Returns a new list with the sorted links
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
                    if (part.StartsWith("vis"))
                    {
                        sortedList.Add(item);
                    }
                }
            }
            
            return CheckListForDublicates(sortedList);
        }

        public static List<string> CheckListForDublicates(List<string> links)
        {
            List<string> sortedList = new();
            foreach (var item in links)
            {
                if (!sortedList.Contains(item) && !item.EndsWith("main-content"))
                {
                    sortedList.Add(item);
                }
            }

            return sortedList;
        }

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
