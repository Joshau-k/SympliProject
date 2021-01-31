using System;
using System.Net;

namespace SearchScraper
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Welcome to google search result checker.");
                Console.Write("Search text:");
                string search = Console.ReadLine();
                Console.Write("Result to look for:");
                string keyphrase = Console.ReadLine();
                int index = new FinderOfKeywordIndexInGoogleSearch(search).FindIndex(keyphrase);
                if (index == -1)
                    Console.WriteLine($"{keyphrase} not found");
                else
                    Console.WriteLine($"{keyphrase} found in search result {index + 1}");
                Console.ReadLine();
            }
            catch (WebException ex)
            {
                Console.WriteLine("Error: Google didn't like all the spam");
                Console.ReadLine();
            }
        }
    }
}
