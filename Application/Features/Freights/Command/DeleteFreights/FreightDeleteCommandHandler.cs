
using Application.Interfaces.Repositories;
using Microsoft.Extensions.Logging;

namespace Application.Features.Freights.Command.DeleteFreights;

public class FreightDeleteCommandHandler : IRequestHandler<FreightDeleteCommand, IResult>
{
    private readonly IFreightRepository _freightRepository;
    private readonly ILogger<FreightDeleteCommandHandler> _logger;

    public FreightDeleteCommandHandler(IFreightRepository freightRepository, ILogger<FreightDeleteCommandHandler> logger)
    {
        _freightRepository = freightRepository;
        _logger = logger;
    }

    public async Task<IResult> Handle(FreightDeleteCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("{FunctionName}, recevied request to delete freight with. Id : {@Requestdata}",
            nameof(FreightDeleteCommandHandler), request);
        try
        {
            var existingFreight = await _freightRepository.FirstOrDefaultAsync(x => x.FreightId == request.FreightId);
            if (existingFreight is null)
            {
                _logger.LogWarning("{FunctionName}, failed to delete Freight with id: {@RequestData}",
                    nameof(FreightDeleteCommandHandler), request);

                return Results.NotFound(new
                {
                    Message = "Freight not found."
                });
            }

            await _freightRepository.RemoveAsync(existingFreight);

            existingFreight.IsActive = false;
            await _freightRepository.UpdateAsync(existingFreight);

            await _freightRepository.SaveChangesAsync();
            _logger.LogInformation("{FunctionName},Freight With id is deleted successfully : {@RequestData}",
                nameof(FreightDeleteCommandHandler), request);

            return Results.Ok(new
            {
                Message = "Freight Deleted successfully."
            });
        }
        catch (Exception ex)
        {
            _logger.LogInformation(ex, "{FunctionName}, Failed to delete freight with id: {@RequestData}",
                nameof(FreightDeleteCommandHandler), request);
            return Results.BadRequest(new
            {
                Message = "Failed to delete freight. Please try again."
            });
        }
    }
}
