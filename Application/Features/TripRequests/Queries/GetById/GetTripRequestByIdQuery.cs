namespace Application.Features.TripRequests.Queries.GetById;

public record GetTripRequestByIdQuery(int Id) : IRequest<IResult>;
