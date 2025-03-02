
using Application.Dto.Consignments;
using Application.Dto.Drivers;
using Application.Dto.Freights;
using Application.Dto.TripRequests;
using Application.Dto.Vehicles;
using Application.Features.Consignments.Queries.GetById;
using Application.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Features.ConsignmentDetails.Queries.GetById;

public class ConsignmentsByIdQueryHandler : IRequestHandler<GetConsignmentsByIdQuery, IResult>
{
    private readonly IConsignmentRepository _consignmentDetails;
    private readonly ILogger<ConsignmentsByIdQueryHandler> _logger;


    public ConsignmentsByIdQueryHandler(IConsignmentRepository consignmentDetails, ILogger<ConsignmentsByIdQueryHandler> logger)
    {
        _consignmentDetails = consignmentDetails;
        _logger = logger;
    }

    public async Task<IResult> Handle(GetConsignmentsByIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("{FunctionName}, received request with consignment Id : {@RequestData}",
            nameof(ConsignmentsByIdQueryHandler), request);
        try
        {
            var consignmentDetails = await _consignmentDetails.Queryable
                .AsNoTracking()
                //.Where(c => c.ConsignmentId == request.ConsignmentId)
                .OrderBy(c => c.PartyName)
                .Select(c => new ConsignmentDto
                {
                    ConsignmentId = c.ConsignmentId,
                    PartyName = c.PartyName,
                    PartyContact = c.PartyContact,
                    GoodDescription = c.GoodDescription,
                    TotalWeight = c.TotalWeight,
                    LoadingPoint = c.LoadingPoint,
                    UnloadingPoint = c.UnloadingPoint,

                    TripRequestDto = c.TripRequest.Select(tr => new TripRequestDto
                    {
                        Id = tr.Id,
                        DriverDto = tr.Driver != null ? new DriverDto
                        {
                            DriverId = tr.Driver.DriverId,
                            FirstName = tr.Driver.FirstName,
                            LastName = tr.Driver.LastName,
                            Contact = tr.Driver.Contact,
                            LicenseNumber = tr.Driver.LicenseNumber,
                            DateOfBirth = tr.Driver.DateOfBirth,
                            Status = tr.Driver.Status
                        } : null,
                        VehicleDto = tr.Vehicle !=null ? new VehicleDto
                        {
                            VehicleId = tr.Vehicle.VehicleId,
                            VehicleName = tr.Vehicle.VehicleName,
                            VehicleCapacity = tr.Vehicle.VehicleCapacity,
                            VehicleImagePath = tr.Vehicle.VehicleImagePath,
                            VehicleNumber = tr.Vehicle.VehicleNumber,
                            VehicleOwner = tr.Vehicle.VehicleOwner,
                            VehicleVolume = tr.Vehicle.VehicleVolume,
                            VehicleType = tr.Vehicle.VehicleType,
                            Status = tr.Vehicle.Status
                        }: null,
                        FreightDto = tr.Freight != null ? new FreightDto
                        {
                            FreightId = tr.Freight.FreightId,
                            AdvancedPaid = tr.Freight.AdvancedPaid,
                            RemainingAmount = tr.Freight.RemainingAmount,
                            FreightRate = tr.Freight.FreightRate,
                        }: null,
                    }).ToList(),

                }).FirstOrDefaultAsync(cancellationToken);
                
            if (consignmentDetails is null)
            {
                _logger.LogWarning("{FunctionName}, request consignment not found : {@RequestData}",
                    nameof(ConsignmentsByIdQueryHandler), request);
                return Results.NotFound(new
                {
                    Message = "Consignment Not Found."
                });
            }
            _logger.LogInformation("{FunctionName}, retrieve consignment successfully. Deetails : {@RequestData}",
                nameof(ConsignmentsByIdQueryHandler), request);

            return Results.Ok(new
            {
                response = consignmentDetails,
            });
        }
        catch (Exception ex)
        {
            _logger.LogInformation(ex, "{Functionname}, failed to retrieve consignment with Id: {@RequestData}",
                nameof(ConsignmentsByIdQueryHandler), request);
            return Results.BadRequest(new
            {
                Message = "Failed to retrieve consignment. Please try again.",
            });
        }

    }
}
