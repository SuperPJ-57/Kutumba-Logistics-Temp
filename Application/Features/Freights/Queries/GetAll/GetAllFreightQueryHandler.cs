
using Application.Dto.Consignments;
using Application.Dto.Drivers;
using Application.Dto.Freights;
using Application.Dto.TripRequests;
using Application.Dto.Trips;
using Application.Dto.Vehicles;
using Application.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;


namespace Application.Features.Freights.Queries.GetAll;

public class GetAllFreightQueryHandler : IRequestHandler<GetAllFreightQuery, IResult>
{
    private readonly IFreightRepository _freightRepository;
    private readonly ILogger<GetAllFreightQueryHandler> _logger;
    public GetAllFreightQueryHandler(IFreightRepository freightRepository, ILogger<GetAllFreightQueryHandler>logger)
    {
        _freightRepository = freightRepository;
        _logger = logger;
    }
    public async Task<IResult> Handle(GetAllFreightQuery request, CancellationToken cancellationToken)
    { 
        _logger.LogInformation("{FunctionName}, request received for freight list",
        nameof(GetAllFreightQueryHandler));
        try
        {
            var totalCount = _freightRepository.Queryable.Count();
            var totalPages = (int)Math.Ceiling((double)totalCount / request.PageSize);

            var freights = await _freightRepository.Queryable
                .Include(f => f.TripRequest)
                    .ThenInclude(tr => tr.Consignment)
                .Include(f => f.TripRequest)
                    .ThenInclude(tr => tr.Vehicle)
                .Include(f => f.TripRequest)
                    .ThenInclude(tr => tr.Driver)
                .Include(f => f.TripRequest)
                    .ThenInclude(tr => tr.Trip)
                .OrderBy(f => f.FreightId)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync(cancellationToken);

            var freightDto = freights.Select(f => new FreightDto
            {
                FreightId = f.FreightId,
                AdvancedPaid = f.AdvancedPaid,
                RemainingAmount = f.RemainingAmount,
                FreightRate = f.FreightRate,
                PaymentMode = f.PaymentMode,
                TripRequestDto = f.TripRequest.Select(tr => new TripRequestDto
                {
                    Id = tr.Id,
                    ConsignmentDto = tr.Consignment != null ? new ConsignmentDto
                    {
                        ConsignmentId = tr.Consignment.ConsignmentId,
                        PartyContact = tr.Consignment.PartyContact,
                        PartyName = tr.Consignment.PartyName,
                        GoodDescription = tr.Consignment.GoodDescription,
                        LoadingPoint = tr.Consignment.LoadingPoint,
                        UnloadingPoint = tr.Consignment.UnloadingPoint,
                        TotalWeight = tr.Consignment.TotalWeight,
                    } : null,
                    VehicleDto = tr.Vehicle != null ? new VehicleDto
                    {
                        VehicleId = tr.Vehicle.VehicleId,
                        VehicleCapacity = tr.Vehicle.VehicleCapacity,
                        VehicleImagePath = tr.Vehicle.VehicleImagePath,
                        VehicleName = tr.Vehicle.VehicleName,
                        VehicleNumber = tr.Vehicle.VehicleNumber,
                        VehicleType = tr.Vehicle.VehicleType,
                        VehicleOwner = tr.Vehicle.VehicleOwner,
                        VehicleVolume = tr.Vehicle.VehicleVolume,
                        Status = tr.Vehicle.Status,
                    } : null,
                    DriverDto = tr.Driver !=null ? new DriverDto
                    {
                        DriverId = tr.Driver.DriverId,
                        FirstName = tr.Driver.FirstName,
                        LastName = tr.Driver.LastName,
                        Contact = tr.Driver.Contact,
                        LicenseNumber = tr.Driver.LicenseNumber,
                        ProfileImagePath = tr.Driver.ProfileImagePath,
                        DateOfBirth = tr.Driver.DateOfBirth,
                        Status = tr.Driver.Status,
                    } : null,
                    TripDto = tr.Trip != null ? new TripDto
                    {
                        TripId = tr.Trip.TripId,
                        TripAllowance = tr.Trip.TripAllowance,
                        DeliveryDate = tr.Trip.DeliveryDate,
                        DriverAllownace = tr.Trip.DriverAllownace,
                        MaintenanceFee = tr.Trip.MaintenanceFee,
                        StartDate = tr.Trip.StartDate,
                        Status = tr.Trip.Status,
                    } : null,

                    FuelReceiptPath = tr.FuelReceiptPath,
                    RefuelStation = tr.RefuelStation,
                    ApprovalStatus = tr.ApprovalStatus
                }).ToList(),
            }).ToList();

            _logger.LogInformation("{FunctionName} successfully retrieve all freight. Count : {@Count}",
                nameof(GetAllFreightQueryHandler), totalCount);

            return Results.Ok(new
            {
                Freight = freightDto,
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
            _logger.LogError(ex,"{FunctionName}, failed to retrieve all freight",
                nameof(GetAllFreightQueryHandler));
            return Results.BadRequest(new
            {
                Message = "An error occurred while retrieving freight. Please try again later."
            });         
        }
    }
}
