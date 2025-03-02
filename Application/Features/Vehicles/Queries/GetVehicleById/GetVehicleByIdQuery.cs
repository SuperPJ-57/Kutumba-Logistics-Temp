namespace Application.Features.Vehicles.Queries.GetVehicleById;

public record GetVehicleByIdQuery(int VehicleId) : IRequest<IResult>;