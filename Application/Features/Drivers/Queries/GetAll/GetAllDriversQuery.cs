
namespace Application.Features.Drivers.Queries.GetAll;

public record GetAllDriversQuery(int PageNumber, int PageSize) : IRequest<IResult>;

