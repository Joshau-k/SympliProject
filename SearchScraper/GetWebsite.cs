using System;
using HtmlAgilityPack;
using System.Collections.Generic;
using NUnit;
using NUnit.Framework;
using System.Linq;
using System.Net;
using System.IO;

namespace SearchScraper
{
    public static class GetWebsite
    {
        public static string GetHtml(string url, params (string,string)[] Params)
        {
            string fullUrl = url;
            foreach ((string name, string value) in Params)
            {
                if (url == fullUrl)
                    fullUrl += "?";
                else
                    fullUrl += "&";
                fullUrl += $"{name}={value}";
            }

            //fullUrl = Uri.EscapeUriString(fullUrl);
            fullUrl = new UriBuilder(fullUrl).Uri.ToString();
            // fullUrl = @"https://api.stackexchange.com/2.2/answers?order=desc&sort=activity&site=stackoverflow"; 
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(fullUrl);
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            string userAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/87.0.4280.88 Safari/537.36";
            request.Headers.Add("User-Agent", userAgent);
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }

        
    }
}

namespace SearchScraper.Test
{

    public class GetWebsiteTest
    {
        [Test]
        public void TestGetHtml()
        {
            var html = GetWebsite.GetHtml("google.com/search", ("q", "e-settlements"));
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);

            Assert.That(doc.ParseErrors, Has.Count.EqualTo(0));
        }
    }
}