using Transport__system_prototype.Models;
using Transport_system_prototype.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Transport__system_prototype.Repository;

namespace Transport_system_prototype.Controllers
{
    [Authorize(Roles = "Admin")] // 👈 Only users in the "Admin" role can access any action here

    public class StationController : Controller
    {
        private readonly IGenaricRepository<Station> stationRepo;
        private readonly IGenaricRepository<Trip> tripRepo;

        public StationController(IGenaricRepository<Station> _stationRepo, IGenaricRepository<Trip> _tripRepo)
        {
            stationRepo=_stationRepo;
            tripRepo=_tripRepo;
        }
        public IActionResult Index()
        {
            return View(stationRepo.GetAll());
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
                stationRepo.Add(Station);
                stationRepo.Save();
                return RedirectToAction("Index");
            }
            return View(Station);
        }
        public IActionResult Edit(int id)
        {
            return View(stationRepo.GetById(id));
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Edit(Station Station)
        {
            Station EditedStation = stationRepo.GetById(Station.Id);
            if (ModelState.IsValid)
            {
                EditedStation.Name = Station.Name;
                EditedStation.Location = Station.Location;
                EditedStation.ImgURL = Station.ImgURL;
                stationRepo.Save();
                return RedirectToAction("Index");
            }
            else
            {
                return View(Station);
            }
        }
        public IActionResult Delete(int id)
        {
            var station = stationRepo.GetById(id);
            if (station == null)
            {
                return NotFound();
            }

            bool isUsedInTrip = tripRepo.GetAll().Any(t => t.FromStationId == id || t.TOStationId == id);

            if (isUsedInTrip)
            {
                TempData["ErrorMessage"] = "Cannot delete this station because it is used in one or more trips.";
                return RedirectToAction("Index");
            }

           stationRepo.Delete(station);
            stationRepo.Save();
            return RedirectToAction("Index");
        }

    }
}
