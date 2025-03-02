namespace Application.Features.TripRequests.Queries.GetAll;

public record GetAllTripRequestQuery(int PageNumber,int PageSize) : IRequest<IResult>;
