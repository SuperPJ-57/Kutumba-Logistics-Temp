namespace Application.Features.TripRequests.Command.DeleteTripRequest;

public record TripRequestDeleteCommand(int Id) : IRequest<IResult>;
