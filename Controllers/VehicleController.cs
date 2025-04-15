using Transport_system_prototype.Models;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Transport_system_prototype.Controllers
{
    public class vehicleController : Controller
    {
        private readonly context data;

        public vehicleController(context db) // 👈 DI gives you the configured DbContext
        {
            data = db;
        }
        public IActionResult Index()
        {
            return View(data.vehicles.ToList());
        }
        public IActionResult create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult create(vehicle vehicle)
        {

            if (ModelState.IsValid)
            {
                data.vehicles.Add(vehicle);
                data.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(vehicle);
        }
        //update
        public IActionResult Edit(int id)
        {
            return View(data.vehicles.Find(id));
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Edit(vehicle vehicle)
        {
            vehicle Editedvehicle = data.vehicles.Find(vehicle.Id);
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
                data.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return View(vehicle);
            }
        }
        public IActionResult Delete(int id)
        {
            var vehicle = data.vehicles.Find(id);
            if (vehicle == null)
            {
                return NotFound();
            }

            bool isUsedInTrip = data.Trips.Any(t => t.vehicleId == id);

            if (isUsedInTrip)
            {
                TempData["ErrorMessage"] = "Cannot delete this vehicle because it is used in one or more trips.";
                return RedirectToAction("Index");
            }

            data.vehicles.Remove(vehicle);
            data.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}

