

using Application.Dto.TripRequests;
using Domain.Logistic.Enum;



namespace Application.Dto.Drivers;

public class DriverDto
{
    public int DriverId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Contact { get; set; }
    public string LicenseNumber { get; set; }
    public DateOnly DateOfBirth { get; set; }
    public string ProfileImagePath { get; set; }
    public Status Status { get; set; } = Status.Inactive;
    public ICollection<TripRequestDto> TripRequestDto { get; set; } = new List<TripRequestDto>();
    
}
