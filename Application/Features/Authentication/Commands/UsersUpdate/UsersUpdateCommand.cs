namespace Application.Features.Authentication.Commands.UsersUpdate;

public record UsersUpdateCommand(
    Guid Id,
    string FirstName,
    string LastName,
    string Email) : IRequest<IResult>;


