using Transport_system_prototype.Models;
using Microsoft.AspNetCore.Mvc;

namespace Transport_system_prototype.Controllers
{
    public class TripController : Controller
    {
        context data = new context();
        public IActionResult Index()
        {
            ViewData["Stations"] = data.Stations.ToList();  
            ViewData["vehicles"] = data.vehicles.ToList();
            return View(data.Trips.ToList());
        }
        //create
        public IActionResult create()
        {
            //take a list of stations and buses and pass it to the view
            ViewData["Stations"] = data.Stations.ToList();
            ViewData["vehicles"]    = data.vehicles.ToList();
            return View();
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult create(Trip Trip)
        {
            ViewData["Stations"] = data.Stations.ToList();
            ViewData["vehicles"]    = data.vehicles.ToList();
            if (ModelState.IsValid)
            {
                data.Trips.Add(Trip);
                data.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(Trip);
        }
        //edit
        public IActionResult Edit(int id)
        {
            ViewData["Stations"] = data.Stations.ToList();
            ViewData["vehicles"]    = data.vehicles.ToList();
            return View(data.Trips.Find(id));
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Edit(Trip Trip)
        {
            ViewData["Stations"] = data.Stations.ToList();
            ViewData["vehicles"]    = data.vehicles.ToList();
            Trip EditedTrip = data.Trips.Find(Trip.Id);
            if (ModelState.IsValid)
            {
                EditedTrip.vehicleId = Trip.vehicleId;
                EditedTrip.Price = Trip.Price;
                EditedTrip.NumberOfSeats = Trip.NumberOfSeats;
                EditedTrip.AvailableSeats = Trip.AvailableSeats;
                EditedTrip.TOStationId = Trip.TOStationId;
                EditedTrip.TripDate = Trip.TripDate;
                data.SaveChanges();
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
            data.Trips.Remove(data.Trips.Find(id));
            data.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
