using System.Collections.Generic;

namespace WebCrawler.DataScrappers
{
    public class CrawlerSettings
    {
        private List<string> PraktikpladsenKeyWords { get; set; }
        private List<string> DrKeyWords { get; set; }
        private List<string> DiscordKeyWords { get; set; }
        private List<string> HrKeyWords { get; set; }

        private int count;

        public PageDefinitions PageDefinition { get; private set; }
        public string NextKeyWord { get; private set; }

        public CrawlerSettings()
        {
            PraktikpladsenKeyWords = new List<string>() { "resultater", "defs-table" };
            DrKeyWords = new List<string>() { };
            DiscordKeyWords = new List<string>() { };
        }

        /// <summary>
        /// Gets the next key word in the list 
        /// Defined by PageDefinition
        /// </summary>
        /// <returns></returns>
        public string GetNextKeyWord()
        {
            var keyWords = GetKeyWordsForPage();
            count = keyWords.IndexOf(NextKeyWord);
            NextKeyWord = keyWords[count];
            return keyWords[count];
        }

        /// <summary>
        /// Returns a list with all keywords to use on the page 
        /// </summary>
        /// <returns></returns>
        public List<string> GetKeyWordsForPage()
        {
            switch (PageDefinition)
            {
                case PageDefinitions.praktikpladsen:

                    if (string.IsNullOrEmpty(NextKeyWord) && !PraktikpladsenKeyWords.Contains(NextKeyWord))
                    {
                        NextKeyWord = PraktikpladsenKeyWords[0];
                    }

                    return PraktikpladsenKeyWords;

                case PageDefinitions.dr:

                    if (string.IsNullOrEmpty(NextKeyWord) && !DrKeyWords.Contains(NextKeyWord))
                    {
                        NextKeyWord = DrKeyWords[0];
                    }

                    return DrKeyWords;

                case PageDefinitions.discord:

                    if (string.IsNullOrEmpty(NextKeyWord) && !DiscordKeyWords.Contains(NextKeyWord))
                    {
                        NextKeyWord = DiscordKeyWords[0];
                    }

                    return DiscordKeyWords;

                case PageDefinitions.hr:
                    
                    if (string.IsNullOrEmpty(NextKeyWord) && !HrKeyWords.Contains(NextKeyWord))
                    {
                        NextKeyWord = HrKeyWords[0];
                    }

                    return HrKeyWords;

                default:
                    return null;
            }
        }

        /// <summary>
        /// This defines the web page name
        /// </summary>
        public enum PageDefinitions
        {
            praktikpladsen, dr, discord, hr
        }
        public void SetPageDefinitions(PageDefinitions pageDefinition)
        {
            PageDefinition = pageDefinition;
        }
    }
}