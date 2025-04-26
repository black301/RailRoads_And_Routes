using Transport_system_prototype.Models;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.AspNetCore.Authorization;
using Transport__system_prototype.Repository;

namespace Transport_system_prototype.Controllers
{
    [Authorize(Roles = "Admin")] // 👈 Only users in the "Admin" role can access any action here

    public class vehicleController : Controller
    {
        private readonly IGenaricRepository<Trip> tripRepo;
        private readonly IGenaricRepository<Vehicle> vehicleRepo;

        public vehicleController(IGenaricRepository<Trip> _tripRepo, IGenaricRepository<Vehicle> _vehicleRepo)
        {
            tripRepo=_tripRepo;
            vehicleRepo=_vehicleRepo;
        }
        public IActionResult Index()
        {
            return View(vehicleRepo.GetAll());
        }
        public IActionResult create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult create(Vehicle vehicle)
        {

            if (ModelState.IsValid)
            {
                vehicleRepo.Add(vehicle);
                vehicleRepo.Save();
                return RedirectToAction("Index");
            }
            return View(vehicle);
        }
        //update
        public IActionResult Edit(int id)
        {
            return View(vehicleRepo.GetById(id));
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Edit(Vehicle vehicle)
        {
            Vehicle Editedvehicle = vehicleRepo.GetById(vehicle.Id);
            if (ModelState.IsValid)
            {
                Editedvehicle.Type = vehicle.Type;
                Editedvehicle.Name = vehicle.Name;
                Editedvehicle.NumberOfSeats = vehicle.NumberOfSeats;
                Editedvehicle.TV = vehicle.TV;
                Editedvehicle.AirConditioning = vehicle.AirConditioning;
                Editedvehicle.WiFi = vehicle.WiFi;
                Editedvehicle.Drinks = vehicle.Drinks;
                Editedvehicle.Snacks = vehicle.Snacks;
                Editedvehicle.ImgURL = vehicle.ImgURL;
                vehicleRepo.Save();
                return RedirectToAction("Index");
            }
            else
            {
                return View(vehicle);
            }
        }
        public IActionResult Delete(int id)
        {
            var vehicle = vehicleRepo.GetById(id);
            if (vehicle == null)
            {
                return NotFound();
            }

            bool isUsedInTrip = tripRepo.GetAll().Any(t => t.vehicleId == id);

            if (isUsedInTrip)
            {
                TempData["ErrorMessage"] = "Cannot delete this vehicle because it is used in one or more trips.";
                return RedirectToAction("Index");
            }

            vehicleRepo.Delete(vehicle);
            vehicleRepo.Save();
            return RedirectToAction("Index");
        }

    }
}

