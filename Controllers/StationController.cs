using Transport__system_prototype.Models;
using Transport_system_prototype.Models;
using Microsoft.AspNetCore.Mvc;
using Transport__system_prototype.Repository;

namespace Transport_system_prototype.Controllers
{
    public class StationController : Controller
    {
        private readonly IStationRepository stationRepo;
        private readonly ITripRepository tripRepo;

        public StationController(IStationRepository _StationRepo,ITripRepository _tripRepo) 
        {
            stationRepo=_StationRepo;
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

            stationRepo.Delete(id);
            stationRepo.Save();
            return RedirectToAction("Index");
        }

    }
}
