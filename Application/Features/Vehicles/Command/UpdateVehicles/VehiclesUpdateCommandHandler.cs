
using Application.Interfaces.Repositories;
using Domain.Logistic;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Application.Features.Vehicles.Command.UpdateVehicles;

public class VehiclesUpdateCommandHandler : IRequestHandler<VehiclesUpdateCommand, IResult>
{
    private readonly IVehicleRepository _vehicleRepository;
    private readonly ILogger<VehiclesUpdateCommandHandler> _logger;
    private readonly ITripRequestRepository _tripRequestRepository;

    public VehiclesUpdateCommandHandler(
        IVehicleRepository vehicleRepository,
        ILogger<VehiclesUpdateCommandHandler> logger,
        ITripRequestRepository tripRequestRepository)
    {
        _vehicleRepository = vehicleRepository;
        _logger = logger;
        _tripRequestRepository = tripRequestRepository;
    }
    public async Task<IResult> Handle(VehiclesUpdateCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("{FunctionName}, received request to update vehicle with Id. Id : {@Requestdata}",
            nameof(VehiclesUpdateCommandHandler), request);

        try
        {
            var existingVehicle = _vehicleRepository.FirstOrDefault(x => x.VehicleId == request.VehicleId);
            if (existingVehicle is null)
            {
                _logger.LogWarning("{FunctionName}, Failed to retrieve Id for Upadte. Id: {@RequestData}",
                    nameof(VehiclesUpdateCommandHandler), request);

                return Results.NotFound(new
                {
                    Message = "Vehicle not found."
                });
            }
            existingVehicle.VehicleOwner = request.VehicleOwner;
            existingVehicle.VehicleName = request.VehicleName;
            existingVehicle.VehicleNumber = request.VehicleNumber;
            existingVehicle.VehicleType = request.VehicleType;
            existingVehicle.VehicleCapacity = request.VehicleCapacity;
            existingVehicle.VehicleVolume = request.VehicleVolume;
            existingVehicle.Status = request.Status;
            existingVehicle.VehicleImagePath = request.VehicleImagePath;

            
            await _vehicleRepository.UpdateAsync(existingVehicle);
            await _vehicleRepository.SaveChangesAsync(cancellationToken);

            var tripRequest = new TripRequest
            {
                DriverId = request.DriverId,
            };
            await _tripRequestRepository.UpdateAsync(tripRequest);
            await _tripRequestRepository.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("{FunctionName}, vehicle update successfully with requested data. Details : {@RequestData}",
                nameof(VehiclesUpdateCommandHandler), JsonConvert.SerializeObject(request));

            return Results.Ok(new
            {
                Message = "Vehicle with Id update successfully.",
                response = existingVehicle
            });
        }
        catch(Exception ex)
        {
            _logger.LogInformation(ex, "{FunctionName}, Failed to Update vehicle with request data. data: {@RequestData}",
                nameof(VehiclesUpdateCommandHandler), request);
            return Results.BadRequest(new
            {
                Message = "Failed to update Vehicle. Please try again."
            });
        }

    }
}
