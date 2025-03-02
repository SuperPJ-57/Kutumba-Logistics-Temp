namespace Application.Features.Trips.Queries.GetAll;

public record GetAllTripQuery(int PageNumber, int PageSize) : IRequest<IResult>;
