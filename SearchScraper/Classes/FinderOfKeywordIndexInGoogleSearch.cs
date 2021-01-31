using System;
using HtmlAgilityPack;
using System.Collections.Generic;
using NUnit;
using NUnit.Framework;
using System.Linq;

namespace SearchScraper
{
    public class FinderOfKeywordIndexInGoogleSearch
    {
        static string baseUrl = "google.com/search";
        static string searchParamName = "q";
        FinderOfKeywordsInSearchResults searchResultFinder;

        public FinderOfKeywordIndexInGoogleSearch(string searchText)
        {
            string html = GetWebsite.GetHtml(baseUrl, (searchParamName, searchText), ("num", "99"));
            searchResultFinder = new FinderOfKeywordsInSearchResults(html, searchText);
        }
        public int FindIndex(string keywordText)
        {
            return searchResultFinder.FindKeyphraseFirstAppearanceIndex(keywordText);
        }   

        public int ResultCount()
        {
            return searchResultFinder.ResultCount();
        }
    }
}


namespace SearchScraper.Test
{

    public class FinderOfKeywordIndexInGoogleSearchTest
    {
        [Test]
        public void TestFindKeyphraseFirstAppearanceIndex_DoesNotThrowException()
        {
            var index = new FinderOfKeywordIndexInGoogleSearch("e-settlements").FindIndex("sympli");
            Assert.Pass();
        }

        [Test]
        public void Test100ResultsFound()
        {
            var count = new FinderOfKeywordIndexInGoogleSearch("googling google").ResultCount();
            Assert.That(count, Is.EqualTo(100));
        }

        [SetUp]
        public void Setup()
        {
            var cache = new WebCache();
            cache.Clear();
        }

    }
}