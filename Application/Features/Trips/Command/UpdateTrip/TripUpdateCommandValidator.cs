
using Application.Interfaces.Repositories;
using FluentValidation;

namespace Application.Features.Trips.Command.UpdateTrip;

public class TripUpdateCommandValidator : AbstractValidator<TripUpdateCommand>
{

    private readonly ITripRequestRepository _tripRequestRepository;
    public TripUpdateCommandValidator(ITripRequestRepository tripRequestRepository)
    {
        _tripRequestRepository = tripRequestRepository;

        RuleFor(x => x.ConsignmentId)
            .MustAsync(async (consignmentId, cancellation) => await _tripRequestRepository.AnyAsync(x => x.ConsignmentId == consignmentId))
            .WithMessage("Consignment not found.");

        RuleFor(x => x.VehicleId)
           .MustAsync(async (vehicle, cancellation) => await _tripRequestRepository.AnyAsync(x => x.VehicleId == vehicle))
           .WithMessage("Consignment not found.");

        RuleFor(x => x.DriverId)
           .MustAsync(async (driver, cancellation) => await _tripRequestRepository.AnyAsync(x => x.DriverId == driver))
           .WithMessage("Consignment not found.");
        RuleFor(x => x.DeliveryDate).NotEmpty().WithMessage("Delivery Date is Required.");

        RuleFor(x => x.TripAllowance).NotEmpty().WithMessage("Trip Allowance is Required.");
        //RuleFor(x => x.TripAllowance)
        //    .GreaterThanOrEqualTo(0).WithMessage("Trip Allowance Cannot be Negative.")
        //    .When(x => x.TripAllowance.HasValue); //for optional 

        RuleFor(x => x.MaintenanceFee).NotEmpty().WithMessage("Maintenance Fee is Required.");
        //RuleFor(x => x.MaintenanceFee)
        //    .GreaterThanOrEqualTo(0).WithMessage("Maintenance Fee Cannot be Negative.")
        //    .When(x => x.MaintenanceFee.HasValue); //for optional

        RuleFor(x => x.DriverAllownace).NotEmpty().WithMessage("Maintenance Fee is Required.");
        

        //RuleFor(x => x.DriverAllownace)
        //    .GreaterThanOrEqualTo(0).WithMessage("Driver Allownace Fee Cannot be Negative.")
        //    .When(x => x.DriverAllownace.HasValue); //for Optional

        //RuleFor(x => x.DriverId).NotEmpty().WithMessage("Driver is Required.");
        //RuleFor(x => x.VehicleId).NotEmpty().WithMessage("Vehicle is Required.");
    }
}