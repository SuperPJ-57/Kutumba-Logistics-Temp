
using Application.Interfaces.Repositories;
using Domain.Logistic;
using Microsoft.Extensions.Logging;

namespace Application.Features.Trips.Command.UpdateTrip;

public class TripUpdateCommandHandler : IRequestHandler<TripUpdateCommand, IResult>
{
    private readonly ITripRepository _tripRepository;
    private readonly ITripRequestRepository _tripRequestRepository;
    private readonly ILogger<TripUpdateCommandHandler> _logger;

    public TripUpdateCommandHandler(
        ITripRepository tripRepository,
        ILogger<TripUpdateCommandHandler> logger,
        ITripRequestRepository tripRequestRepository)
    {
        _tripRepository = tripRepository;
        _logger = logger;
        _tripRequestRepository = tripRequestRepository;
    }

    public async Task<IResult> Handle(TripUpdateCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("{FunctionName}, received request to update trip with data : {@RequestData}",
            nameof(TripUpdateCommandHandler), request);
        try
        {
            var existingTrip = await _tripRepository.GetByIdAsync(request.TripId);
            if (existingTrip is null)
            {
                _logger.LogWarning("{FunctionName}, failed to retrieve trip id with : {@RequestData}",
                    nameof(TripUpdateCommandHandler), request);

                return Results.NotFound(new
                {
                    Message = "Trip Not Found."
                });
            }
            existingTrip.StartDate = request.StartDate;
            existingTrip.DeliveryDate = request.DeliveryDate;
            existingTrip.MaintenanceFee = request.MaintenanceFee;
            existingTrip.TripAllowance = request.TripAllowance;
            existingTrip.DriverAllownace = request.DriverAllownace;

            await _tripRepository.UpdateAsync(existingTrip);
            await _tripRepository.SaveChangesAsync();

            var tripRequest = new TripRequest
            {
                TripId = existingTrip.TripId,
                DriverId = request.DriverId,
                VehicleId = request.VehicleId,
                ConsignmentId = request.ConsignmentId
            };

            await _tripRequestRepository.UpdateAsync(tripRequest);
            await _tripRequestRepository.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("{FunctionName}, successfully update trip with request data : {@Request}",
                nameof(TripUpdateCommandHandler), request);

            return Results.Ok(new
            {
                tripDetails = existingTrip,
                Message = "trip update successfully."
            });
        }
        catch (Exception ex)
        {
            _logger.LogInformation(ex, "{FunctionName}, failed to update trip with data : {@RequestData}", 
                nameof(TripUpdateCommandHandler), request);
            return Results.BadRequest(new
            {
                Message = "Failed to update trip. Please try again."
            });
        }
    }
}
