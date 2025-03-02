namespace Application.Features.Trips.Command.UpdateTrip;

public record TripUpdateCommand(
    int TripId,
    DateTime StartDate,
    DateTime DeliveryDate,
    string TripAllowance,
    string MaintenanceFee,
    string DriverAllownace,
    int DriverId,
    int VehicleId,
    int ConsignmentId) : IRequest<IResult>;

