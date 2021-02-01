using System;
using System.Collections.Generic;
using NUnit.Framework;
using System.Linq;

namespace SearchScraper
{
    public class GoogleSearchResultsOrganiser : ISearchResultsOrganiser
    {
        HtmlParser parser;
        public GoogleSearchResultsOrganiser(string html)
        {
            parser = new HtmlParser(html);
        }

        public List<string> FindResults(string  searchText)
        {
            var urlSearchText = Uri.EscapeUriString(searchText);
            var results = parser.GetNodesByProperty("div", "data-async-context", $"query:{urlSearchText}");
            return results.SelectMany(x => x.ChildNodes.Where(x => x.Name == "div").Select(y=> y.OuterHtml)).ToList();
        }
     
    }
}

namespace SearchScraper.Test
{

    public class GoogleSearchResultsOrganiserTest
    {
        [Test]
        public void TestGetGoogleOrganicResults()
        {
            string html = @"<div data-async-context=""query:heygooglehowdoItypespaces?""><div class=""g""> result 1 </div><div class=""g""> result 2 </div><div class=""g""> result 3 </div><div class=""g""> result 4 </div><div class=""g""> result 5 </div></div>";
            var resultsOrganiser = new GoogleSearchResultsOrganiser(html);
            var results = resultsOrganiser.FindResults("heygooglehowdoItypespaces?");
            Assert.That(results, Has.Count.EqualTo(5));
            Assert.That(results.First(), Is.EqualTo(@"<div class=""g""> result 1 </div>"));
            Assert.That(results[1], Is.EqualTo(@"<div class=""g""> result 2 </div>"));
        }
    }
}