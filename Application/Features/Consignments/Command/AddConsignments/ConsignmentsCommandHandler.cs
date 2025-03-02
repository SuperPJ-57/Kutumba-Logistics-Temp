
using Application.Interfaces.Repositories;
using Domain.Logistic;
using Microsoft.Extensions.Logging;

namespace Application.Features.Consignments.Command.AddConsignment;

public class ConsignmentsCommandHandler : IRequestHandler<ConsignmentsCommand, IResult>
{
    private readonly IConsignmentRepository _consignmentDetails;
    private readonly ILogger<ConsignmentsCommandHandler> _logger;
    private readonly ITripRequestRepository _tripRequestRepository;

    public ConsignmentsCommandHandler(
        IConsignmentRepository consignmentDetails,
        ILogger<ConsignmentsCommandHandler> logger,
        ITripRequestRepository tripRequestRepository)
    {
        _consignmentDetails = consignmentDetails;
        _logger = logger;
        _tripRequestRepository = tripRequestRepository;
    }

    public async Task<IResult> Handle(ConsignmentsCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("{FunctionName}, received request to create consignment with data : {@RequestData}",
            nameof(ConsignmentsCommandHandler), request);
        try
        {
            var consignmentDetail = new Consignment
            {
                PartyContact = request.PartyContact,
                PartyName = request.PartyName,
                GoodDescription = request.GoodDescription,
                TotalWeight = request.TotalWeight,
                LoadingPoint = request.LoadingPoint,
                UnloadingPoint = request.UnloadingPoint,
            };
            await _consignmentDetails.AddAsync(consignmentDetail);
            await _consignmentDetails.SaveChangesAsync(cancellationToken);

            var tripRequest = new TripRequest
            {
                Id = request.Id,
                DriverId = request.DriverId,
                VehicleId = request.VehicleId,
                FreightId = request.FreightId,
                ConsignmentId = consignmentDetail.ConsignmentId,
            };

            await _tripRequestRepository.UpdateAsync(tripRequest);
            await _tripRequestRepository.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("{FunctionName}, Consignment created successfully with request data : {@RequestData}",
                nameof(ConsignmentsCommandHandler), request);

            return Results.Ok(new
            {
                Message = "Consignment Created Successfully.",
                ConsignmentDetail = consignmentDetail
            });
        }
        catch (Exception ex)
        {
            _logger.LogInformation(ex, "{FunctionName}, failed to created consigment with requested data: {@RequestData}",
                nameof(ConsignmentsCommandHandler), request);
            return Results.BadRequest(new
            {
                Message = "Failed to created consignment. Please try again."
            });
        }
    }
}
