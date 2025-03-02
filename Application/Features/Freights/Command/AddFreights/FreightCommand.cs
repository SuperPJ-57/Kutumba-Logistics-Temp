using System.ComponentModel;
using Domain.Logistic.Enum;

namespace Application.Features.Freights.Command.AddFreights;

public record FreightCommand(
    int FreightId,
    string FreightRate,
    string AdvancedPaid,
    string RemainingAmount,
    int Id,
    int DriverId,
    int VehicleId,
    PaymentMode PaymentMode) : IRequest<IResult>;

