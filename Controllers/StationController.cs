using Transport__system_prototype.Models;
using Transport_system_prototype.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Transport_system_prototype.Controllers
{
    [Authorize(Roles = "Admin")] // 👈 Only users in the "Admin" role can access any action here

    public class StationController : Controller
    {
        private readonly context data;

        public StationController(context db) 
        {
            data = db;
        }
        public IActionResult Index()
        {
            return View(data.Stations.ToList());
        }

        public IActionResult create()
        {
            return View();
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult create(Station Station)
        {
            if (ModelState.IsValid)
            {
                data.Stations.Add(Station);
                data.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(Station);
        }
        public IActionResult Edit(int id)
        {
            return View(data.Stations.Find(id));
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Edit(Station Station)
        {
            Station EditedStation = data.Stations.Find(Station.Id);
            if (ModelState.IsValid)
            {
                EditedStation.Name = Station.Name;
                EditedStation.Location = Station.Location;
                EditedStation.ImgURL = Station.ImgURL;
                data.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return View(Station);
            }
        }
        public IActionResult Delete(int id)
        {
            var station = data.Stations.Find(id);
            if (station == null)
            {
                return NotFound();
            }

            bool isUsedInTrip = data.Trips.Any(t => t.FromStationId == id || t.TOStationId == id);

            if (isUsedInTrip)
            {
                TempData["ErrorMessage"] = "Cannot delete this station because it is used in one or more trips.";
                return RedirectToAction("Index");
            }

            data.Stations.Remove(station);
            data.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}
