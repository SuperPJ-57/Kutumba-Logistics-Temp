
using Application.Interfaces.Repositories;
using FluentValidation;

namespace Application.Features.Drivers.Command.AddDrivers;

public class DriversCommandValidator : AbstractValidator<DriversCommand>
{
    private readonly ITripRepository _tripRepository;
    private readonly IDriversRepository _driverRepository;
    public DriversCommandValidator(ITripRepository tripRepository, IDriversRepository driverRepository)
    {
        _tripRepository = tripRepository;
        _driverRepository = driverRepository;

        //RuleFor(x => x.TripId).
        //    MustAsync(async (TripId, cancellationToken) => await _tripRepository.AnyAsync(x => x.TripId == TripId)).WithMessage("Trip not found");
        //RuleFor(x => x.DriverDto.LastName).NotEmpty().WithMessage("FirstName is Required.")
        //.MinimumLength(3).WithMessage("FirstName must 3 character long.");

        RuleFor(x => x.FirstName).NotEmpty().WithMessage("LastName is Required.")
        .MinimumLength(3).WithMessage("FirstName must 3 character long.");


        RuleFor(x => x.LastName).NotEmpty().WithMessage("LastName is Required.")
        .MinimumLength(3).WithMessage("FirstName must 3 character long.");

        RuleFor(x => x.Contact).NotEmpty().WithMessage("Contact Number is Required.")
        .MinimumLength(10).WithMessage("Invalid Phone Number")
        .MaximumLength(14).WithMessage("Invalid Phone Nummber").
        MustAsync(async (phone, cancellaction) => !await _driverRepository.AnyAsync(x => x.Contact == phone))
        .WithMessage("Driver with Phone Number already exists.");

        RuleFor(x => x.LicenseNumber)
            .NotEmpty().WithMessage("License Number is required.")
            .MustAsync(async (licenseNumber , cancellation) => !await _driverRepository.AnyAsync(x => x.LicenseNumber == licenseNumber))
            .WithMessage("Driver with License Number already exists.");

        RuleFor(x => x.DateOfBirth).NotEmpty().WithMessage("Date of Birth is Required.");

        //RuleFor(x => x.ProfileImage).NotEmpty().WithMessage("Profile Image is Required");
        RuleFor(x => x.Status).NotEmpty().WithMessage("Status is Required");
        
    }
}