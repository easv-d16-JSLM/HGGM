using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using HGGM.Authorization;
using Microsoft.AspNetCore.Mvc;
using HGGM.Models;
using Microsoft.Extensions.Localization;

namespace HGGM.Controllers
{
    public class HomeController : Controller
    {
        private readonly IStringLocalizer<HomeController> _l;

        public HomeController(IStringLocalizer<HomeController> localizer)
        {
            _l = localizer;
        }

        public IActionResult Index()
        {
            ViewData["Message"] = _l["Welcome."];
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
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
