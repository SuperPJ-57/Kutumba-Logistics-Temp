
using Application.Dto.Consignments;
using Application.Dto.Drivers;
using Application.Dto.TripRequests;
using Application.Dto.Trips;
using Application.Dto.Vehicles;
using Application.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;



namespace Application.Features.Vehicles.Queries.GetVehicleById;

public class GetVehicleByIdQueryHandler : IRequestHandler<GetVehicleByIdQuery, IResult>
{
    private readonly IVehicleRepository _vehiclesRepository;
    private ILogger<GetVehicleByIdQueryHandler> _logger;
    

    public GetVehicleByIdQueryHandler(IVehicleRepository vehiclesRepository, ILogger<GetVehicleByIdQueryHandler> logger)
    {
        _vehiclesRepository = vehiclesRepository;
        _logger = logger;
    }

    public async Task<IResult> Handle(GetVehicleByIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("{FunctionName}, received request with Vehicle Id : {RequestID}",
            nameof(GetVehicleByIdQueryHandler), request.VehicleId);

        try
        {
            var vehicleDetails = await _vehiclesRepository.Queryable.AsNoTracking()
                .Where(v => v.VehicleId == request.VehicleId)
                .OrderBy(v => v.VehicleNumber)
                .Select(v => new VehicleDto
                {
                    VehicleId = v.VehicleId,
                    VehicleName = v.VehicleName,
                    VehicleOwner = v.VehicleOwner,
                    VehicleType = v.VehicleType,
                    VehicleCapacity = v.VehicleCapacity,
                    VehicleVolume = v.VehicleVolume,
                    VehicleImagePath = v.VehicleImagePath,
                    VehicleNumber = v.VehicleNumber,
                    TripRequestDto = v.TripRequest.Select(tr => new TripRequestDto
                    {
                        Id = tr.Id,
                        DriverDto = tr.Driver != null ? new DriverDto
                        {
                            DriverId = tr.Driver.DriverId,
                            FirstName = tr.Driver.FirstName,
                            LastName = tr.Driver.LastName,
                            LicenseNumber = tr.Driver.LicenseNumber,
                            Contact = tr.Driver.Contact,
                            DateOfBirth = tr.Driver.DateOfBirth,
                            ProfileImagePath = tr.Driver.ProfileImagePath,
                            Status = tr.Driver.Status,
                            
                        } : null,
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
                        RefuelStation = tr.RefuelStation,
                        FuelReceiptPath = tr.RefuelStation,
                        ApprovalStatus = tr.ApprovalStatus
                    }).ToList(),
                }).FirstOrDefaultAsync(cancellationToken);

            if (vehicleDetails is null)
            {
                _logger.LogWarning("{FunctionName}, request vehicle not found : {@RequestID}",
                    nameof(GetVehicleByIdQueryHandler), request.VehicleId);

                return Results.NotFound(new
                {
                    Message = "Vehicle Not found."
                });
            }

            _logger.LogInformation("{FunctionName}, retrieve Vehicle successfully. Details : {@VehicleDetails}",
                nameof(GetVehicleByIdQueryHandler), JsonConvert.SerializeObject(vehicleDetails));
            return Results.Ok(new
            {
                response = vehicleDetails
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "{FunctionName}, error occured while retrieving vehicle. Id: {RequestId}",
                nameof(GetVehicleByIdQueryHandler), request);
            return Results.BadRequest(new
            { 
                message = "An error occured while retrieving vehicles. Please try again later."
            });
        }
    }
}
