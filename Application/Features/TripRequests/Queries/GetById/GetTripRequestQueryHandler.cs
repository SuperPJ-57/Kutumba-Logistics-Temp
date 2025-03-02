
using Application.Dto.Consignments;
using Application.Dto.Drivers;
using Application.Dto.TripRequests;
using Application.Dto.Trips;
using Application.Dto.Vehicles;
using Application.Interfaces.Repositories;
using Domain.Logistic.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Features.TripRequests.Queries.GetById;

public class GetTripRequestQueryHandler : IRequestHandler<GetTripRequestByIdQuery, IResult>
{
    private readonly ITripRequestRepository _tripRequestRepository;
    private readonly ILogger<GetTripRequestQueryHandler> _logger;
    public GetTripRequestQueryHandler(
        ITripRequestRepository tripRequestRepository,
        ILogger<GetTripRequestQueryHandler> logger)
    {
        _tripRequestRepository = tripRequestRepository;
        _logger = logger;
    }

    public async Task<IResult> Handle(GetTripRequestByIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("{FunctionName}, received request to retrieve triprquest with Id : {@RequestData}",
            nameof(GetTripRequestQueryHandler), request.Id);
        try
        {
            var tripRequestDetails = await _tripRequestRepository.Queryable.AsNoTracking()
            .Where(tr => tr.Id == request.Id)
            .Select(tr => new TripRequestDto
            {
                DriverDto = new DriverDto
                {
                    DriverId = tr.Driver.DriverId,
                    DateOfBirth = tr.Driver.DateOfBirth,
                    FirstName = tr.Driver.FirstName,
                    LastName = tr.Driver.LastName,
                    Contact = tr.Driver.Contact,
                    LicenseNumber = tr.Driver.LicenseNumber
                },

                VehicleDto = new VehicleDto
                {
                    VehicleId = tr.Vehicle.VehicleId,
                    VehicleCapacity = tr.Vehicle.VehicleCapacity,
                    VehicleImagePath = tr.Vehicle.VehicleImagePath,
                    VehicleName = tr.Vehicle.VehicleName,
                    VehicleNumber = tr.Vehicle.VehicleNumber,
                    VehicleOwner = tr.Vehicle.VehicleOwner,
                    VehicleType = tr.Vehicle.VehicleType,
                    VehicleVolume = tr.Vehicle.VehicleVolume,
                    Status = tr.Vehicle.Status
                },
                ConsignmentDto = new ConsignmentDto
                {
                    ConsignmentId = tr.Consignment.ConsignmentId,
                    GoodDescription = tr.Consignment.GoodDescription,
                    LoadingPoint = tr.Consignment.LoadingPoint,
                    UnloadingPoint = tr.Consignment.UnloadingPoint,
                    PartyContact = tr.Consignment.PartyContact,
                    PartyName = tr.Consignment.PartyName,
                    TotalWeight = tr.Consignment.TotalWeight
                },
                TripDto = new TripDto
                {
                    TripId = tr.Trip.TripId,
                    TripAllowance = tr.Trip.TripAllowance,
                    DeliveryDate = tr.Trip.DeliveryDate,
                    DriverAllownace = tr.Trip.DriverAllownace,
                    MaintenanceFee = tr.Trip.MaintenanceFee,
                    StartDate = tr.Trip.StartDate,
                    Status = tr.Trip.Status,
                },
                FuelReceiptPath = tr.FuelReceiptPath,
                RefuelStation = tr.RefuelStation,
                ApprovalStatus = tr.ApprovalStatus,

            })
            .FirstOrDefaultAsync(cancellationToken);
            if (tripRequestDetails is null)
            {
                _logger.LogWarning("{FunctionName}, Failed to Retrieve Trip Request Id : {@RequestData}",
                    nameof(GetTripRequestQueryHandler), request);
                return Results.NotFound(new
                {
                    Message = "Trip Request Not Found."
                });
            }

            return Results.Ok(new
            {
                response = tripRequestDetails,
                Message = "Successfully retrieve TripRquest"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,"{FunctionName}, failed to retrieve Triip Request with Id : {RequestData}",
                nameof(GetTripRequestQueryHandler), request);
            return Results.BadRequest(new
            {
                Message = "Failed to Retrieve Trip Request. Please Try Again."
            });
        }
    }
}
