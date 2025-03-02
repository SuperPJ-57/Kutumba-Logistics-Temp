namespace Application.Features.Vehicles.Command.DeleteVehicles;
public record DeleteVehicleCommand(int VehicleId) : IRequest<IResult>;
