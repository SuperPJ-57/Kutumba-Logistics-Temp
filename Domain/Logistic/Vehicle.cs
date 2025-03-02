using System.Text.Json.Serialization;
using Domain.Logistic.Enum;
using Microsoft.AspNetCore.Http;

namespace Domain.Logistic;

public class Vehicle
{
    public int VehicleId { get; set; }
    public string VehicleOwner { get; set; }
    public string VehicleNumber { get; set; }
    public string VehicleName { get; set; }
    public string VehicleType { get; set; }
    public string VehicleCapacity { get; set; }
    public string VehicleVolume { get; set; }
    //[NotMapped]
    //public IFormFile VehicleImage { get; set; }
    public string VehicleImagePath { get; set; }
    public Status Status { get; set; } = Status.Inactive;
    public bool IsActive { get; set; } = true;
    [JsonIgnore]
    public ICollection<TripRequest> TripRequest{ get; set; } = new List<TripRequest>();
}