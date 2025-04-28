using Transport_system_prototype.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Transport__system_prototype.Repository;
using Microsoft.AspNetCore.SignalR;
using RailRoads_And_Routes.Hubs;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Transport_system_prototype.Controllers
{
    [Authorize(Roles = "Admin")] // 👈 Only users in the "Admin" role can access any action here

    public class TripController : Controller
    {
        private readonly Transport__system_prototype.Repository.IGenaricRepository<Trip> tripRepo;
        private readonly IGenaricRepository<Vehicle> vehicleRepo;
        private readonly IGenaricRepository<Station> stationRepo;
        private readonly IHubContext<BookingHub> _hubContext;

        public TripController(IGenaricRepository<Trip> _tripRepo, IGenaricRepository<Vehicle> _vehicleRepo,IGenaricRepository<Station> _stationRepo, IHubContext<BookingHub> hubContext)
        {
            tripRepo=_tripRepo;
            vehicleRepo=_vehicleRepo;
            stationRepo=_stationRepo;
            _hubContext = hubContext;
        }
        public IActionResult Index()
        {
            ViewData["Stations"] = tripRepo.GetAll();  
            ViewData["vehicles"] = vehicleRepo.GetAll();
            return View(tripRepo.GetAll());
        }
        //create
        public IActionResult create()
        {
            //take a list of stations and buses and pass it to the view
            ViewData["Stations"] =stationRepo.GetAll();
            ViewData["vehicles"]    = vehicleRepo.GetAll();
            return View();
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Create([Bind("Id,FromStationId,TOStationId,TripDate,Price,VehicleId,NumberOfSeats,AvailableSeats")] Trip trip)
        {
            if (ModelState.IsValid)
            {
                tripRepo.Add(trip);
                await tripRepo.SaveAsync();

                // Notify clients about the new trip
                await _hubContext.Clients.All.SendAsync("ReceiveNewTrip", 
                    trip.Id, 
                    trip.FromStation?.Name, 
                    trip.TOStation?.Name, 
                    trip.TripDate.ToString("MMM dd, yyyy"), 
                    trip.vehicle?.Name, 
                    trip.AvailableSeats, 
                    trip.Price);

                return RedirectToAction(nameof(Index));
            }
            ViewData["FromStationId"] = new SelectList(stationRepo.GetAll(), "Id", "Name", trip.FromStationId);
            ViewData["TOStationId"] = new SelectList(stationRepo.GetAll(), "Id", "Name", trip.TOStationId);
            ViewData["VehicleId"] = new SelectList(vehicleRepo.GetAll(), "Id", "Name", trip.vehicleId);
            return View(trip);
        }
        //edit
        public IActionResult Edit(int id)
        {
            ViewData["Stations"] = stationRepo.GetAll();
            ViewData["vehicles"]    = vehicleRepo.GetAll();
            return View(tripRepo.GetById(id));
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FromStationId,TOStationId,TripDate,Price,VehicleId,NumberOfSeats,AvailableSeats")] Trip trip)
        {
            if (id != trip.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    tripRepo.Update(trip);
                    await tripRepo.SaveAsync();

                    // Notify clients about the trip update
                    await _hubContext.Clients.All.SendAsync("ReceiveTripUpdate", trip.Id, trip.AvailableSeats);

                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TripExists(trip.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            ViewData["FromStationId"] = new SelectList(stationRepo.GetAll(), "Id", "Name", trip.FromStationId);
            ViewData["TOStationId"] = new SelectList(stationRepo.GetAll(), "Id", "Name", trip.TOStationId);
            ViewData["VehicleId"] = new SelectList(vehicleRepo.GetAll(), "Id", "Name", trip.vehicleId);
            return View(trip);
        }
        //delete
        public IActionResult Delete(int id)
        {
            tripRepo.Delete(tripRepo.GetById(id));
            tripRepo.Save();
            return RedirectToAction("Index");
        }

        private bool TripExists(int id)
        {
            return tripRepo.GetAll().Any(e => e.Id == id);
        }
    }
}
