public class BookingViewModel
{
    public int BookingId { get; set; }
    public string FromStation { get; set; }
    public string ToStation { get; set; }
    public DateTime TripDate { get; set; }
    public string VehicleName { get; set; }
    public int NumberOfSeats { get; set; }
    public decimal TotalPrice { get; set; }
    public DateTime BookingDate { get; set; }
}