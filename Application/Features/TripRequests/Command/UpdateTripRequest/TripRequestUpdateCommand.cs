using Domain.Logistic;
using Domain.Logistic.Enum;

namespace Application.Features.TripRequests.Command.UpdateTripRequest;

public record TripRequestUpdateCommand(
    int Id,
    Trip Trip,
    Consignment Consignment,
    Driver Driver,
    Vehicle Vehicle,
    string RefuelStation,
    string FuelReceiptPath,
    ApprovalStatus ApprovalStatus) : IRequest<IResult>;
