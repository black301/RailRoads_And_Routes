using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Stripe;
using Stripe.Checkout;
using System.Security.Claims;
using Transport__system_prototype.Models;
using Transport__system_prototype.Settings;
using Transport_system_prototype.Models;

namespace Transport__system_prototype.Controllers
{
    public class BookingController : Controller
    {
        private readonly context data;
        private readonly StripeSettings _stripeSettings;

        public BookingController(context context, IOptions<StripeSettings> stripeOptions)
        {
            data = context;
            _stripeSettings = stripeOptions.Value;
            StripeConfiguration.ApiKey = _stripeSettings.SecretKey;
        }

        // GET: Booking
        public IActionResult Index()
        {
            var bookings = data.Bookings
                .Include(b => b.Client)
                .Include(b => b.Trip)
                .ThenInclude(t => t.vehicle)
                .ToList();

            return View(bookings);
        }

        // GET: Booking/Create
        public IActionResult Create()
        {
            // 1. Get current user ID from claims
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // 2. Find matching Client from database
            var currentClient = data.Clients
                .Include(c => c.User) // Include related User info (name, email, phone)
                .FirstOrDefault(c => c.UserID == userId);

            // 3. Pass the client to the view
            ViewBag.CurrentUser = currentClient;

            ViewBag.Clients = data.Clients.ToList();
            ViewBag.Trips = data.Trips
                .Include(t => t.vehicle)
                .Where(t => t.AvailableSeats > 0)
                .ToList();

            return View();
        }

        // POST: Booking/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Booking booking)
        {
            // 1. Get current user ID from claims
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // 2. Find matching Client from database
            var currentClient = data.Clients
                .Include(c => c.User) // Include related User info (name, email, phone)
                .FirstOrDefault(c => c.UserID == userId);

            // 3. Pass the client to the view
            ViewBag.CurrentUser = currentClient;

            if (ModelState.IsValid)
            {
                var trip = data.Trips.FirstOrDefault(t => t.Id == booking.TripId);
                if (trip != null && trip.AvailableSeats >= booking.NumberOfSeats)
                {
                    trip.AvailableSeats -= booking.NumberOfSeats;
                    booking.BookingDate = DateTime.Now;

                    data.Bookings.Add(booking);
                    data.SaveChanges();

                    return RedirectToAction(nameof(Index));
                }

                ModelState.AddModelError("", "Not enough available seats.");
            }

            ViewBag.Clients = data.Clients.ToList();
            ViewBag.Trips = data.Trips
                .Include(t => t.vehicle)
                .Where(t => t.AvailableSeats > 0)
                .ToList();

            return View(booking);
        }

        // POST: Booking/Pay
        [HttpPost]
        public IActionResult Pay(int bookingId)
        {
            var booking = data.Bookings
                .Include(b => b.Client)
                .Include(b => b.Trip)
                    .ThenInclude(t => t.FromStation)
                .Include(b => b.Trip)
                    .ThenInclude(t => t.TOStation)
                .FirstOrDefault(b => b.Id == bookingId);

            if (booking == null || booking.Trip == null)
                return NotFound();

            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = new List<SessionLineItemOptions>
                {
                    new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmount = booking.Trip.Price * 100, // cents
                            Currency = "usd",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = $"Trip from {booking.Trip.FromStation?.Name} to {booking.Trip.TOStation?.Name}"
                            }
                        },
                        Quantity = booking.NumberOfSeats
                    }
                },
                Mode = "payment",
                SuccessUrl = Url.Action("Success", "Booking", null, Request.Scheme)!,
                CancelUrl = Url.Action("Cancel", "Booking", null, Request.Scheme)!
            };

            var service = new SessionService();
            Session session = service.Create(options);

            return Redirect(session.Url);
        }

        // GET: Booking/Success
        public IActionResult Success()
        {
            return View(); // You can display a success message here
        }

        // GET: Booking/Cancel
        public IActionResult Cancel()
        {
            return View(); // You can display a cancel/failure message here
        }

        // GET: Booking/Delete/5
        public IActionResult Delete(int id)
        {
            var booking = data.Bookings
                .Include(b => b.Client)
                .Include(b => b.Trip)
                .FirstOrDefault(b => b.Id == id);

            if (booking == null)
                return NotFound();

            return View(booking);
        }

        // POST: Booking/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var booking = data.Bookings.FirstOrDefault(b => b.Id == id);
            if (booking != null)
            {
                var trip = data.Trips.FirstOrDefault(t => t.Id == booking.TripId);
                if (trip != null)
                {
                    trip.AvailableSeats += booking.NumberOfSeats;
                }

                data.Bookings.Remove(booking);
                data.SaveChanges();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
