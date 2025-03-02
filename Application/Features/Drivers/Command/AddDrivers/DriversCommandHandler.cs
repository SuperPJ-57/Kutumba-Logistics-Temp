
using Application.Interfaces.Repositories;
using Domain.Logistic;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Application.Features.Drivers.Command.AddDrivers;

public class DriversCommandHandler : IRequestHandler<DriversCommand, IResult>
{
    private readonly IDriversRepository _driversRepository;
    private readonly ILogger<DriversCommandHandler> _logger;
    private readonly ITripRequestRepository _tripRequestRepository;
    public DriversCommandHandler(IDriversRepository driversRepository, ILogger<DriversCommandHandler> logger, ITripRequestRepository tripRequestRepository)
    {
        _driversRepository = driversRepository;
        _logger = logger;
        _tripRequestRepository = tripRequestRepository;
    }
    public async Task<IResult> Handle(DriversCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("{FunctionName} with request: {@RequestData}",
                nameof(DriversCommandHandler), request);

        try
        {
            var driverCreate = new Driver
            {
                DriverId = request.DriverId,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Contact = request.Contact,
                LicenseNumber = request.LicenseNumber,
                Status = request.Status,
                DateOfBirth = request.DateOfBirth,
                //ProfileImagePath = request.ProfileImage,
            };
            await _driversRepository.AddAsync(driverCreate);
            await _driversRepository.SaveChangesAsync(cancellationToken);

            var tripRequest = new TripRequest
            {
                DriverId = driverCreate.DriverId,
            };
            await _tripRequestRepository.AddAsync(tripRequest);
            await _tripRequestRepository.SaveChangesAsync();
            

            _logger.LogInformation("{FunctionName} Successfully added driver. Details : {@DriverDetails}",
                            nameof(DriversCommandHandler), JsonConvert.SerializeObject(request));
            return Results.Ok(new
            {
                //response = driverCreate,
                Message = "Drivers created successfully."
            });
        }
        catch(Exception ex)
        {
            _logger.LogWarning(ex,"{FunctionName} failed to add driver with request: {@RequestData}",
                         nameof(DriversCommandHandler), request);

            return Results.BadRequest(new
            {
                Message = "Failed to add driver. Please try again."
            });

        }

    }
}
