using System;
using HtmlAgilityPack;
using System.Collections.Generic;
using NUnit;
using NUnit.Framework;
using System.Linq;

namespace SearchScraper
{
    public interface ISearchResultsOrganiser
    {
        public List<string> FindResults(string searchText);
    }
}