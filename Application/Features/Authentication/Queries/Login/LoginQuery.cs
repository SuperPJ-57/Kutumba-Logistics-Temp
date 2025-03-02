namespace Application.Features.Authentication.Queries.Login;

public record LoginQuery(
    string Email,
    string Password) : IRequest<IResult>;
