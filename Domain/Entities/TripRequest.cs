

namespace Domain.Entities
{
    public class TripRequest
    {
        public int Id { get; set; }
        public string VehicleName { get; set; }
        public string From { get; set; }
        public string To { get; set; }

        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        public string Time { get; set; } // Store time in "hh:mm tt" format (e.g., "10:00 AM")
    }
}
