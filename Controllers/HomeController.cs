using System.Diagnostics;
using Transport_system_prototype.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Transport_system_prototype.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly context data;

        public HomeController(ILogger<HomeController> logger, context db)
        {
            data = db;
            _logger = logger;
        }

        public IActionResult Index()
        {
            ViewBag.Stations = data.Stations.ToList();
            return View(data.Trips
                .Include(t => t.vehicle)
                .ToList());
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
