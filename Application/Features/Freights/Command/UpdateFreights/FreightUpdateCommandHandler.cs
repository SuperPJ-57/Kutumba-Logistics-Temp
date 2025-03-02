
using Application.Interfaces.Repositories;
using Domain.Logistic;
using Microsoft.Extensions.Logging;

namespace Application.Features.Freights.Command.UpdateFreights;

public class FreightUpdateCommandHandler : IRequestHandler<FreightUpdateCommand, IResult>
{
    private readonly IFreightRepository _freightRepository;
    private readonly ILogger<FreightUpdateCommandHandler> _logger;
    private readonly ITripRequestRepository _tripRequestRepository;
    public FreightUpdateCommandHandler(
        IFreightRepository freightRepository,
        ILogger<FreightUpdateCommandHandler> logger,
        ITripRequestRepository tripRequestRepository)
    {
        _freightRepository = freightRepository;
        _logger = logger;
        _tripRequestRepository = tripRequestRepository;
    }

    public async Task<IResult> Handle(FreightUpdateCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("{FunctionName}, received request for update with data : {@RequestData}",
            nameof(FreightUpdateCommandHandler), request);
        try
        {
            var existingFreightDetails = await _freightRepository.GetByIdAsync(request.FreightId);
            if (existingFreightDetails is null)
            {
                _logger.LogWarning("{FunctionName}, failed to update with request data : {@RequestData}",
                    nameof(FreightUpdateCommandHandler), request);
                return Results.NotFound(new
                {
                    Message = "Freight Details not found."
                });
            }
            existingFreightDetails.FreightId = request.FreightId;
            existingFreightDetails.FreightRate = request.FreightRate;
            existingFreightDetails.AdvancedPaid = request.AdvancedPaid;
            existingFreightDetails.RemainingAmount = request.RemainingAmount;
            existingFreightDetails.PaymentMode = request.PaymentMode;

            await _freightRepository.UpdateAsync(existingFreightDetails);
            await _freightRepository.SaveChangesAsync();

            var tripRequestUpdate = new TripRequest
            {
                Id = request.FreightId,
                DriverId = request.DriverId,
                VehicleId = request.VehicleId,
                FreightId = existingFreightDetails.FreightId,
            };

            await _tripRequestRepository.UpdateAsync(tripRequestUpdate);
            await _tripRequestRepository.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("{FunctionName}, freight update successfully with data : {@RequestData}",
                nameof(FreightUpdateCommandHandler), request);

            return Results.Ok(new
            {
                Message = "Updated successfully",
                Freight = existingFreightDetails,
            });
        }
        catch (Exception ex)
        {
            _logger.LogInformation("{FunctionName}, failed to update freight with data : {@Requestdata}",
                nameof(FreightUpdateCommandHandler), request);
            return Results.BadRequest(new
            {
                Message = "Failed to update Freight. Please try again."
            });
        }

    }
}
