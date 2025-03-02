using FluentValidation;

namespace Application.Features.Authentication.Commands.Register;

public class RegisterCommandVaildator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandVaildator()
    {
        RuleFor(x => x.FirstName).NotEmpty().WithMessage("FirstName is Required.")
            .MinimumLength(3).WithMessage("FirstName must be 3 character long");

        RuleFor(x => x.LastName).NotEmpty().WithMessage("LastName is Required.")
            .MinimumLength(3).WithMessage("LastName must be 3 character long");

        RuleFor(x => x.Email).NotEmpty().WithMessage("Email is Required.")
            .EmailAddress().WithMessage("Invalid Email Address.");

        RuleFor(x => x.Password).NotEmpty().WithMessage("Password is Required")
            .MinimumLength(8).WithMessage("Password must be 8 character long.");

    }
}
