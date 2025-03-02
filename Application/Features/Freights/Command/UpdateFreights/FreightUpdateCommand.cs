using Domain.Logistic.Enum;

namespace Application.Features.Freights.Command.UpdateFreights;

public record FreightUpdateCommand(
    int FreightId,
    string FreightRate,
    string AdvancedPaid,
    string RemainingAmount,
    int Id,
    int DriverId,
    int VehicleId,
    PaymentMode PaymentMode) : IRequest<IResult>;
