


using Application.Interfaces.Repositories;
using Domain.Logistic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;


namespace Application.Features.TripRequests.Command.UpdateTripRequest;

public class TripRequestUpdateCommandHandler : IRequestHandler<TripRequestUpdateCommand, IResult>
{
    private readonly ITripRequestRepository _tripRequestRepository;
    private readonly ILogger<TripRequestUpdateCommandHandler> _logger;
    private readonly IDriversRepository _driversRepository;
    private readonly ITripRepository _tripRepository;
    private readonly IConsignmentRepository _consignmentRepository;
    private readonly IVehicleRepository _vehicleRepository;
    public TripRequestUpdateCommandHandler(
        ITripRequestRepository tripRequestRepository,
        IDriversRepository driversRepository,
        ITripRepository tripRepository,
        IConsignmentRepository consignmentRepository,
        IVehicleRepository vehicleRepository,
        ILogger<TripRequestUpdateCommandHandler> logger)
    {
        _tripRequestRepository = tripRequestRepository;
        _driversRepository = driversRepository;
        _tripRepository = tripRepository;
        _consignmentRepository = consignmentRepository;
        _vehicleRepository = vehicleRepository;
        _logger = logger;
    }

    public async Task<IResult> Handle(TripRequestUpdateCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("{FunctionName}, received request to TripRequest with Data : {@RequestData}",
            nameof(TripRequestUpdateCommandHandler), request);
        try
        {
            var existingTripRequest = await _tripRequestRepository
                .Queryable.FirstOrDefaultAsync(tr => tr.Id == request.Id);
            if (existingTripRequest is null)
            {
                _logger.LogWarning("{FunctionName}, Failed to Retrieve TripRequest with Id : {@RequestData}",
                    nameof(TripRequestUpdateCommandHandler), request);
                return Results.NotFound(new
                {
                    Message = "Failed to retrieve TripRequest."
                });
            }
            var driver = new Driver
            {
                FirstName = request.Driver.FirstName,
                LastName = request.Driver.LastName,
                LicenseNumber = request.Driver.LicenseNumber,
                Contact = request.Driver.Contact,
            };

            var vehicle = new Vehicle
            {
                VehicleNumber = request.Vehicle.VehicleNumber,
                VehicleCapacity = request.Vehicle.VehicleCapacity,
            };

            var consignment = new Consignment
            {
                PartyName = request.Consignment.PartyName,
                PartyContact = request.Consignment.PartyContact,
                LoadingPoint = request.Consignment.LoadingPoint,
                UnloadingPoint = request.Consignment.UnloadingPoint,
            };
            var trip = new Trip
            {
                TripId = request.Trip.TripId,
            };
            await _driversRepository.UpdateAsync(driver);
            await _driversRepository.SaveChangesAsync(cancellationToken);

            await _vehicleRepository.UpdateAsync(vehicle);
            await _vehicleRepository.SaveChangesAsync(cancellationToken);

            await _consignmentRepository.UpdateAsync(consignment);
            await _consignmentRepository.SaveChangesAsync(cancellationToken);

            await _tripRepository.UpdateAsync(trip);
            await _tripRepository.SaveChangesAsync(cancellationToken);

            existingTripRequest.ApprovalStatus = request.ApprovalStatus;
            existingTripRequest.RefuelStation = request.RefuelStation;
            existingTripRequest.FuelReceiptPath = request.FuelReceiptPath;
            existingTripRequest.Trip = request.Trip;
            existingTripRequest.Driver = request.Driver;
            existingTripRequest.Consignment = request.Consignment;
            existingTripRequest.Vehicle = request.Vehicle;

            await _tripRequestRepository.UpdateAsync(existingTripRequest);
            await _tripRequestRepository.SaveChangesAsync();
            _logger.LogInformation("{FunctionName}, Successfully Update TripRequest with Data : {@RequestData}",
                nameof(TripRequestUpdateCommandHandler), request);

            return Results.Ok(new
            {
                response = existingTripRequest,
                Message = "Successfully Update TripRequest."
            });

        }
        catch (Exception ex)
        { 
            _logger.LogError(ex, "{FunctionName}, Failed to Update TripRequest with Data : {@TripRequestDetails}",
                nameof(TripRequestUpdateCommandHandler), request);
            return Results.BadRequest(new
            {
                Message = "Failed to Update TripRequest. Please Try Again."
            });
        }
    }
}

