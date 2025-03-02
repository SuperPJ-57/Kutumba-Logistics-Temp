using Application.Dto.Consignments;
using Application.Dto.Drivers;
using Application.Dto.TripRequests;
using Application.Dto.Trips;
using Application.Dto.Vehicles;
using Application.Interfaces.Repositories;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Features.Drivers.Queries.GetById;

public class GetDriversByIdQueryHandler : IRequestHandler<GetDriversByIdQuery, IResult>
{
    private readonly IDriversRepository _driversRepository;
    private readonly ILogger<GetDriversByIdQueryHandler> _logger;
    private readonly IMapper _mapper;

    public GetDriversByIdQueryHandler(IDriversRepository driversRepository, ILogger<GetDriversByIdQueryHandler> logger, IMapper mapper)
    {
        _driversRepository = driversRepository;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<IResult> Handle(GetDriversByIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("{FiunctionName} received a request to retrieve driver Id: {@Requestdata}",
            nameof(GetDriversByIdQueryHandler), request);
        try
        {
            var driver = await _driversRepository.Queryable.AsNoTracking()
                        .Where(d => d.DriverId == request.DriverId)
                        .OrderBy(d => d.LicenseNumber)
                        .Select(d => new DriverDto
                        {
                            DriverId = d.DriverId,
                            FirstName = d.FirstName,
                            LastName = d.LastName,
                            Contact = d.Contact,
                            LicenseNumber = d.LicenseNumber,
                            ProfileImagePath = d.ProfileImagePath,
                            DateOfBirth = d.DateOfBirth,
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
                                }: null,
                                ConsignmentDto = tr.Consignment != null? new ConsignmentDto
                                {
                                    ConsignmentId = tr.Consignment.ConsignmentId,
                                    PartyContact = tr.Consignment.PartyContact,
                                    PartyName = tr.Consignment.PartyName,
                                    LoadingPoint = tr.Consignment.LoadingPoint,
                                    TotalWeight = tr.Consignment.TotalWeight,
                                    UnloadingPoint = tr.Consignment.UnloadingPoint,
                                    GoodDescription = tr.Consignment.GoodDescription,
                                }: null,
                                RefuelStation = tr.RefuelStation,
                                ApprovalStatus = tr.ApprovalStatus,
                                FuelReceiptPath = tr.FuelReceiptPath,

                            }).ToList(),
                        }).FirstOrDefaultAsync(cancellationToken);
            if(driver is null)
            {
                _logger.LogWarning("{FunctionName} Failed. No driver found with Id : {@RequestId}",
                    nameof(GetDriversByIdQueryHandler), request);

                return Results.NotFound(new
                {
                    Message = "Driver Not Found."
                });
            }
            _logger.LogInformation("{FunctionName}, Successfully retrieve all with Id :{@RequestId}",
                nameof(GetDriversByIdQueryHandler), request);

            //var driverDto = _mapper.Map<DriverDto>(driver);
            return Results.Ok(new
            {
                response = driver
            });
        }
        catch(Exception ex)
        {
            _logger.LogInformation(ex, "{FunctionName} failed to retrieve driver with Id : {@RequestId}",
                nameof(GetDriversByIdQueryHandler), request);
            return Results.BadRequest(new
            {
                Message = "Failed to retrieve Driver with Id. Please Try again later."
            });
        };
    }
}
