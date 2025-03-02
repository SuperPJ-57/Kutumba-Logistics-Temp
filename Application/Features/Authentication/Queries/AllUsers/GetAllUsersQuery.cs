namespace Application.Features.Authentication.Queries.AllUsers;

public record GetAllUsersQuery(int PageNumber, int PageSize) : IRequest<IResult>;

