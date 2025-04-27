using System.Diagnostics;
using Transport_system_prototype.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Transport__system_prototype.Repository;
using Transport__system_prototype.ViewModels;
using Transport__system_prototype.Models;
using System.Security.Claims;

namespace Transport_system_prototype.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IGenaricRepository<Trip> tripRepo;
        private readonly IGenaricRepository<Station> stationRepo;
        private readonly IGenaricRepository<AppUser> userRepo;
        private readonly IGenaricRepository<Booking> bookingRepo;
        private readonly IGenaricRepository<Client> clientRepo;

        public HomeController(
            ILogger<HomeController> logger, 
            IGenaricRepository<Trip> _tripRepo, 
            IGenaricRepository<Station> _stationRepo, 
            IGenaricRepository<AppUser> _userRepo,
            IGenaricRepository<Booking> _bookingRepo,
            IGenaricRepository<Client> _clientRepo)
        {
            _logger = logger;
            tripRepo = _tripRepo;
            stationRepo = _stationRepo;
            userRepo = _userRepo;
            bookingRepo = _bookingRepo;
            clientRepo = _clientRepo;
        }

        public IActionResult Index()
        {
            AppUser? appUser = null;
            List<Booking> userBookings = new List<Booking>();

            if (User.Identity.IsAuthenticated)
            {
                var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
                if (userIdClaim != null)
                {
                    appUser = userRepo.GetById(userIdClaim.Value);
                }
            }

            UserTripStationViewModel usertripsStations = new UserTripStationViewModel
            {
                Stations = stationRepo.GetAll(),
                Trips = tripRepo.GetAll(),
                AppUser = appUser,
                UserBookings = userBookings
            };
          
            return View(usertripsStations);
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
