using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SearchScrapper.Models
{
    public class SearchEngine
    {
        public enum SearchEngineType { Google = 1, Bing = 2 }
        public static List<SearchEngineType> SearchEngineList { get { return Enum.GetValues(typeof(SearchEngineType)).Cast<SearchEngineType>().ToList(); } }
    }
}
