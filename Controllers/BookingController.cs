using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Transport_system_prototype.Models;
using Transport__system_prototype.Models;
using Transport__system_prototype.Repository;
using System.Security.Claims;
using Microsoft.AspNetCore.SignalR;
using RailRoads_And_Routes.Hubs;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Transport_system_prototype.Controllers
{
    [Authorize]
    public class BookingController : Controller
    {
        private readonly IGenaricRepository<Booking> _bookingRepository;
        private readonly IGenaricRepository<Trip> _tripRepository;
        private readonly IGenaricRepository<Client> _clientRepository;
        private readonly IGenaricRepository<AppUser> _userRepository;

        private readonly IHubContext<BookingHub> _hubContext;

        public BookingController(
            IGenaricRepository<Booking> bookingRepository,
            IGenaricRepository<Trip> tripRepository,
            IGenaricRepository<Client> clientRepository,
            IGenaricRepository<AppUser> userRepository,
            IHubContext<BookingHub> hubContext)
        {
            _bookingRepository = bookingRepository;
            _tripRepository = tripRepository;
            _clientRepository = clientRepository;
            _userRepository = userRepository;
            _hubContext = hubContext;
        }
        
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Create(int tripId, int numberOfSeats)
        {
            var trip = _tripRepository.GetAll().FirstOrDefault(c => c.Id == tripId);
            if (trip == null || trip.AvailableSeats < numberOfSeats)
            {
                TempData["Error"] = "Selected trip is not available or has insufficient seats.";
                return RedirectToAction("Index", "Home");
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = _userRepository.GetAll().FirstOrDefault(c => c.Id == userId);

            if (user == null)
            {
                TempData["Error"] = "Client profile not found.";
                return RedirectToAction("Index", "Home");
            }

            var booking = new Booking
            {
                UserId = user.Id,
                TripId = tripId,
                NumberOfSeats = numberOfSeats,
                BookingDate = DateTime.Now
            };

            try
            {
                // Update available seats
                trip.AvailableSeats -= numberOfSeats;
                _tripRepository.Update(trip);
                _bookingRepository.Add(booking);
                _bookingRepository.Save();

                
                await _hubContext.Clients.All.SendAsync("ReceiveSeatUpdate", trip.Id, trip.AvailableSeats);

                TempData["Success"] = "Booking confirmed successfully!";
                return RedirectToAction("Index", "Home");
            }
            catch (Exception)
            {
                TempData["Error"] = "An error occurred while processing your booking.";
                return RedirectToAction("Index", "Home");
            }
        }

    }
}