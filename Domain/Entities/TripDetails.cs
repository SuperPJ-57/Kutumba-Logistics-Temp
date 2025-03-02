namespace Domain.Entities
{
    public class TripDetails
    {
        public int Id { get; set; }
        public string VehicleName { get; set; }
        public string From { get; set; } 
        public string To { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
