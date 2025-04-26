using Transport_system_prototype.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Transport__system_prototype.Repository;

namespace Transport_system_prototype.Controllers
{
    [Authorize(Roles = "Admin")] // 👈 Only users in the "Admin" role can access any action here

    public class TripController : Controller
    {
        private readonly Transport__system_prototype.Repository.IGenaricRepository<Trip> tripRepo;
        private readonly IGenaricRepository<Vehicle> vehicleRepo;
        private readonly IGenaricRepository<Station> stationRepo;

        public TripController(IGenaricRepository<Trip> _tripRepo, IGenaricRepository<Vehicle> _vehicleRepo,IGenaricRepository<Station> _stationRepo)
        {
            tripRepo=_tripRepo;
            vehicleRepo=_vehicleRepo;
            stationRepo=_stationRepo;
        }
        public IActionResult Index()
        {
            ViewData["Stations"] = tripRepo.GetAll();  
            ViewData["vehicles"] = vehicleRepo.GetAll();
            return View(tripRepo.GetAll());
        }
        //create
        public IActionResult create()
        {
            //take a list of stations and buses and pass it to the view
            ViewData["Stations"] =stationRepo.GetAll();
            ViewData["vehicles"]    = vehicleRepo.GetAll();
            return View();
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult create(Trip Trip)
        {
            ViewData["Stations"] = stationRepo.GetAll();
            ViewData["vehicles"]    = vehicleRepo.GetAll();
            if (ModelState.IsValid)
            {
               tripRepo.Add(Trip);
                tripRepo.Save();
                return RedirectToAction("Index");
            }
            return View(Trip);
        }
        //edit
        public IActionResult Edit(int id)
        {
            ViewData["Stations"] = stationRepo.GetAll();
            ViewData["vehicles"]    = vehicleRepo.GetAll();
            return View(tripRepo.GetById(id));
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Edit(Trip Trip)
        {
            ViewData["Stations"] = stationRepo.GetAll();
            ViewData["vehicles"]    = vehicleRepo.GetAll();
            Trip EditedTrip = tripRepo.GetById(Trip.Id);
            if (ModelState.IsValid)
            {
                EditedTrip.vehicleId = Trip.vehicleId;
                EditedTrip.Price = Trip.Price;
                EditedTrip.NumberOfSeats = Trip.NumberOfSeats;
                EditedTrip.AvailableSeats = Trip.AvailableSeats;
                EditedTrip.TOStationId = Trip.TOStationId;
                EditedTrip.TripDate = Trip.TripDate;
                tripRepo.Save();
                return RedirectToAction("Index");
            }
            else
            {
                return View(Trip);
            }
        }
        //delete
        public IActionResult Delete(int id)
        {
            tripRepo.Delete(tripRepo.GetById(id));
            tripRepo.Save();
            return RedirectToAction("Index");
        }
    }
}
