using Application.Interfaces.Repositories;
using Domain.Logistic;
using Microsoft.Extensions.Logging;


namespace Application.Features.Consignments.Command.UpdateConsignment;

public class ConsignmentsUpdateCommandHandler : IRequestHandler<ConsignmentsUpdateCommand, IResult>
{
    private readonly IConsignmentRepository _consignmentDetails;
    private readonly ITripRequestRepository _tripRequestRepository;
    private readonly ILogger<ConsignmentsUpdateCommandHandler> _logger;


    public ConsignmentsUpdateCommandHandler(
        IConsignmentRepository consignmentDetails,
        ITripRequestRepository tripRequestRepository,
        ILogger<ConsignmentsUpdateCommandHandler> logger)
    {
        _consignmentDetails = consignmentDetails;
        _tripRequestRepository = tripRequestRepository;
        _logger = logger;
    }

    public async Task<IResult> Handle(ConsignmentsUpdateCommand request, CancellationToken cancellationToken)
    {

        var consignmentDetails =  await _consignmentDetails.FirstOrDefaultAsync(x => x.ConsignmentId == request.ConsignmentId);
        if(consignmentDetails is null)
        {
            return Results.NotFound(new
            {
                Message = "Consignment Not Found."
            });
        }

        consignmentDetails.ConsignmentId = request.ConsignmentId;
        consignmentDetails.PartyName = request.PartyName;
        consignmentDetails.PartyContact = request.PartyContact;
        consignmentDetails.GoodDescription = request.GoodDescription;
        consignmentDetails.TotalWeight = request.TotalWeight;
        consignmentDetails.LoadingPoint = request.LoadingPoint;
        consignmentDetails.UnloadingPoint = request.UnloadingPoint;

        await _consignmentDetails.UpdateAsync(consignmentDetails);
        await _consignmentDetails.SaveChangesAsync(cancellationToken);
        var tripRequest = new TripRequest
        {
            ConsignmentId = consignmentDetails.ConsignmentId,
            DriverId = request.DriverId,
            VehicleId = request.VehicleId,
            FreightId = request.FreightId,
            Id = request.Id
        };

        await _tripRequestRepository.UpdateAsync(tripRequest);
        await _tripRequestRepository.SaveChangesAsync(cancellationToken);



        return Results.Ok(new
        {
            Message = "Consignment Update Successfully.",
            ConsignmentDetail = consignmentDetails
        });

    }
}
