namespace Bus_system_prototype.Models
{
    public class Bus
    {
        public int Id { get; set; }
        public string Type { get; set; }
        //added
        public int NumberOfSeats { get; set; }
        //--------------------------------
        public bool TV { get; set; }
        public bool AirConditioning { get; set; }
        public bool WiFi { get; set; }
        public bool Drinks { get; set; }
        public bool Snacks { get; set; }
        public string ImgURL { get; set; }
    }
}
