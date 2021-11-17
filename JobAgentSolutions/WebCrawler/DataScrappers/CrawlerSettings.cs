namespace WebCrawler.DataScrappers
{
    public class CrawlerSettings
    {
        public PageDefinitions PageDefinition { get; private set; }
        
        /// <summary>
        /// This defines the web page name
        /// </summary>
        public enum PageDefinitions 
        { 
            praktikpladsen, dr, discord, hr 
        }

        private PageKeyWord _keyWord;
        /// <summary>
        /// Page key word is use to categorize the search 
        /// Page definitions have their own page keys
        /// </summary>
        public enum PageKeyWord 
        {
            resultater
        }

        public string Url { get; set; } 
        public void SetPageDefinitions(PageDefinitions pageDefinition)
        {
            PageDefinition = pageDefinition;
        }

        public PageKeyWord GetPageKeyWordForPage()
        {
            switch (PageDefinition)
            {
                case PageDefinitions.praktikpladsen:

                    _keyWord = PageKeyWord.resultater;
                    break;
                
                case PageDefinitions.dr:
                    
                    break;

                case PageDefinitions.discord:
                    
                    break;
                
                case PageDefinitions.hr:
                
                    break;
                
                default:
                    break;
            }

            return _keyWord;
        }
    }
}