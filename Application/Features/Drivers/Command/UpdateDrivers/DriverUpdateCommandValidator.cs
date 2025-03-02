using FluentValidation;

namespace Application.Features.Drivers.Command.UpdateDrivers;

public class DriversUpdateCommandValidator : AbstractValidator<DriversUpdateCommand>
{
    public DriversUpdateCommandValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty().WithMessage("FirstName is Required.")
            .MinimumLength(3).WithMessage("FirstName must 3 character long.");

        RuleFor(x => x.LastName).NotEmpty().WithMessage("LastName is Required.")
            .MinimumLength(3).WithMessage("FirstName must 3 character long.");

        RuleFor(x => x.Contact).NotEmpty().WithMessage("Contact Number is Required.")
            .MinimumLength(10).WithMessage("Invalid Phone Number")
            .MaximumLength(14).WithMessage("Invalid Phone Nummber");

        RuleFor(x => x.LicenseNumber).NotEmpty().WithMessage("License Number is Required.");

        RuleFor(x => x.DateOfBirth).NotEmpty().WithMessage("Date of Birth is Required.");

        //RuleFor(x => x.ProfileImage).NotEmpty().WithMessage("Profile Image is Required");
        RuleFor(x => x.Status).NotEmpty().WithMessage("Status is Required");
    }
}