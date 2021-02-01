using System;
using HtmlAgilityPack;
using System.Collections.Generic;
using NUnit;
using NUnit.Framework;
using System.Linq;
using System.Net;
using System.IO;
using System.Threading;

namespace SearchScraper
{
    public class WebCache
    {
        string cacheFile = "cache.txt";
        int timeoutmilliseconds = 1000 * 60 * 60;
        public WebCache()
        {

        }

        public WebCache(int milliseconds)
        {
            timeoutmilliseconds = milliseconds;
        }

        public void AddEntry(string fullurl, string html)
        {
            fullurl = System.Web.HttpUtility.UrlEncode(fullurl);
            html = System.Web.HttpUtility.UrlEncode(html);
            var newValue = (html, DateTime.Now);

            Dictionary<string, (string, DateTime)> cache = GetCache();
            if (cache.ContainsKey(fullurl))
                cache[fullurl] = newValue;
            else
                cache.Add(fullurl, newValue);
            File.WriteAllLines(cacheFile, cache.Select(x => $"{x.Key}|{x.Value.Item1}|{x.Value.Item2}"));
        }

        private Dictionary<string, (string value, DateTime time)> GetCache()
        {
            if (!File.Exists(cacheFile))
                File.WriteAllText(cacheFile, "");

            Dictionary<string, (string, DateTime)> dict = new Dictionary<string, (string, DateTime)>();
            foreach (string line in File.ReadLines(cacheFile))
            {
                string key = line.Split("|")[0];
                string value = line.Split("|")[1];
                DateTime time = DateTime.Parse(line.Split("|")[2]);
                dict.Add(key, (value, time));
            }

            return dict;
        }

        public string GetEntry(string key)
        {
            key = System.Web.HttpUtility.UrlEncode(key);
            var dict = GetCache();
            if (dict.ContainsKey(key) && dict[key].time.AddMilliseconds(timeoutmilliseconds) > DateTime.Now)
                return System.Web.HttpUtility.UrlDecode(dict[key].value);
            return null;
        }

        public void Clear()
        {
            File.Delete(cacheFile);
        }
        
    }
}

namespace SearchScraper.Test
{

    public class WebCacheTest
    {
        [Test]
        public void TestGetCache()
        {
            File.WriteAllText("cache.txt", $"key|value|{DateTime.Now.AddDays(1)}");

            string html = new WebCache().GetEntry("key");
            Assert.That(html, Is.EqualTo("value"));
        }

        [Test]
        public void TestSetGetCache()
        {
            var cache = new WebCache();
            cache.AddEntry("key1", "html1");
            cache.AddEntry("key4", "html2");
            cache.AddEntry("key5", "html6");
            Assert.That(cache.GetEntry("key1"), Is.EqualTo("html1"));
            Assert.That(cache.GetEntry("key4"), Is.EqualTo("html2"));
            Assert.That(cache.GetEntry("key5"), Is.EqualTo("html6"));
        }

        [Test]
        public void TestSetCacheTwice()
        {
            var cache = new WebCache();
            cache.AddEntry("key1", "html1");
            cache.AddEntry("key1", "html2");
            Assert.That(cache.GetEntry("key1"), Is.EqualTo("html2"));
        }

        [Test]
        public void TestCacheEntryMissing()
        {
            var cache = new WebCache();
            cache.AddEntry("key1", "html1");
            Assert.That(cache.GetEntry("key2"), Is.Null);
        }

        [Test]
        public void TestCacheTimeout()
        {
            var timeoutMillis = 1000;
            var cache = new WebCache(timeoutMillis);
            cache.AddEntry("key1", "html1");
            Assert.That(cache.GetEntry("key1"), Is.EqualTo("html1"));
            Thread.Sleep(timeoutMillis + 100);
            Assert.That(cache.GetEntry("key1"), Is.Null);
        }

        [Test]
        public void TestCacheWorksWithUrlAndHtml()
        {
            var cache = new WebCache();
            string url = "http://google.com/search?q=e-settlements";
            string html = @"<!DOCTYPE html>
<html>
<body>

<h1>My First | Heading</h1>
<p>My first paragraph.</p>

</body>
</html>";
            cache.AddEntry(url, html);
            Assert.That(cache.GetEntry(url), Is.EqualTo(html));
        }

        [Test]
        public void TestCacheWorksWithNewLineAndCarriageReturn()
        {
            var cache = new WebCache();
            string url = "http://google.com/search?q=e-settlements";
            string html = String.Format(@"<!DOCTYPE html>
<html>
<body>

<h1>My First | Heading</h1>
<p>My first paragraph.</p>
{0}
{1}
{2}
</body>
</html>", System.Environment.NewLine, "\r", "\n");
            cache.AddEntry(url, html);
            Assert.That(cache.GetEntry(url), Is.EqualTo(html));
        }

        [Test]
        public void TestCacheWorksWithTestHtmlDoc()
        {
            TextReader tr = new StreamReader(@"TestSearchResult.txt");
            string html = tr.ReadToEnd();

            var cache = new WebCache();
            string url = "http://google.com/search?q=e-settlements";
            cache.AddEntry(url, html);
            Assert.That(cache.GetEntry(url), Is.EqualTo(html));
        }

        //

        [SetUp]
        public void Setup()
        {
            var cache = new WebCache();
            cache.Clear();
        }

        [TearDown]
        public void Teardown()
        {
            var cache = new WebCache();
            cache.Clear();
        }
    }
}