using Application.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;


namespace Application.Features.Drivers.Command.DeleteDrivers;

public class DriversDeleteCommandHandler : IRequestHandler<DriversDeleteCommand, IResult>
{
    private readonly IDriversRepository _driversRepository;
    private readonly ILogger<DriversDeleteCommandHandler> _logger;
    public DriversDeleteCommandHandler(IDriversRepository driversRepository, ILogger<DriversDeleteCommandHandler> logger)
    {
        _driversRepository = driversRepository;
        _logger = logger;
    }

    public async Task<IResult> Handle(DriversDeleteCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("{FunctionName} with request : {@RquestData}",
            nameof(DriversDeleteCommandHandler), request);
        try
        {
            var driverToDelete = await _driversRepository.Queryable.FirstOrDefaultAsync(d => d.DriverId==request.DriverId,cancellationToken);
            if (driverToDelete is null)
            {
                _logger.LogWarning("{FunctionName} failed. Driver with Id: {@RequestId} not found.", 
                    nameof(DriversDeleteCommandHandler), request.DriverId);
                return Results.NotFound(new
                {
                    Message = "Driver not found."
                });
            }
            driverToDelete.IsActive = false;
            await _driversRepository.UpdateAsync(driverToDelete);
            await _driversRepository.SaveChangesAsync();

            _logger.LogInformation("{FunctionName} Succefully deleted driver with Id : {@DriverID}",
                nameof(DriversDeleteCommandHandler), request);

            return Results.Ok(new
            {
                Message = "Driver deleted successfully."
            });

        }
        catch(Exception ex)
        {
            _logger.LogInformation(ex ,"{FunctionName} Succefully deleted driver with Id : {@DriverID}", 
                nameof(DriversDeleteCommandHandler), request);

            return Results.BadRequest(new
            {
                Message = "Failed to delete driver. Please try again."
            });
        }
    }
}
 