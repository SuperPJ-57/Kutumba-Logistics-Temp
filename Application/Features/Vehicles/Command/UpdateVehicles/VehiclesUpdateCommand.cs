
using Domain.Logistic.Enum;

namespace Application.Features.Vehicles.Command.UpdateVehicles;

public record VehiclesUpdateCommand(
    int VehicleId,
    string VehicleOwner,
    string VehicleNumber,
    string VehicleName,
    string VehicleType,
    string VehicleCapacity,
    string VehicleVolume,
    string VehicleImagePath,
    int DriverId,
    Status Status) : IRequest<IResult>;

