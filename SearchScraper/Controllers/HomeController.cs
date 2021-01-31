using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SearchScraper;
using SearchScrapper.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

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
            if (searchRequest.SearchEngine == SearchEngine.SearchEngineType.Google)
            {
                var finder = new FinderOfKeywordIndexInGoogleSearch(searchRequest.SearchText);
                int location = 1 + finder.FindIndex(searchRequest.Keywords);

                searchRequest.Result = $"Found in position {location}"; ;
            }
            else if (searchRequest.SearchEngine == 0)
            {

            }
            else
            {
                searchRequest.Result = $"{searchRequest.SearchEngine.ToString()} is not yet available";
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
