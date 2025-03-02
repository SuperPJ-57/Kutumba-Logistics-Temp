
using Application.Dto.Vehicles;
using Domain.Logistic.Enum;

namespace Application.Features.Vehicles.Command.AddVehicles;

public record VehiclesCommand(
 string VehicleOwner,
 string VehicleNumber,
 string VehicleName,
 string VehicleType,
 string VehicleCapacity,
 string VehicleVolume,
 int DriverId,
 int Id, //TripRequest Id
//[NotMapped]
//public IFormFile VehicleImage { get; set; }
string VehicleImagePath,
Status Status) : IRequest<IResult>;
