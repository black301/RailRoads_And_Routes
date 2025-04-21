using Transport_system_prototype.Models;
using Microsoft.AspNetCore.Mvc;
using Transport__system_prototype.Repository;
using Transport__system_prototype.View_Models;

namespace Transport_system_prototype.Controllers
{
    public class TripController : Controller
    {
        private readonly ITripRepository tripRepo;
        private readonly IStationRepository stationRepo;
        private readonly IVehicleRepository vehicleRepo;

        public TripController(ITripRepository _tripRepo,IStationRepository _stationRepo,IVehicleRepository vehicleRepo) // 👈 DI gives you the configured DbContext
        {
            tripRepo=_tripRepo;
            stationRepo=_stationRepo;
            this.vehicleRepo=vehicleRepo;
        }
        public IActionResult Index()
        {
            ViewData["Stations"] = stationRepo.GetAll();  
            ViewData["vehicles"] = vehicleRepo.GetAll();
            return View(tripRepo.GetAll());
        }
        //create
        public IActionResult create()
        {
            //take a list of stations and buses and pass it to the view
            ViewData["Stations"] = stationRepo.GetAll();
            ViewData["vehicles"] = vehicleRepo.GetAll();
            return View();
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult create(Trip Trip)
        {
            ViewData["Stations"] = stationRepo.GetAll();
            ViewData["vehicles"] = vehicleRepo.GetAll();
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
            ViewData["vehicles"] = vehicleRepo.GetAll();
            return View(tripRepo.GetById(id));
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Edit(Trip Trip)
        {
            ViewData["Stations"] = stationRepo.GetAll();
            ViewData["vehicles"] = vehicleRepo.GetAll();
            var stations = stationRepo.GetAll();
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
            tripRepo.Delete(id);
            tripRepo.Save();
            return RedirectToAction("Index");
        }
    }
}
