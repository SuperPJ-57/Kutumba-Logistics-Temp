
using Application.Dto.Consignments;
using Application.Dto.Drivers;
using Application.Dto.Freights;
using Application.Dto.TripRequests;
using Application.Dto.Trips;
using Application.Dto.Vehicles;
using Application.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Features.Trips.Queries.GetById;

public class GetTripByIdQueryHandler : IRequestHandler<GetTripByIdQuery, IResult>
{
    private readonly ITripRepository _tripRepository;
    private readonly ILogger<GetTripByIdQueryHandler> _logger;
    public GetTripByIdQueryHandler(ITripRepository tripRepository,ILogger<GetTripByIdQueryHandler> logger)
    {
        _tripRepository = tripRepository;

        _logger = logger;
    }

    public async Task<IResult> Handle(GetTripByIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("{FiunctionName} received a request to retrieve trip Id: {@Requestdata}",
            nameof(GetTripByIdQueryHandler), request);
        try
        {
            var trip = await _tripRepository.Queryable.AsNoTracking()
                .Where(t => t.TripId == request.TripId)
                .OrderBy(t => t.StartDate)
                .Select(t => new TripDto
                {
                    TripId = t.TripId,
                    StartDate = t.StartDate,
                    DeliveryDate = t.DeliveryDate,
                    TripAllowance = t.TripAllowance,
                    DriverAllownace = t.DriverAllownace,
                    MaintenanceFee = t.MaintenanceFee,
                    Status = t.Status,
                    TripRequestDto = t.TripRequest.Select(tr => new TripRequestDto
                    {
                        Id = tr.Id,
                        DriverDto = tr.Driver != null ? new DriverDto
                        {
                            DriverId = tr.Driver.DriverId,
                            FirstName = tr.Driver.FirstName,
                            LastName = tr.Driver.LastName,
                            DateOfBirth = tr.Driver.DateOfBirth,
                            ProfileImagePath = tr.Driver.ProfileImagePath,
                            Contact = tr.Driver.Contact,
                            LicenseNumber = tr.Driver.LicenseNumber,
                        } : null,
                        VehicleDto = tr.Vehicle != null ? new VehicleDto
                        {
                            VehicleId = tr.Vehicle.VehicleId,
                            VehicleCapacity = tr.Vehicle.VehicleCapacity,
                            VehicleImagePath = tr.Vehicle.VehicleImagePath,
                            VehicleName = tr.Vehicle.VehicleName,
                            VehicleNumber = tr.Vehicle.VehicleNumber,
                            VehicleOwner = tr.Vehicle.VehicleOwner,
                            VehicleType = tr.Vehicle.VehicleType,
                            VehicleVolume = tr.Vehicle.VehicleVolume,
                            Status = tr.Vehicle.Status,
                        } : null,
                        ConsignmentDto = tr.Consignment != null ? new ConsignmentDto
                        {
                            ConsignmentId = tr.Consignment.ConsignmentId,
                            PartyContact = tr.Consignment.PartyContact,
                            PartyName = tr.Consignment.PartyName,
                            GoodDescription = tr.Consignment.GoodDescription,
                            LoadingPoint = tr.Consignment.LoadingPoint,
                            TotalWeight = tr.Consignment.TotalWeight,
                            UnloadingPoint = tr.Consignment.UnloadingPoint,
                        } : null,
                        FreightDto = tr.Freight != null ? new FreightDto
                        {
                            FreightId = tr.Freight.FreightId,
                            FreightRate = tr.Freight.FreightRate,
                            AdvancedPaid = tr.Freight.AdvancedPaid,
                            RemainingAmount = tr.Freight.RemainingAmount,
                            PaymentMode = tr.Freight.PaymentMode,
                        }: null,
                        RefuelStation = tr.RefuelStation,
                        FuelReceiptPath = tr.FuelReceiptPath,
                        ApprovalStatus = tr.ApprovalStatus,

                    }).ToList(),
                }).FirstOrDefaultAsync(cancellationToken);
            if(trip is null)
            {
                _logger.LogWarning("{FunctionName} Failed. No trip found with Id : {@RequestId}",
                    nameof(GetTripByIdQueryHandler), request);
                return Results.NotFound(new
                {
                    Message = "Trip Not Found."
                });
            }

            _logger.LogInformation("{FunctionName}, Successfully retrieve all with Id :{@RequestId}",
                nameof(GetTripByIdQueryHandler), request);

            return Results.Ok(new
            {
                response = trip
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "{FunctionName} failed to retrieve trip with Id : {@RequestId}",
                nameof(GetTripByIdQueryHandler), request);

            return Results.BadRequest(new
            {
                Message = "Failed to fetch Trip. Please Try again."
            });
        };
    }
}

