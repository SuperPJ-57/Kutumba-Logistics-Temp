
using Application.Interfaces.Repositories;
using FluentValidation;

namespace Application.Features.Consignments.Command.AddConsignment;

public class ConsignmentsCommandValidator : AbstractValidator<ConsignmentsCommand>
{
    private readonly ITripRequestRepository _tripRequestRepository;
    public ConsignmentsCommandValidator(ITripRequestRepository tripRequestRepository)
    {
        _tripRequestRepository = tripRequestRepository;

        RuleFor(x => x.DriverId)
            .MustAsync(async (driverId, cancellation) => await _tripRequestRepository.AnyAsync(x => x.DriverId == driverId))
            .WithMessage("Driver doesnot exists.");
        RuleFor(x => x.VehicleId)
            .MustAsync(async (vehicleId, cancellation) => await _tripRequestRepository.AnyAsync(x => x.VehicleId == vehicleId))
            .WithMessage("Vehicle doesnot exists.");

        RuleFor(x => x.FreightId)
            .MustAsync(async (freightId, cancellation) => await _tripRequestRepository.AnyAsync(x => x.FreightId == freightId))
            .WithMessage("Freight doesnot exists.");

        RuleFor(x => x.PartyName).NotEmpty().WithMessage("Party Name is Requird.");
        RuleFor(x => x.PartyContact).NotEmpty().WithMessage("Party Conatact Number is Required.");
        RuleFor(x => x.GoodDescription).NotEmpty().WithMessage("Good Description is Required.");
        RuleFor(x => x.TotalWeight).NotEmpty().WithMessage("Total Weight is Required.");
        RuleFor(x => x.LoadingPoint).NotEmpty().WithMessage("Loading Point is Required.");
        RuleFor(x => x.UnloadingPoint).NotEmpty().WithMessage("Unloading point is Required.");
        
        
    }
}
