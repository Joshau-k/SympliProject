using System;
using HtmlAgilityPack;
using System.Collections.Generic;
using NUnit;
using NUnit.Framework;
using System.Linq;
using System.IO;

namespace SearchScraper
{
    public class FinderOfKeywordsInSearchResults
    {
        List<string> results;
        public FinderOfKeywordsInSearchResults(string html, string searchtext)
        {
            var organiser = new SearchResultsOrganiser(html);
            results = organiser.FindResults(searchtext);
        }

        public int FindKeyphraseFirstAppearanceIndex(string keyphrase)
        {
            for (int i = 0; i < results.Count; ++i)
            {
                if (results[i].Contains(keyphrase))
                    return i;
            }
            return -1;
        }

        public int ResultCount()
        {
            return results.Count;
        }
    }
}

namespace SearchScraper.Test
{

    public class FinderOfKeywordsInSearchResultsTest
    {
        [Test]
        public void TestFindKeyphraseFirstAppearanceIndex()
        {
            string html = @"<div data-async-context=""query:heygooglehowdoItypespaces?""><div class=""g""> result 1 </div><div class=""g""> result 2 </div><div class=""g""> result 3 </div><div class=""g""> result 4 </div><div class=""g""> result 5 </div></div>";
            var finder = new FinderOfKeywordsInSearchResults(html, "heygooglehowdoItypespaces?");
            int location = finder.FindKeyphraseFirstAppearanceIndex("result 3");
            Assert.That(location, Is.EqualTo(2));
        }

        [Test]
        public void TestFindKeyphraseFirstAppearanceIndexWithSpaces()
        {
            string html = @"<div data-async-context=""query:does%20this%20work?""><div class=""g""> result 1 </div><div class=""g""> result 2 </div><div class=""g""> result 3 </div><div class=""g""> result 4 </div><div class=""g""> result 5 </div></div>";
            var finder = new FinderOfKeywordsInSearchResults(html, "does this work?");
            int location = finder.FindKeyphraseFirstAppearanceIndex("result 5");
            Assert.That(location, Is.EqualTo(4));
        }

        [Test]
        public void TestActualData()
        {
            TextReader tr = new StreamReader(@"TestSearchResult.txt");
            string html = tr.ReadToEnd();
            var finder = new FinderOfKeywordsInSearchResults(html, "e-settlements");
            int location = finder.FindKeyphraseFirstAppearanceIndex("sympli");
            Assert.That(location, Is.EqualTo(2));
        }

        [Test]
        public void TestFindingUrlWorks()
        {
            string html = @"<div data-async-context=""query:heygooglehowdoItypespaces?""><div class=""g""> result 1 </div><div class=""g""> www.sympli.com.au </div><div class=""g""> result 3 </div><div class=""g""> result 4 </div><div class=""g""> result 5 </div></div>";
            var finder = new FinderOfKeywordsInSearchResults(html, "heygooglehowdoItypespaces?");
            int location = finder.FindKeyphraseFirstAppearanceIndex("www.sympli.com.au");
            Assert.That(location, Is.EqualTo(1));
        }
    }
}