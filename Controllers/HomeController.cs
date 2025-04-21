using System.Diagnostics;
using Transport_system_prototype.Models;
using Microsoft.AspNetCore.Mvc;
using Transport__system_prototype.Repository;
using Transport__system_prototype.View_Models;

namespace Transport_system_prototype.Controllers
{
    public class HomeController : Controller
    {
        
        private readonly ILogger<HomeController> _logger;
        private readonly IStationRepository stationRepo;
        private readonly ITripRepository tripRepo;

        public HomeController(ILogger<HomeController> logger,IStationRepository _stationRepo,ITripRepository _tripRepo)
        {
            _logger = logger;
            stationRepo=_stationRepo;
            tripRepo=_tripRepo;
        }

        public IActionResult Index()
        {
            StationsTripsViewModel stationsTrips = new StationsTripsViewModel
            {
                Stations= stationRepo.GetAll(),
                Trips= tripRepo.GetAll()
            };
            return View(stationsTrips);
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
