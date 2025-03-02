
using Domain.Logistic;



namespace Application.Features.TripRequests.Command.AddTripRequest;

public record TripRequestCommand(TripRequest TripRequest) : IRequest<IResult>;
