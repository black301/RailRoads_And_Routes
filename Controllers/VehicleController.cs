using Transport_system_prototype.Models;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Transport_system_prototype.Controllers
{
    public class vehicleController : Controller
    {
        context Data = new context();
        public IActionResult Index()
        {
            return View(Data.vehicles.ToList());
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
                Data.vehicles.Add(vehicle);
                Data.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(vehicle);
        }
        //update
        public IActionResult Edit(int id)
        {
            return View(Data.vehicles.Find(id));
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Edit(vehicle vehicle)
        {
            vehicle Editedvehicle = Data.vehicles.Find(vehicle.Id);
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
                Data.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return View(vehicle);
            }
        }
        public IActionResult Delete(int id)
        {
            Data.vehicles.Remove(Data.vehicles.Find(id));
            Data.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}

