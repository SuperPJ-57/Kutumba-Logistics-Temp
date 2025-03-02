namespace Application.Features.Vehicles.Queries.GetAllVehicle;

public record GetAllVehiclesQuery(int PageNumber, int PageSize) : IRequest<IResult>;
