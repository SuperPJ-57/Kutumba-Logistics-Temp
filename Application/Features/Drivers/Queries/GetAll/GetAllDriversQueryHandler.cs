using Application.Dto.Consignments;
using Application.Dto.Drivers;
using Application.Dto.TripRequests;
using Application.Dto.Trips;
using Application.Dto.Vehicles;
using Application.Features.Drivers.Queries.GetAll;
using Application.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;



namespace Application.Features.Accounts.Query.GetAll;

public class GetAllDriversQueryHandler : IRequestHandler<GetAllDriversQuery, IResult>
{
    private readonly ILogger<GetAllDriversQueryHandler> _logger;
    private readonly IDriversRepository _driversRepository;
    private readonly ITripRequestRepository _ripRequestRepository;
    public GetAllDriversQueryHandler(
        ILogger<GetAllDriversQueryHandler> logger,
        IDriversRepository driversRepositroy,
        ITripRequestRepository ripRequestRepository)
    {
        _logger = logger;
        _driversRepository = driversRepositroy;
        _ripRequestRepository = ripRequestRepository;
    }


    public async Task<IResult> Handle(GetAllDriversQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("{FunctionName} received a request to retrieve all drivers.",
            nameof(GetAllDriversQueryHandler));

        try
        {

            var totalCount = _driversRepository.Queryable.Count();

            var totalPages = (int)Math.Ceiling((double)totalCount / request.PageSize);


            var drivers = await _driversRepository.Queryable.AsNoTracking()
                .Include(d => d.TripRequest)
                    .ThenInclude(tr => tr.Trip)
                .Include(d => d.TripRequest)
                    .ThenInclude(tr => tr.Vehicle)
                .Include(d => d.TripRequest)
                    .ThenInclude(tr => tr.Consignment)
                .OrderBy(d => d.LicenseNumber)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync(cancellationToken);

            var driverDto = drivers.Select(d => new DriverDto
            {
                DriverId = d.DriverId,
                FirstName = d.FirstName,
                LastName = d.LastName,
                LicenseNumber = d.LicenseNumber,
                Contact = d.Contact,
                ProfileImagePath = d.ProfileImagePath,
                DateOfBirth = d.DateOfBirth,
                Status = d.Status,
                TripRequestDto = d.TripRequest.Select(tr => new TripRequestDto
                {
                    Id = tr.Id,
                    TripDto = tr.Trip != null ? new TripDto
                    {
                        TripId = tr.Trip.TripId,
                        DeliveryDate = tr.Trip.DeliveryDate,
                        StartDate = tr.Trip.StartDate,
                        DriverAllownace = tr.Trip.DriverAllownace,
                        MaintenanceFee = tr.Trip.MaintenanceFee,
                        TripAllowance = tr.Trip.TripAllowance,
                        Status = tr.Trip.Status
                    } : null,
                    VehicleDto = tr.Vehicle != null ? new VehicleDto
                    {
                        VehicleId = tr.Vehicle.VehicleId,
                        VehicleCapacity = tr.Vehicle.VehicleCapacity,
                        VehicleImagePath = tr.Vehicle.VehicleImagePath,
                        VehicleName = tr.Vehicle.VehicleName,
                        VehicleOwner = tr.Vehicle.VehicleOwner,
                        VehicleType = tr.Vehicle.VehicleType,
                        VehicleVolume = tr.Vehicle.VehicleVolume,
                        VehicleNumber = tr.Vehicle.VehicleNumber,
                        Status = tr.Vehicle.Status
                    } : null,
                    ConsignmentDto = tr.Consignment != null ? new ConsignmentDto
                    {
                        ConsignmentId = tr.Consignment.ConsignmentId,
                        PartyContact = tr.Consignment.PartyContact,
                        PartyName = tr.Consignment.PartyName,
                        LoadingPoint = tr.Consignment.LoadingPoint,
                        TotalWeight = tr.Consignment.TotalWeight,
                        UnloadingPoint = tr.Consignment.UnloadingPoint,
                        GoodDescription = tr.Consignment.GoodDescription,
                    } : null,
                    FuelReceiptPath = tr.FuelReceiptPath,
                    RefuelStation = tr.RefuelStation,
                    ApprovalStatus = tr.ApprovalStatus
                }).ToList(),
            }).ToList();

            _logger.LogInformation("{FunctionName} successfully retrieved all driverDetails. Count: {@Count}",
              nameof(GetAllDriversQueryHandler), totalCount);

            return Results.Ok(new
            {
                Driver = driverDto,
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
            _logger.LogError(ex, "{FunctionName} error occurred while retrieving  all the drivers.",
                nameof(GetAllDriversQueryHandler));

            return Results.BadRequest(new
            {
                Message = "An error occurred while retrieving driverDetails. Please try again later."
            });
        }
    }
}