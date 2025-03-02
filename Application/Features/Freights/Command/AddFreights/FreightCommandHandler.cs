
using Application.Interfaces.Repositories;
using Domain.Logistic;
using Microsoft.Extensions.Logging;


namespace Application.Features.Freights.Command.AddFreights;

public class FreightCommandHandler : IRequestHandler<FreightCommand, IResult>
{
    private readonly IFreightRepository _freightRepository;
    private readonly ILogger<FreightCommandHandler> _logger;
    private readonly ITripRequestRepository _tripRequestRepository;

    public FreightCommandHandler(
        IFreightRepository freightRepository,
        ILogger<FreightCommandHandler> logger,
        ITripRequestRepository tripRequestRepository)
    {
        _freightRepository = freightRepository;
        _logger = logger;
        _tripRequestRepository = tripRequestRepository;
    }
    public async Task<IResult> Handle(FreightCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("{FunctionName}, received request to created freight with : {@RequestData}",
            nameof(FreightCommandHandler), request);
        try
        {
            var freightDetail = new Freight
            {
                FreightId = request.FreightId,
                FreightRate = request.FreightRate,
                AdvancedPaid = request.AdvancedPaid,
                RemainingAmount = request.RemainingAmount,
                PaymentMode = request.PaymentMode,
            };

            await _freightRepository.AddAsync(freightDetail);
            await _freightRepository.SaveChangesAsync();


            var tripRequest = new TripRequest
            {
                Id = request.Id,
                DriverId = request.DriverId,
                VehicleId = request.VehicleId,
                FreightId = freightDetail.FreightId
            };
            await _tripRequestRepository.UpdateAsync(tripRequest);
            await _tripRequestRepository.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("{FunctionName}, Freight is created successfully with Detail : {@RequestData}",
                nameof(FreightCommandHandler), request);

            return Results.Ok(new
            {
                Message = "Freight add Successfully.",
                response = freightDetail
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,"{FunctionName}, failed to created freight with Details : {@RequestData}",
                nameof(FreightCommandHandler), request);
            return Results.BadRequest(new
            {
                Message = "Failed to created Freight. Please try again."
            });
        }
    }
}
