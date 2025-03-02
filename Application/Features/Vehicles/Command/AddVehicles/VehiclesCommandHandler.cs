

using Application.Interfaces.Repositories;
using Domain.Logistic;
using Microsoft.Extensions.Logging;

namespace Application.Features.Vehicles.Command.AddVehicles;

public class VehiclesCommandHandler : IRequestHandler<VehiclesCommand, IResult>
{
    private readonly IVehicleRepository _vehicleRepository;
    private readonly ILogger<VehiclesCommandHandler> _logger;
    private readonly ITripRequestRepository _tripRequestRepository;
    public VehiclesCommandHandler(
        IVehicleRepository vehicleRepository,
        ILogger<VehiclesCommandHandler> logger,
        IDriversRepository driversRepository, ITripRequestRepository tripRequestRepositroy)
    {
        _vehicleRepository = vehicleRepository;
        _logger = logger;
        _tripRequestRepository = tripRequestRepositroy;
    }

    public async Task<IResult> Handle(VehiclesCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("{FunctionName}, recevied request : {@RequestData}",
            nameof(VehiclesCommandHandler), request);
        await Task.CompletedTask;
        try
        {
            var vehicle = new Vehicle
            {

                VehicleOwner = request.VehicleOwner,
                VehicleNumber = request.VehicleNumber,
                VehicleName = request.VehicleName,
                VehicleType = request.VehicleNumber,
                VehicleCapacity = request.VehicleCapacity,
                VehicleVolume = request.VehicleVolume,
                VehicleImagePath = request.VehicleImagePath,
                Status = request.Status
            };
            await _vehicleRepository.AddAsync(vehicle);
            await _vehicleRepository.SaveChangesAsync();

            var tripRequest = new TripRequest
            {
                Id = request.Id,
                VehicleId = vehicle.VehicleId,
                DriverId = request.DriverId

            };
            await _tripRequestRepository.UpdateAsync(tripRequest);
            await _tripRequestRepository.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("{FunctionName}, Request Vehicle is created Successfully, :{@RequestData}",
                nameof(VehiclesCommandHandler), request);
            return Results.Ok(new
            {
                response = vehicle,
            });
        }
        catch (Exception ex)
        {
            _logger.LogInformation(ex,"{FunctionName}, failed to create vehicle with request : {@RequestData}",
                nameof(VehiclesCommandHandler), request);
            return Results.BadRequest(new
            {
                Message = "Failed to create vehicle. Please try again later."
            });
        }
    }
}
