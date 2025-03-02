namespace Application.Features.Authentication.Queries.UsersById;

public record GetUsersByIdQuery(Guid Id) : IRequest<IResult>;

