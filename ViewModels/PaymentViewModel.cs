public class PaymentViewModel
{
    public string TripId { get; set; }
    public int NumberOfSeats { get; set; }
    public decimal TotalAmount { get; set; }
    public string Currency { get; set; } = "usd";
    public string SuccessUrl { get; set; }
    public string CancelUrl { get; set; }
}