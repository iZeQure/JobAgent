using System;
using System.Linq;
using WebCrawler.DataScrappers;
using static WebCrawler.DataScrappers.CrawlerSettings;

namespace WebCrawler.DataSorters
{
    public static class UrlCutter
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
                            return Enum.GetNames(typeof(PageDefinitions)).Cast<PageDefinitions>().FirstOrDefault(x => x.ToString().Contains(result));
                        }

                    }
                }
            }

            throw new Exception("Nothing found");
        }
    }
}
