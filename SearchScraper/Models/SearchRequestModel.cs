using System;
using static SearchScrapper.Models.SearchEngine;

namespace SearchScrapper.Models
{
    public class SearchRequestModel
    {
        public SearchEngineType SearchEngine { get; set; }
        public string SearchText { get; set; }
        public string Keywords { get; set; }
        public string Result { get; set; }
    }
}
