using Application.Interfaces.Repositories;
using FluentValidation;

namespace Application.Features.Vehicles.Command.AddVehicles;

public class VehicleCommandValidator : AbstractValidator<VehiclesCommand>
{
    private readonly IDriversRepository _driversRepository;
    private readonly IVehicleRepository _vehicleRepository;
    private readonly ITripRequestRepository _tripRequestRepository;
    public VehicleCommandValidator(
        IDriversRepository driversRepository,
        IVehicleRepository vehicleRepository,
        ITripRequestRepository tripRequestRepository)
    {
        _driversRepository = driversRepository;
        _vehicleRepository = vehicleRepository;
        _tripRequestRepository = tripRequestRepository;

        //RuleFor(x => x.DriverId)
        //    .MustAsync(async (driver, cancellation) => await _tripRequestRepository.AnyAsync(x => x.DriverId == driver))
        //    .WithMessage("Driver With License Number not Found.");
        //RuleFor(x => x.licenseNumber)
        //    .MustAsync(async (driverId, cancellation) => await _driversRepository.AnyAsync(x => x.LicenseNumber == driverId))
        //    .WithMessage("Driver with license Number doesnot exists.");

        RuleFor(x => x.VehicleOwner).NotEmpty().WithMessage("Owner Name is Required.")
            .MinimumLength(3).WithMessage("Name must be 3 Character long");

        RuleFor(x => x.VehicleNumber).NotEmpty().WithMessage("Vehicel Number is Required.")
            .MustAsync(async (vehicleNumber, cancellation) => !await _vehicleRepository.AnyAsync(x => x.VehicleNumber == vehicleNumber))
            .WithMessage("Vehicle Number already exists.");

        RuleFor(x => x.VehicleName).NotEmpty().WithMessage("Vehicel Name is Required.");
        RuleFor(x => x.VehicleType).NotEmpty().WithMessage("Vehicel Type is Required.");
        RuleFor(x => x.VehicleCapacity).NotEmpty().WithMessage("Vehicel Capacity is Required.");
        RuleFor(x => x.VehicleVolume).NotEmpty().WithMessage("Vehicel Volume is Required.");
        RuleFor(x => x.Status).NotEmpty().WithMessage("Status is Required.");



        //RuleFor(x => x.VehicleImage).NotEmpty().WithMessage("Image is Required.");

    }
}