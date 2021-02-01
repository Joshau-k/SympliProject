using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SearchScraper;
using SearchScrapper.Models;
using System;
using System.Diagnostics;

namespace SearchScrapper.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View("Index", new SearchRequestModel());
        }

        [HttpPost]
        public IActionResult Index(SearchRequestModel searchRequest)
        {
            try
            { 
                var finder = new KeywordSearcher(searchRequest.SearchEngine, searchRequest.SearchText);
                int location = 1 + finder.FindIndex(searchRequest.Keywords);

                searchRequest.Result = $"Found in position {location}"; ;
            
            }
            catch (NotImplementedException)
            {
                searchRequest.Result = $"{searchRequest.SearchEngine.ToString()} is not yet available";
            }
            catch (Exception)
            {
                searchRequest.Result = $"Could not perform search";
            }
            return View("Index", searchRequest);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
