namespace Application.Features.Trips.Command.DeleteTrip;

public record TripDeleteCommand(int TripId) : IRequest<IResult>;
