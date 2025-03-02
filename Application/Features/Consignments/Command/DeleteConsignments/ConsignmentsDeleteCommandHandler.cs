

using Application.Interfaces.Repositories;
using Microsoft.Extensions.Logging;

namespace Application.Features.Consignments.Command.DeleteConsignment;

public class ConsignmentsDeleteCommandHandler : IRequestHandler<ConsignmentsDeleteCommand, IResult>
{
    private readonly IConsignmentRepository _consignmentDetails;
    private readonly ILogger<ConsignmentsDeleteCommandHandler> _logger;

    public ConsignmentsDeleteCommandHandler(IConsignmentRepository consignmentDetails, ILogger<ConsignmentsDeleteCommandHandler> logger)
    {
        _consignmentDetails = consignmentDetails;
        _logger = logger;
    }

    public async Task<IResult> Handle(ConsignmentsDeleteCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("{FunctionName}, request received to delete consignment with Id : {@RequestData}",
            nameof(ConsignmentsDeleteCommandHandler), request);
        try
        {
            var consignment = await _consignmentDetails.GetByIdAsync(request.ConsignmentId);
            if (consignment is null)
            {
                _logger.LogWarning("{FunctionName}, failed to retrieve consignment with Id",
                    nameof(ConsignmentsDeleteCommandHandler));
                return Results.NotFound(new
                {
                    Message = "Consignment Not Found."
                });
            }
            consignment.IsActive = false;
            await _consignmentDetails.UpdateAsync(consignment);
            await _consignmentDetails.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("{FunctionName}, Successfully delete consignment with Id {@RequestData}",
                nameof(ConsignmentsDeleteCommandHandler), request);
            return Results.Ok(new
            {
                Messge = "Consignment Deleted."
            });
        }
        catch (Exception ex)
        {
            _logger.LogInformation(ex,"{FunctionName}, failed to delete consignment with Id",
                nameof(ConsignmentsDeleteCommandHandler));
            return Results.BadRequest(new
            {

                Message = "Failed to delete consignment. Please try again."
            });
        }
    }
}
