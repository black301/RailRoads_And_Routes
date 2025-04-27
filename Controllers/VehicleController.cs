using Transport_system_prototype.Models;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.AspNetCore.Authorization;
using Transport__system_prototype.Repository;

namespace Transport_system_prototype.Controllers
{
    [Authorize(Roles = "Admin")]
    public class vehicleController : Controller
    {
        private readonly IGenaricRepository<Trip> tripRepo;
        private readonly IGenaricRepository<Vehicle> vehicleRepo;

        public vehicleController(IGenaricRepository<Trip> _tripRepo, IGenaricRepository<Vehicle> _vehicleRepo)
        {
            tripRepo = _tripRepo;
            vehicleRepo = _vehicleRepo;
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
        [ValidateAntiForgeryToken]
        public IActionResult Create(Vehicle vehicle)
        {
            if (ModelState.IsValid)
            {
                // Generate a unique file name
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(vehicle.ImageFile.FileName);

                // Define the path where you want to save the uploaded image
                string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

                // Ensure the folder exists
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                string filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    vehicle.ImageFile.CopyTo(stream);
                }

                // Save the image URL (relative path)
                vehicle.ImgURL = "/uploads/" + fileName;

                vehicleRepo.Add(vehicle);
                vehicleRepo.Save();
                return RedirectToAction("Index");
            }
            return View(vehicle); // return vehicle with validation errors
        }

        //update
        // Edit (GET)
        public IActionResult Edit(int id)
        {
            return View(vehicleRepo.GetById(id));
        }

        // Edit (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Vehicle vehicle)
        {
            Vehicle editedVehicle = vehicleRepo.GetById(vehicle.Id);
            if (ModelState.IsValid)
            {
                // Update basic fields
                editedVehicle.Type = vehicle.Type;
                editedVehicle.Name = vehicle.Name;
                editedVehicle.NumberOfSeats = vehicle.NumberOfSeats;
                editedVehicle.TV = vehicle.TV;
                editedVehicle.AirConditioning = vehicle.AirConditioning;
                editedVehicle.WiFi = vehicle.WiFi;
                editedVehicle.Drinks = vehicle.Drinks;
                editedVehicle.Snacks = vehicle.Snacks;

                // Check if a new image is uploaded
                if (vehicle.ImageFile != null && vehicle.ImageFile.Length > 0)
                {
                    // Delete the old image if it exists
                    if (!string.IsNullOrEmpty(editedVehicle.ImgURL))
                    {
                        string oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", editedVehicle.ImgURL.TrimStart('/'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    // Save the new image
                    string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                    if (!Directory.Exists(uploadsFolder))
                        Directory.CreateDirectory(uploadsFolder);

                    string newFileName = Guid.NewGuid().ToString() + Path.GetExtension(vehicle.ImageFile.FileName);
                    string newFilePath = Path.Combine(uploadsFolder, newFileName);

                    using (var stream = new FileStream(newFilePath, FileMode.Create))
                    {
                        vehicle.ImageFile.CopyTo(stream);
                    }

                    // Update the image URL
                    editedVehicle.ImgURL = "/uploads/" + newFileName;
                }

                vehicleRepo.Save();
                return RedirectToAction("Index");
            }
            else
            {
                return View(vehicle);
            }
        }

        // Delete
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

            // Delete the image file if it exists
            if (!string.IsNullOrEmpty(vehicle.ImgURL))
            {
                string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", vehicle.ImgURL.TrimStart('/'));
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }

            vehicleRepo.Delete(vehicle);
            vehicleRepo.Save();
            return RedirectToAction("Index");
        }


    }
}

