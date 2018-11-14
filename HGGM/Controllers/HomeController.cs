using System.Diagnostics;
using HGGM.Models;
using HGGM.Services;
using Microsoft.AspNetCore.Mvc;

namespace HGGM.Controllers
{
    public class HomeController : Controller
    {
        private readonly MarkdownService _markdown;

        public HomeController(MarkdownService markdown)
        {
            _markdown = markdown;
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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }

        public IActionResult Index()
        {
            ViewData["Message"] = _markdown.ToHtml("- [x] Markdown works!");
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}