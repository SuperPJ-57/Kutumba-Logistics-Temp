using System.Text.Json.Serialization;

namespace Application.Dto.TrackVehicles
{
    public class LocationRequest
    {
        [JsonPropertyName("vehicleName")]
        public string VehicleName { get; set; }

        [JsonPropertyName("latitude")]
        public double Latitude { get; set; }

        [JsonPropertyName("longitude")]
        public double Longitude { get; set; }
    }
}
