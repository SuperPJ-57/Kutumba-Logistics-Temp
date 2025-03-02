
namespace Application.Features.Consignments.Command.UpdateConsignment;

public record ConsignmentsUpdateCommand(
int ConsignmentId,
string PartyName,
string PartyContact,
string GoodDescription,
string TotalWeight,
string LoadingPoint,
string UnloadingPoint,
int Id,
int VehicleId,
int DriverId,
int FreightId) : IRequest<IResult>;

