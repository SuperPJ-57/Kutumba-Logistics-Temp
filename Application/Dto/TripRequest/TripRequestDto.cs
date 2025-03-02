using System.ComponentModel.DataAnnotations;

namespace Application.Dto.TripRequest
{
    public class TripRequestDto
    {
        public int Id { get; set; }
        public string VehicleName { get; set; }
        public string Destination { get; set; }

        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        public string Time { get; set; } // Store time in "hh:mm tt" format (e.g., "10:00 AM")
    }
}
