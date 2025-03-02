
using Application.Dto.TripRequests;
using Domain.Logistic.Enum;


namespace Application.Dto.Vehicles;

public class VehicleDto
{
    public int VehicleId { get; set; }
    public string VehicleOwner { get; set; }
    public string VehicleNumber { get; set; }
    public string VehicleName { get; set; }
    public string VehicleType { get; set; }
    public string VehicleCapacity { get; set; }
    public string VehicleVolume { get; set; }
    public string VehicleImagePath { get; set; }
    public Status Status { get; set; } = Status.Inactive;
    public ICollection<TripRequestDto> TripRequestDto { get; set; } = new List<TripRequestDto>();

}
