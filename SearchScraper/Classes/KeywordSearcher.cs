using System;
using HtmlAgilityPack;
using System.Collections.Generic;
using NUnit;
using NUnit.Framework;
using System.Linq;
using static SearchScrapper.Models.SearchEngine;

namespace SearchScraper
{
    public class KeywordSearcher
    {
        static string googleBaseUrl = "google.com/search";
        static string googleSearchParamName = "q";
        static string bingBaseUrl = "bing.com/search";
        static string bingSearchParamName = "q";
        FinderOfKeywordsInSearchResults searchResultFinder;

        public KeywordSearcher(SearchEngineType searchEngine, string searchText)
        {
            ISearchResultsOrganiser organiser;
            if (searchEngine == SearchEngineType.Google)
            {
                string html = GetWebsite.GetHtml(googleBaseUrl, (googleSearchParamName, searchText), ("num", "99"));
                organiser = new GoogleSearchResultsOrganiser(html);
            }
            else if (searchEngine == SearchEngineType.Bing)
            {
                string html = GetWebsite.GetHtml(bingBaseUrl, (bingSearchParamName, searchText), ("count", "100"));
                organiser = new BingSearchResultsOrganiser(html);
            }
            else
            {
                throw new Exception("No Search Engine Selected");
            }
            searchResultFinder = new FinderOfKeywordsInSearchResults(organiser, searchText);
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
            var index = new KeywordSearcher(SearchEngineType.Google, "e-settlements").FindIndex("sympli");
            Assert.Pass();
        }

        [Test]
        public void Test100ResultsFound()
        {
            var count = new KeywordSearcher(SearchEngineType.Google, "googling google").ResultCount();
            Assert.That(count, Is.EqualTo(100));
        }

        [Test]
        public void Test100ResultsFoundForBing()
        {
            var count = new KeywordSearcher(SearchEngineType.Bing, "binging bing").ResultCount();
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