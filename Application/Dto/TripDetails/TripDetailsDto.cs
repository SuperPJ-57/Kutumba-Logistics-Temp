using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dto.TripDetails
{
    public class TripDetailsDto
    {
        public int Id { get; set; }
        public string VehicleName { get; set; }
        public string Destination { get; set; } // Format: From - To
        public double Latitude { get; set; }
        public double Longitude { get; set; }

       
    }
}
