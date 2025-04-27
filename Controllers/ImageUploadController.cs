using Microsoft.AspNetCore.Mvc;

namespace Transport__system_prototype.Controllers
{
    public class ImageUploadController : Controller
    {
        // GET: Displays the upload form
        public IActionResult Index()
        {
            return View();
        }

        // POST: Handles the image upload
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upload(IFormFile imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
            {
                ModelState.AddModelError("", "Please select a file.");
                return View("Index");
            }

            // Validate file type (e.g., only images)
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            var fileExtension = Path.GetExtension(imageFile.FileName).ToLower();
            if (!allowedExtensions.Contains(fileExtension))
            {
                ModelState.AddModelError("", "Only JPEG, PNG, or GIF files are allowed.");
                return View("Index");
            }

            // Create uploads folder if it doesn't exist
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            // Generate a unique filename
            var uniqueFileName = Guid.NewGuid().ToString() + fileExtension;
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            // Save the file
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }

            // Return success message with the file path
            ViewBag.Message = $"File uploaded successfully: {uniqueFileName}";
            ViewBag.ImagePath = $"/uploads/{uniqueFileName}"; // Relative path for display

            return View("Index");
        }
    }
}
