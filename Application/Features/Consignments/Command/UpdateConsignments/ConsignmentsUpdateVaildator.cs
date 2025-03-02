using Application.Interfaces.Repositories;
using FluentValidation;

namespace Application.Features.Consignments.Command.UpdateConsignment;

public class ConsignmentDetailsUpdateValidator : AbstractValidator<ConsignmentsUpdateCommand>
{
    private readonly ITripRequestRepository _tripRequestRepository;
    public ConsignmentDetailsUpdateValidator(ITripRequestRepository tripRequestRepository)
    {
        _tripRequestRepository = tripRequestRepository;

        RuleFor(x => x.PartyName).NotEmpty().WithMessage("Party Name is Requird.");
        RuleFor(x => x.PartyContact).NotEmpty().WithMessage("Party Conatact Number is Required.");
        RuleFor(x => x.GoodDescription).NotEmpty().WithMessage("Good Description is Required.");
        RuleFor(x => x.TotalWeight).NotEmpty().WithMessage("Total Weight is Required.");
        RuleFor(x => x.LoadingPoint).NotEmpty().WithMessage("Loading Point is Required.");
        RuleFor(x => x.UnloadingPoint).NotEmpty().WithMessage("Unloading point is Required.");
        RuleFor(x => x.VehicleId).NotEmpty().WithMessage("Trip Id is Required.")
            .MustAsync(async (vehicle, cancellation) => await _tripRequestRepository.AnyAsync(x => x.VehicleId == vehicle))
            .WithMessage("Vehicle Doesnot exists.");
        RuleFor(x => x.FreightId).NotEmpty().WithMessage("Freight Id is required.")
            .MustAsync(async (freight , cancellation) => await _tripRequestRepository.AnyAsync(x => x.FreightId == freight));
        RuleFor(x => x.DriverId).NotEmpty().WithMessage("Driver is Required.")
            .MustAsync(async (driver, cancellation) => await _tripRequestRepository.AnyAsync(x => x.DriverId == driver))
            .WithMessage("Driver doesnot exists.");
    }
}
