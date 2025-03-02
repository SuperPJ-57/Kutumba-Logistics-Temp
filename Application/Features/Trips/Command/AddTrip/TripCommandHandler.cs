
using Application.Interfaces.Repositories;
using Domain.Logistic;
using Microsoft.Extensions.Logging;

namespace Application.Features.Trips.Command.AddTrip;

public class TripCommandHandler : IRequestHandler<TripCommand, IResult>
{
    private readonly ITripRepository _tripRepository;
    private readonly ILogger<TripCommandHandler> _logger;
    private readonly ITripRequestRepository _tripRequestRepository;

    public TripCommandHandler(
        ITripRepository tripRepository,
        ILogger<TripCommandHandler> logger,
        ITripRequestRepository tripRequestRepository)
    {
        _tripRepository = tripRepository;
        _logger = logger;
        _tripRequestRepository = tripRequestRepository;
    }

    public async Task<IResult> Handle(TripCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("{FunctionName}, received request to create data : {@RequestData}",
            nameof(TripCommandHandler), request);
        try
        {
            var trip = new Trip
            {

                TripAllowance = request.TripAllowance,
                StartDate = request.StartDate,
                DeliveryDate = request.DeliveryDate,
                MaintenanceFee = request.MaintenanceFee,
                DriverAllownace = request.DriverAllownace,

            };
            await _tripRepository.AddAsync(trip);
            await _tripRepository.SaveChangesAsync(cancellationToken);

            var tripRequest = new TripRequest
            {
                TripId = trip.TripId,
                DriverId = request.DriverId,
                VehicleId = request.VehicleId,
                ConsignmentId = request.ConsignmentId,
                FreightId = request.FreightId,
                Id = request.Id,
            };

            await _tripRequestRepository.UpdateAsync(tripRequest);
            await _tripRequestRepository.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("{FunctionName}, Successfully created trip with data : {@RequestData}",
                nameof(TripCommandHandler), request);
            return Results.Ok(new
            {
                Message = "Trip Created Successfully.",
                response = trip
            });
        }
        catch (Exception ex)
        { 
            _logger.LogInformation(ex, "{FunctionName}, Failed to create trip with data : {@RequestData}",
                nameof(TripCommandHandler), request);
            return Results.BadRequest(new
            {
                Message = "failed to create trip. Please try again" 
            });
        }

    }
}
