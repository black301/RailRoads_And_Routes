using Transport__system_prototype.Models;
using Transport_system_prototype.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Transport__system_prototype.Repository;

namespace Transport_system_prototype.Controllers
{
    [Authorize(Roles = "Admin")]
    public class StationController : Controller
    {
        private readonly IGenaricRepository<Station> stationRepo;
        private readonly IGenaricRepository<Trip> tripRepo;

        public StationController(IGenaricRepository<Station> _stationRepo, IGenaricRepository<Trip> _tripRepo)
        {
            stationRepo = _stationRepo;
            tripRepo = _tripRepo;
        }

        public IActionResult Index()
        {
            return View(stationRepo.GetAll());
        }

        public IActionResult Create()
        {
            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Create(Station station)
        {
            if (ModelState.IsValid)
            {
                if (station.ImageFile != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(station.ImageFile.FileName);
                    string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

                    if (!Directory.Exists(uploadsFolder))
                        Directory.CreateDirectory(uploadsFolder);

                    string filePath = Path.Combine(uploadsFolder, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        station.ImageFile.CopyTo(stream);
                    }

                    station.ImgURL = "/uploads/" + fileName;
                }

                stationRepo.Add(station);
                stationRepo.Save();
                return RedirectToAction("Index");
            }
            return View(station);
        }

        public IActionResult Edit(int id)
        {
            return View(stationRepo.GetById(id));
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Edit(Station station)
        {
            Station EditedStation = stationRepo.GetById(station.Id);
            if (ModelState.IsValid)
            {
                if (station.ImageFile != null)
                {
                    // Delete the old image if exists
                    if (!string.IsNullOrEmpty(EditedStation.ImgURL))
                    {
                        string oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", EditedStation.ImgURL.TrimStart('/'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    // Upload the new image
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(station.ImageFile.FileName);
                    string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

                    if (!Directory.Exists(uploadsFolder))
                        Directory.CreateDirectory(uploadsFolder);

                    string filePath = Path.Combine(uploadsFolder, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        station.ImageFile.CopyTo(stream);
                    }

                    EditedStation.ImgURL = "/uploads/" + fileName;
                }

                EditedStation.Name = station.Name;
                EditedStation.Location = station.Location;
                stationRepo.Save();
                return RedirectToAction("Index");
            }
            else
            {
                return View(station);
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


            // Delete the image file if it exists
            if (!string.IsNullOrEmpty(station.ImgURL))
            {
                string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", station.ImgURL.TrimStart('/'));
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }


            stationRepo.Delete(station);
            stationRepo.Save();
            return RedirectToAction("Index");
        }
    }
}
