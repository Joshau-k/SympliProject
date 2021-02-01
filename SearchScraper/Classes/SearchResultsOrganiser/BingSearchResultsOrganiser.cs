using System;
using System.Collections.Generic;
using NUnit.Framework;
using System.Linq;

namespace SearchScraper
{
    public class BingSearchResultsOrganiser : ISearchResultsOrganiser
    {
        HtmlParser parser;
        public BingSearchResultsOrganiser(string html)
        {
            parser = new HtmlParser(html);
        }

        public List<string> FindResults(string  searchText)
        {
            var urlSearchText = Uri.EscapeUriString(searchText);
            var results = parser.GetNodesByProperty("ol", "id", "b_results");
            return results.SelectMany(x => x.ChildNodes.Where(x => x.Name == "li").Select(y=> y.OuterHtml)).ToList();
        }
     
    }
}

namespace SearchScraper.Test
{

    public class BingSearchResultsOrganiserTest
    {
        [Test]
        public void TestGetBingResults()
        {
            string html = @"<ol id=""b_results""><li class=""b_algo""> item a </li><li class=""b_algo""> item b </li><li class=""b_algo""> item c </li><li class=""b_algo""> item d </li></ol>";
            var resultsOrganiser = new BingSearchResultsOrganiser(html);
            var results = resultsOrganiser.FindResults("heybinghowdoItypespaces?");
            Assert.That(results, Has.Count.EqualTo(4));
            Assert.That(results.First(), Is.EqualTo(@"<li class=""b_algo""> item a </li>"));
            Assert.That(results[1], Is.EqualTo(@"<li class=""b_algo""> item b </li>"));
        }
    }
}