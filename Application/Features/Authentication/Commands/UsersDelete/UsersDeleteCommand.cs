namespace Application.Features.Authentication.Commands.UsersDelete;

public record UsersDeleteCommand(Guid Id) : IRequest<IResult>;
