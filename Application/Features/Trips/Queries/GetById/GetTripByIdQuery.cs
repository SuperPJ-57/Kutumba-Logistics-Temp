using Domain.Logistic;

namespace Application.Features.Trips.Queries.GetById;

public record GetTripByIdQuery(
    int TripId) : IRequest<IResult>;
