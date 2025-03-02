using Application.Interfaces.Repositories;
using Microsoft.Extensions.Logging;

namespace Application.Features.Vehicles.Command.DeleteVehicles;

public class VehicleDeleteCommandHandler : IRequestHandler<DeleteVehicleCommand, IResult>
{
    private readonly IVehicleRepository _vehicleRepository;
    private readonly ILogger<VehicleDeleteCommandHandler> _logger;
    public VehicleDeleteCommandHandler(IVehicleRepository vehicleRepository, ILogger<VehicleDeleteCommandHandler> logger)
    {
        _vehicleRepository = vehicleRepository;
        _logger = logger;
    }

    public async Task<IResult> Handle(DeleteVehicleCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("{FunctionName}, received request to delete vehicle. Id : {@RequestData}",
            nameof(VehicleDeleteCommandHandler), request);

        try
        {
            var existingVehicle = await _vehicleRepository.FirstOrDefaultAsync(x => x.VehicleId == request.VehicleId);
            if (existingVehicle is null)
            {
                _logger.LogInformation("{FunctionName}, request vehicle Id not found. Id : {@RequestData}",
                    nameof(VehicleDeleteCommandHandler), request);
                return Results.NotFound(new
                {
                    Message = "Vehicle not found."
                });
            }

            _vehicleRepository.Remove(existingVehicle);

            existingVehicle.IsActive = false;
            await _vehicleRepository.UpdateAsync(existingVehicle);

            await _vehicleRepository.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("{FunctionName}, vehicle is deleted successfully with request Id. {@RequestData}",
                nameof(VehicleDeleteCommandHandler), request);

            return Results.Ok(new
            {
                Message = "Vehicle details has been deleted successfylly.",

            });
        }
        catch (Exception ex)
        {
            _logger.LogInformation(ex, "{FunctionName}, failed to delete request vehicle Id. Id :{RequestData}",
                nameof(VehicleDeleteCommandHandler), request);
            return Results.BadRequest(new
            {
                Message = "Failed to delete request Id. Please try again."
            });
        }
    }
}
