using Application.Interfaces.Repositories;
using FluentValidation;

namespace Application.Features.Vehicles.Command.UpdateVehicles;

public class VehicleUpdateCommandValidator : AbstractValidator<VehiclesUpdateCommand>
{
    private readonly IDriversRepository _driversRepository;
    private readonly IVehicleRepository _vehicleRepository;
    public VehicleUpdateCommandValidator(IDriversRepository driversRepository, IVehicleRepository vehicleRepository)
    {
        _driversRepository = driversRepository;
        _vehicleRepository = vehicleRepository;

        RuleFor(x => x.DriverId)
            .MustAsync(async (driverId, cancellation) => await _driversRepository.AnyAsync(x => x.DriverId == driverId))
            .WithMessage("Driver not Found.");
        RuleFor(x => x.VehicleOwner).NotEmpty().WithMessage("Owner Name is Required.")
            .MinimumLength(3).WithMessage("Name must be 3 Character long");

        RuleFor(x => x.VehicleNumber).NotEmpty().WithMessage("Vehicel Number is Required.")
            .MustAsync(async (vehicleNumber, cancellation) => !await _vehicleRepository.AnyAsync(x => x.VehicleName == vehicleNumber))
            .WithMessage("Vehicle Number already exists.");

        RuleFor(x => x.VehicleName).NotEmpty().WithMessage("Vehicel Name is Required.");
        RuleFor(x => x.VehicleType).NotEmpty().WithMessage("Vehicel Type is Required.");
        RuleFor(x => x.VehicleCapacity).NotEmpty().WithMessage("Vehicel Capacity is Required.");
        RuleFor(x => x.VehicleVolume).NotEmpty().WithMessage("Vehicel Volume is Required.");
        //RuleFor(x => x.VehiclePathImage).NotEmpty().WithMessage("Vehicle Image is Required.");
        RuleFor(x => x.Status).NotEmpty().WithMessage("Status is Required.");
        
    }
}