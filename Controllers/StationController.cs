using Bus_system_prototype.Models;
using Microsoft.AspNetCore.Mvc;

namespace Bus_system_prototype.Controllers
{
    public class StationController : Controller
    {
        context data = new context(); 
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
            data.Stations.Remove(data.Stations.Find(id));
            data.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
