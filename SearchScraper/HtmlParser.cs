using System;
using HtmlAgilityPack;
using System.Collections.Generic;
using NUnit;
using NUnit.Framework;
using System.Linq;

namespace SearchScraper
{
    public class HtmlParser
    {
        HtmlDocument doc;
        public HtmlParser(string html)
        {
            doc = new HtmlDocument();
            doc.LoadHtml(html);
        }

        public List<HtmlNode> GetNodesByProperty(string name, string value)
        {
            return doc.DocumentNode.SelectNodes($"//div[@{name}='{value}']")?.AsEnumerable()?.ToList() ?? new List<HtmlNode>();
        }
       
    }
}

namespace SearchScraper.Test
{

    public class HtmlParserTest
    {
        [Test]
        public void TestCreateParser()
        {
            var parser = new HtmlParser("<html></html>");
            Assert.Pass();
        }

        [Test]
        public void TestGetNodeByClass()
        {
            var parser = new HtmlParser("<html><body><div class='foo'>text</div></body></html>");
            var nodes = parser.GetNodesByProperty("class", "foo");
            Assert.That(nodes, Has.Count.EqualTo(1));
            var node = nodes.Single();
            Assert.That(node.InnerHtml, Is.EqualTo("text"));
            Assert.That(node.OuterHtml, Is.EqualTo("<div class='foo'>text</div>"));
        }

        [Test]
        public void TestGetMultipleNodesByClass()
        {
            var parser = new HtmlParser("<html><body><div class='foo'>text</div><div class='foo'>text2</div></body></html>");
            var nodes = parser.GetNodesByProperty("class", "foo");
            Assert.That(nodes, Has.Count.EqualTo(2));
            var node1 = nodes.First();
            Assert.That(node1.InnerHtml, Is.EqualTo("text"));
            Assert.That(node1.OuterHtml, Is.EqualTo("<div class='foo'>text</div>"));
            var node2 = nodes[1];
            Assert.That(node2.InnerHtml, Is.EqualTo("text2"));
            Assert.That(node2.OuterHtml, Is.EqualTo("<div class='foo'>text2</div>"));
        }

        [Test]
        public void TestGetNodesByClassWithMoreInside()
        {
            var parser = new HtmlParser("<html><body><div class='foo'><p>test</p></div><div class='foo'><p>test2</p></div></body></html>");
            var nodes = parser.GetNodesByProperty("class", "foo");
            Assert.That(nodes, Has.Count.EqualTo(2));
            var node1 = nodes.First();
            Assert.That(node1.InnerHtml, Is.EqualTo("<p>test</p>"));
            Assert.That(node1.OuterHtml, Is.EqualTo("<div class='foo'><p>test</p></div>"));
            var node2 = nodes[1];
            Assert.That(node2.InnerHtml, Is.EqualTo("<p>test2</p>"));
            Assert.That(node2.OuterHtml, Is.EqualTo("<div class='foo'><p>test2</p></div>"));
        }


    }
}