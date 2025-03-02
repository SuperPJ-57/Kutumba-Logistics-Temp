using Application.Dto.Consignments;
using Application.Dto.Drivers;
using Application.Dto.TripRequests;
using Application.Dto.Trips;
using Application.Dto.Vehicles;
using Application.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Application.Features.TripRequests.Queries.GetAll;

public class GetAllTripRequestQueryHandler : IRequestHandler<GetAllTripRequestQuery, IResult>
{
    private readonly ITripRequestRepository _tripRequestRepository;
    private readonly ILogger<GetAllTripRequestQueryHandler> _logger;

    public GetAllTripRequestQueryHandler(ITripRequestRepository tripRequestRepository, ILogger<GetAllTripRequestQueryHandler> logger)
    {
        _tripRequestRepository = tripRequestRepository;
        _logger = logger;
    }

    public async Task<IResult> Handle(GetAllTripRequestQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("{FunctionName} received request to get all data", nameof(GetAllTripRequestQueryHandler));

        try
        {
            var totalCount = await _tripRequestRepository.Queryable.CountAsync(cancellationToken);
            var totalPages = (int)Math.Ceiling((double)totalCount / request.PageSize);

            var tripRequests = await _tripRequestRepository.Queryable.AsNoTracking()
                                    .Include(tr => tr.Trip)
                                    .Include(tr => tr.Consignment)
                                    .Include(tr => tr.Vehicle)
                                    .Include(tr => tr.Driver)
                                    .ToListAsync(cancellationToken);
            //.GetAllAsNoTrackingAsync(request.PageNumber, request.PageSize);

            var tripRequestDtos = tripRequests.Select(tr => new TripRequestDto
            {



                Id = tr.Id,

                TripDto = new TripDto
                {
                    TripId = tr.Trip.TripId,
                    DeliveryDate = tr.Trip.DeliveryDate,
                    TripAllowance = tr.Trip.TripAllowance,
                    MaintenanceFee = tr.Trip.MaintenanceFee,
                    DriverAllownace = tr.Trip.DriverAllownace,
                    StartDate = tr.Trip.StartDate,
                    Status = tr.Trip.Status,
                },

                ConsignmentDto = new ConsignmentDto
                {
                    ConsignmentId = tr.Consignment.ConsignmentId,
                    PartyName = tr.Consignment.PartyName,
                    PartyContact = tr.Consignment.PartyContact,
                    LoadingPoint = tr.Consignment.LoadingPoint,
                    UnloadingPoint = tr.Consignment.UnloadingPoint,
                    GoodDescription = tr.Consignment.GoodDescription,
                    TotalWeight = tr.Consignment.TotalWeight,
                },

                VehicleDto = new VehicleDto
                {
                    VehicleId = tr.Vehicle.VehicleId,
                    VehicleImagePath = tr.Vehicle.VehicleImagePath,
                    VehicleOwner = tr.Vehicle.VehicleOwner,
                    VehicleName = tr.Vehicle.VehicleName,
                    VehicleType = tr.Vehicle.VehicleType,
                    VehicleVolume = tr.Vehicle.VehicleVolume,
                    VehicleCapacity = tr.Vehicle.VehicleCapacity,
                    VehicleNumber = tr.Vehicle.VehicleNumber,
                    Status = tr.Vehicle.Status
                },

                DriverDto = new DriverDto
                {
                    DriverId = tr.Driver.DriverId,
                    FirstName = tr.Driver.FirstName,
                    LastName = tr.Driver.LastName,
                    DateOfBirth = tr.Driver.DateOfBirth,
                    LicenseNumber = tr.Driver.LicenseNumber,
                    Contact = tr.Driver.Contact,
                    Status = tr.Driver.Status,
                },

                RefuelStation = tr.RefuelStation,
                FuelReceiptPath = tr.FuelReceiptPath,
                ApprovalStatus = tr.ApprovalStatus,
            }).ToList();

            _logger.LogInformation("{FunctionName} successfully retrieved trip requests. Count: {Count}", 
                nameof(GetAllTripRequestQueryHandler), JsonConvert.SerializeObject(totalCount));

            return Results.Ok(new
            {
                response = tripRequestDtos,
                meta = new
                {
                    PageNumber = request.PageNumber,
                    PageSize = request.PageSize,
                    TotalCount = totalCount,
                    TotalPages = totalPages
                }
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "{FunctionName} error occurred while retrieving trip requests", 
                nameof(GetAllTripRequestQueryHandler));
            return Results.BadRequest(new { Message = "An error occurred while retrieving trip requests. Please try again later." });
        }
    }
}
