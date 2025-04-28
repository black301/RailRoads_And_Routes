using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.Checkout;
using Transport__system_prototype.ViewModels;
using Transport__system_prototype.Settings;
using Microsoft.Extensions.Options;
using Transport__system_prototype.Repository;
using Transport_system_prototype.Models;
using Transport__system_prototype.Models;
using RailRoads_And_Routes.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

public class PaymentController : Controller
{
    private readonly IGenaricRepository<Trip> _tripRepository;
    private readonly IGenaricRepository<Booking> _bookingRepository;
    private readonly StripeSettings _stripeSettings;
    private readonly IHubContext<BookingHub> _hubContext;
    private readonly IEmailSender _emailSender;
    private readonly UserManager<AppUser> _userManager;

    public PaymentController(
        IOptions<StripeSettings> stripeSettings,
        IGenaricRepository<Trip> tripRepository,
        IGenaricRepository<Booking> bookingRepository,
        IHubContext<BookingHub> hubContext,
        IEmailSender emailSender,
        UserManager<AppUser> userManager)
    {
        _stripeSettings = stripeSettings.Value;
        _tripRepository = tripRepository;
        _bookingRepository = bookingRepository;
        _hubContext = hubContext;
        _emailSender = emailSender;
        _userManager = userManager;
    }

    [HttpPost]
    public IActionResult CreateCheckoutSession([FromBody] PaymentViewModel model)
    {
        var trip = _tripRepository.GetAll().FirstOrDefault(t => t.Id.ToString() == model.TripId);
        if (trip == null) return NotFound();

        var domain = $"{Request.Scheme}://{Request.Host}";
        var options = new SessionCreateOptions
        {
            PaymentMethodTypes = new List<string> { "card" },
            LineItems = new List<SessionLineItemOptions>
            {
                new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = trip.Price * 100, // Stripe uses cents
                        Currency = model.Currency,
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = $"Trip from {trip.FromStation?.Name} to {trip.TOStation?.Name}",
                            Description = $"Travel Date: {trip.TripDate:g}"
                        },
                    },
                    Quantity = model.NumberOfSeats,
                },
            },
            Mode = "payment",
            SuccessUrl = $"{domain}/Payment/Success?tripId={model.TripId}&seats={model.NumberOfSeats}",
            CancelUrl = $"{domain}/Payment/Cancel"
        };

        var service = new SessionService();
        var session = service.Create(options);

        return Json(new { id = session.Id });
    }

    public async Task<IActionResult> Success(string tripId, int seats)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId)) return Challenge();

        var user = await _userManager.FindByIdAsync(userId);
        var trip = _tripRepository.GetAll().FirstOrDefault(t => t.Id.ToString() == tripId);

        if (trip != null && user != null)
        {
            // Create booking record
            var booking = new Booking
            {
                UserId = userId,
                TripId = trip.Id,
                NumberOfSeats = seats,
                BookingDate = DateTime.Now
            };

            _bookingRepository.Add(booking);
            await _bookingRepository.SaveAsync();

            // Update trip available seats
            trip.AvailableSeats -= seats;
            _tripRepository.Update(trip);
            await _tripRepository.SaveAsync();

            // Send confirmation email with ticket
            var subject = "Booking Confirmation - Your Ticket";
            var message = $"Dear {user.UserName},<br><br>" +
                         $"Thank you for booking with RailRoads & Routes! Your booking has been confirmed.<br>" +
                         $"Your ticket is attached to this email.<br><br>" +
                         $"Safe travels!<br>" +
                         $"RailRoads & Routes Team";

            var ticketData = new
            {
                PassengerName = user.UserName,
                From = trip.FromStation?.Name,
                To = trip.TOStation?.Name,
                Date = trip.TripDate.ToString("MMMM dd, yyyy"),
                Time = trip.TripDate.ToString("hh:mm tt"),
                Seats = seats,
                Price = $"${trip.Price * seats}",
                BookingId = $"BK-{booking.Id}"
            };

            await _emailSender.SendEmailWithAttachmentAsync(user.Email, subject, message, null, "Ticket.pdf");

            // Notify other users about seat update
            await NotifyTripUpdate(tripId, trip.AvailableSeats, user.UserName);

            TempData["SuccessMessage"] = "Booking confirmed! Check your email for the ticket.";
        }

        TempData["TripId"] = tripId;
        TempData["Seats"] = seats;
        return View();
    }

    public IActionResult Cancel()
    {
        return RedirectToAction("Index","Home");
    }

    // Add after successful payment
    private async Task NotifyTripUpdate(string tripId, int availableSeats, string clientName)
    {
        await _hubContext.Clients.All.SendAsync("ReceiveTripUpdate", tripId, availableSeats);
        await _hubContext.Clients.All.SendAsync("ReceiveBookingConfirmation", tripId, clientName, availableSeats);
    }
}