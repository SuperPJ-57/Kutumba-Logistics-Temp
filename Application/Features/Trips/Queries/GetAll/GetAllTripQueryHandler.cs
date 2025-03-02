
using Application.Dto.Consignments;
using Application.Dto.Drivers;
using Application.Dto.TripRequests;
using Application.Dto.Trips;
using Application.Dto.Vehicles;
using Application.Features.Accounts.Query.GetAll;
using Application.Interfaces.Repositories;
using AutoMapper;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;


namespace Application.Features.Trips.Queries.GetAll;

public class GetAllTripQueryHandler : IRequestHandler<GetAllTripQuery, IResult>
{
    private readonly ITripRepository _tripRepository;
    private readonly ILogger<GetAllTripQueryHandler> _logger;

    public GetAllTripQueryHandler(
        ITripRepository tripRepository,
        ILogger<GetAllTripQueryHandler> logger)
    {
        _tripRepository = tripRepository;
        _logger = logger;
    }

    public async Task<IResult> Handle(GetAllTripQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("{FunctionName} received a request to retrieve all Trips.",
            nameof(GetAllTripQueryHandler));
        try
        {
            var totalCount = _tripRepository.Queryable.Count();
            var totalPages = (int)Math.Ceiling((double)totalCount / request.PageSize);

            var trips = await _tripRepository.Queryable.AsNoTracking()
                .Include(t => t.TripRequest)
                    .ThenInclude(tr => tr.Driver)
                .Include(t => t.TripRequest)
                    .ThenInclude(tr => tr.Vehicle)
                .Include(t => t.TripRequest)
                    .ThenInclude(tr => tr.Consignment)
                .OrderBy(t => t.StartDate)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync(cancellationToken);

            var tripDto = trips.Select(t => new TripDto
            {
                TripId = t.TripId,
                StartDate = t.StartDate,
                DeliveryDate = t.DeliveryDate,
                TripAllowance = t.TripAllowance,
                MaintenanceFee = t.MaintenanceFee,
                DriverAllownace = t.DriverAllownace,
                Status = t.Status,
                TripRequestDto = t.TripRequest.Select(tr => new TripRequestDto
                {
                    Id = tr.Id,
                    DriverDto = tr.Driver !=null ? new DriverDto
                    {
                        DriverId = tr.Driver.DriverId,
                        FirstName = tr.Driver.FirstName,
                        LastName = tr.Driver.LastName,
                        Contact = tr.Driver.Contact,
                        LicenseNumber = tr.Driver.LicenseNumber,
                        DateOfBirth = tr.Driver.DateOfBirth,
                        ProfileImagePath = tr.Driver.ProfileImagePath,
                        Status = tr.Driver.Status,
                    }:null,
                    VehicleDto = tr.Vehicle !=null ? new VehicleDto
                    {
                        VehicleId = tr.Vehicle.VehicleId,
                        VehicleName = tr.Vehicle.VehicleName,
                        VehicleCapacity = tr.Vehicle.VehicleCapacity,
                        VehicleImagePath = tr.Vehicle.VehicleImagePath,
                        VehicleNumber = tr.Vehicle.VehicleNumber,
                        VehicleOwner = tr.Vehicle.VehicleOwner,
                        VehicleType = tr.Vehicle.VehicleType,
                        VehicleVolume = tr.Vehicle.VehicleVolume,
                        Status = tr.Vehicle.Status,
                    }: null,
                    ConsignmentDto = tr.Consignment !=null ? new ConsignmentDto
                    {
                        ConsignmentId = tr.Consignment.ConsignmentId,
                        PartyContact = tr.Consignment.PartyContact,
                        PartyName = tr.Consignment.PartyName,
                        LoadingPoint = tr.Consignment.LoadingPoint,
                        UnloadingPoint = tr.Consignment.UnloadingPoint,
                        GoodDescription = tr.Consignment.GoodDescription,
                        TotalWeight = tr.Consignment.TotalWeight,
                    }: null,
                    RefuelStation = tr.RefuelStation,
                    FuelReceiptPath = tr.FuelReceiptPath,
                    ApprovalStatus = tr.ApprovalStatus
                }).ToList()
            }).ToList();

            _logger.LogInformation("{FunctionName} successfully retrieved all TripDetails. Count: {@Count}",
              nameof(GetAllTripQueryHandler), totalCount);

            return Results.Ok(new
            {
                Trips = tripDto,
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
            _logger.LogError(ex, "{FunctionName} error occurred while retrieving  all the Trips.",
                nameof(GetAllDriversQueryHandler));
            return Results.BadRequest(new
            {
                Message = "An error occurred while retrieving trips. Please try again later."
            });
        }
    }
}