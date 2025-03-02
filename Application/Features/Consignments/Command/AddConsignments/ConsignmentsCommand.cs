
namespace Application.Features.Consignments.Command.AddConsignment;

public record ConsignmentsCommand(
    string PartyName,
    string PartyContact,
    string GoodDescription,
    string TotalWeight,
    string LoadingPoint,
    string UnloadingPoint,
    int Id,
    int VehicleId,
    int DriverId,
    int ConsignmentId,
    int FreightId) : IRequest<IResult>;
