namespace Application.Features.Drivers.Command.DeleteDrivers;

public record DriversDeleteCommand( int DriverId) : IRequest<IResult>;
