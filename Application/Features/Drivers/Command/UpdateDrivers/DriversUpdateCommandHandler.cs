
using Application.Interfaces.Repositories;
using Domain.Logistic;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;


namespace Application.Features.Drivers.Command.UpdateDrivers;

public class DriversUpdateCommandHandler : IRequestHandler<DriversUpdateCommand, IResult>
{
    private readonly IDriversRepository _driversRepository;
    private readonly ITripRequestRepository _tripRequestRepository;
    private readonly ILogger<DriversUpdateCommandHandler> _logger;
    public DriversUpdateCommandHandler(
        IDriversRepository driversRepository,
        ILogger<DriversUpdateCommandHandler> logger,
        ITripRequestRepository tripRequestRepository)
    {
        _driversRepository = driversRepository;
        _logger = logger;
        _tripRequestRepository = tripRequestRepository;
    }
    public async Task<IResult> Handle(DriversUpdateCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("{FunctionName} with request : {@Requestdata}",
            nameof(DriversUpdateCommandHandler), request);

        try
        {
            var existingDriver = await _driversRepository.GetByIdAsync(request.DriverId);
            if (existingDriver is null)
            {
                _logger.LogWarning("{FunctionNmae} is failed. Driver with Id : {@DriverId} not found.",
                    nameof(DriversUpdateCommandHandler), request);
                return Results.NotFound(new
                {
                    Message = "Driver not Found."
                });
            }


            //DriverID = request.driverId,
            existingDriver.FirstName = request.FirstName;
            existingDriver.LastName = request.LastName;
            existingDriver.Contact = request.Contact;
            existingDriver.LicenseNumber = request.LicenseNumber;

            _driversRepository.Update(existingDriver);
            await _driversRepository.SaveChangesAsync();

            var tripRequest = new TripRequest
            {
                DriverId = existingDriver.DriverId,
            };
            await _tripRequestRepository.UpdateAsync(tripRequest);
            await _tripRequestRepository.SaveChangesAsync();

            _logger.LogInformation("{FunctionName} is successfully Updated driver. Details : {@DriverDetails}",
                nameof(DriversUpdateCommandHandler), JsonConvert.SerializeObject(request));
            return Results.Ok(new
            {
                Message = "Driver Update Successfully.",
                response = existingDriver
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "{FunctionName} failed to update driver with the request : {@RequestData}",
                nameof(DriversUpdateCommandHandler), request);
            return Results.BadRequest(new
            {
                Message = "Failed to update driver."
            });
        }
    }
}
