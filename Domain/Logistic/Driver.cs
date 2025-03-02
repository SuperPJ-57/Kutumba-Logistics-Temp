using System.Text.Json.Serialization;
using Domain.Logistic.Enum;
using Microsoft.AspNetCore.Http;



namespace Domain.Logistic;

public class Driver
{
    public int DriverId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Contact { get; set; }
    public string LicenseNumber { get; set; }
    public DateOnly DateOfBirth { get; set; }
    //[NotMapped]
    //public IFormFile DriverImage { get; set; }
    public string ProfileImagePath { get; set; }
    public Status Status { get; set; } = Status.Inactive;
    public bool IsActive { get; set; } = true;
    [JsonIgnore]
    public ICollection<TripRequest> TripRequest { get; set; } = new List<TripRequest>();
}
