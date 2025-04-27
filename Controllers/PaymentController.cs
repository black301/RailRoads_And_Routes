using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.Checkout;
using Transport__system_prototype.ViewModels;
using Transport__system_prototype.Settings;
using Microsoft.Extensions.Options;
using Transport__system_prototype.Repository;
using Transport_system_prototype.Models;

public class PaymentController : Controller
{
    private readonly IGenaricRepository<Trip> _tripRepository;
    private readonly StripeSettings _stripeSettings;

    public PaymentController(
        IGenaricRepository<Trip> tripRepository,
        IOptions<StripeSettings> stripeSettings)
    {
        _tripRepository = tripRepository;
        _stripeSettings = stripeSettings.Value;
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

    public IActionResult Success(string tripId, int seats)
    {
        TempData["TripId"] = tripId;
        TempData["Seats"] = seats;
        return View();
    }

    public IActionResult Cancel()
    {
        return View();
    }
}