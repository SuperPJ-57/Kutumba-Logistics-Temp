
using Application.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Features.TripRequests.Command.DeleteTripRequest;

public class TripRequestDeleteCommandHandler : IRequestHandler<TripRequestDeleteCommand, IResult>
{
    private readonly ITripRequestRepository _tripRequestRepository;
    private readonly ILogger<TripRequestDeleteCommandHandler> _logger;
    public TripRequestDeleteCommandHandler(
        ITripRequestRepository tripRequestRepository,
        ILogger<TripRequestDeleteCommandHandler> logger)
    {
        _tripRequestRepository = tripRequestRepository;
        _logger = logger;
    }

    public async Task<IResult> Handle(TripRequestDeleteCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("{FUnctioName}, recevied delete request with Id : {@RequestData}",
            nameof(TripRequestDeleteCommandHandler), request);

        try
        {
            var tripRequest = await _tripRequestRepository.Queryable.FirstOrDefaultAsync(tr => tr.Id == request.Id);
            if (tripRequest is null)
            {
                _logger.LogWarning("{FunctionName}, Failed to Retrieve TripRequest with Id : {@Requestdata}",
                    nameof(TripRequestDeleteCommandHandler), request);
                return Results.NotFound(new
                {
                    Message = "Failed to Retrieve Trip Request with Id."
                });
            }
            _logger.LogInformation("{FunctionName}, Successfully delete TripRequest with Id : {@TripRequestDetails}",
                nameof(TripRequestDeleteCommandHandler), request);


            _tripRequestRepository.Remove(tripRequest);

            tripRequest.IsActive = false;
            await _tripRequestRepository.UpdateAsync(tripRequest);

            await _tripRequestRepository.SaveChangesAsync(cancellationToken);

            return Results.Ok(new
            {
                Message = "Successfully deleted TripRequest."
            });
        }
        catch (Exception ex)
        {
            _logger.LogInformation(ex, "{FunctionName}, Failed to delete TripRequest with Id : {@RequestData}",
                nameof(TripRequestDeleteCommandHandler), request);
            return Results.BadRequest(new
            {
                Message = "Failed to Delete TripRequest. Please try again."
            });
        }
    }
}
