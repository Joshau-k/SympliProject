using System.Collections.Generic;

namespace SearchScraper
{
    public interface ISearchResultsOrganiser
    {
        public List<string> FindResults(string searchText);
    }
}