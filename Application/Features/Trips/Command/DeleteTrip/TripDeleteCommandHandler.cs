using Application.Interfaces.Repositories;
using Microsoft.Extensions.Logging;


namespace Application.Features.Trips.Command.DeleteTrip;

public class TripDeleteCommandHandler : IRequestHandler<TripDeleteCommand, IResult>
{
    private readonly ITripRepository _tripRepository;
    private readonly ILogger<TripDeleteCommandHandler> _logger;

    public TripDeleteCommandHandler(ITripRepository tripRepository, ILogger<TripDeleteCommandHandler> logger)
    {
        _tripRepository = tripRepository;
        _logger = logger;
    }

    public async Task<IResult> Handle(TripDeleteCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("{FunctionName}, received request for delete with Id : {@RequestData}",
            nameof(TripDeleteCommandHandler), request);

        try
        {
            var trip = await _tripRepository.FirstOrDefaultAsync(x => x.TripId == request.TripId);
            if (trip is null)
            {
                _logger.LogWarning("{FunctionName}, failed to retrieve Trip with Id : {@RequestData}",
                    nameof(TripDeleteCommandHandler), request);

                return Results.NotFound(new
                {
                    Message = "Trip not found."
                });
            }

            _tripRepository.Remove(trip);

            trip.IsActive = false;
            await _tripRepository.UpdateAsync(trip);

            await _tripRepository.SaveChangesAsync();

            _logger.LogInformation("{FunctionName}, Successfully delete trip with request Id : {@RequestData}",
                nameof(TripDeleteCommandHandler), request);

            return Results.Ok(new
            {
                Message = "Trip deleted successfully."
            });
        }
        catch (Exception ex)
        {
            _logger.LogInformation(ex, "{FunctionName}, Failed to created trip with data : {@RequestData}",
                nameof(TripDeleteCommandHandler), request);
            return Results.BadRequest(new
            {
                Message = "failed to create trip. Please try again."
            });
        }


    }
}
