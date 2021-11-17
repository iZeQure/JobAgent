﻿using HtmlAgilityPack;
using System.Collections.Generic;

namespace WebCrawler.DataSorters
{
    public interface IHtmlSorter
    {
        public List<string> LinksFromSite { get; }
        public string[] GetHtmlArray(HtmlDocument document);
        public IEnumerable<string[]> HtmlArraySplitOn(char startsWith, string[] htmlArray);
        public List<string> GetLinksFromDocument(string[] htmlArray);
    }
}