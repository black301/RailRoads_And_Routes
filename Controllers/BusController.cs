using Bus_system_prototype.Models;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Bus_system_prototype.Controllers
{
    public class BusController : Controller
    {
        context Data = new context();
        public IActionResult Index()
        {
            return View(Data.Buses.ToList());
        }
        public IActionResult create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult create(Bus Bus)
        {

            if (ModelState.IsValid)
            {
                Data.Buses.Add(Bus);
                Data.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(Bus);
        }
        //update
        public IActionResult Edit(int id)
        {
            return View(Data.Buses.Find(id));
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Edit(Bus Bus)
        {
            Bus EditedBus = Data.Buses.Find(Bus.Id);
            if (ModelState.IsValid)
            {
                EditedBus.Type = Bus.Type;
                EditedBus.TV = Bus.TV;
                EditedBus.AirConditioning = Bus.AirConditioning;
                EditedBus.WiFi = Bus.WiFi;
                EditedBus.Drinks = Bus.Drinks;
                EditedBus.Snacks = Bus.Snacks;
                EditedBus.ImgURL = Bus.ImgURL;
                Data.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return View(Bus);
            }
        }
        public IActionResult Delete(int id)
        {
            Data.Buses.Remove(Data.Buses.Find(id));
            Data.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}

