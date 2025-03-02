using FluentValidation;

namespace Application.Features.Authentication.Commands.UsersUpdate;

public class UsersUpdateCommandVaildator : AbstractValidator<UsersUpdateCommand>
{
    public UsersUpdateCommandVaildator()
    {
        RuleFor(x => x.FirstName).NotEmpty().WithMessage("FirstName is Required.")
            .MinimumLength(3).WithMessage("FirstName must be 3 character long.");

        RuleFor(x => x.LastName).NotEmpty().WithMessage("LastName is Required.")
            .MinimumLength(3).WithMessage("FirstName must be 3 character long.");

        RuleFor(x => x.Email).NotEmpty().WithMessage("Email is Required.")
            .EmailAddress().WithMessage("Invalid Email Address.");
    }
}
