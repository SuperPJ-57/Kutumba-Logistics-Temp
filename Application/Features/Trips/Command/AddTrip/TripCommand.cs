

namespace Application.Features.Trips.Command.AddTrip;

public record TripCommand(
    int TripId,
    DateTime StartDate,
    DateTime DeliveryDate,
    string TripAllowance,
    string MaintenanceFee,
    string DriverAllownace,
    int Id,
    int DriverId,
    int ConsignmentId,
    int FreightId,
    int VehicleId) : IRequest<IResult>;