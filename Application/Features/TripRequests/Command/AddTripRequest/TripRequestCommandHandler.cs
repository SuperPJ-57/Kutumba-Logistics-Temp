using Application.Interfaces.Repositories;
using Domain.Logistic;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Application.Features.TripRequests.Command.AddTripRequest;
public class TripRequestCommandHandler : IRequestHandler<TripRequestCommand, IResult>
{
    private readonly ITripRequestRepository _tripRequestRepository;
    private readonly ITripRepository _tripRepository;
    private readonly IDriversRepository _driversRepository;
    private readonly IVehicleRepository _vehicleRepository;
    private readonly IConsignmentRepository _consignmentRepository;
    private readonly ILogger<TripRequestCommandHandler> _logger;

    public TripRequestCommandHandler(
        ITripRequestRepository tripRequestRepository, ITripRepository tripRepository,
        IDriversRepository driversRepository, IVehicleRepository vehicleRepository,
        IConsignmentRepository consignmentRepository, ILogger<TripRequestCommandHandler> logger)
    {
        _tripRequestRepository = tripRequestRepository;
        _tripRepository = tripRepository;
        _driversRepository = driversRepository;
        _vehicleRepository = vehicleRepository;
        _consignmentRepository = consignmentRepository;
        _logger = logger;
    }
    public async Task<IResult> Handle(TripRequestCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("{FunctionName}, received request to created trip with data : {@RequestData}",
            nameof(TripRequestCommandHandler), request);

        try
        {
            var driver = new Driver
            {

                FirstName = request.TripRequest.Driver.FirstName,
                LastName = request.TripRequest.Driver.LastName,
                LicenseNumber = request.TripRequest.Driver.LicenseNumber,
                Contact = request.TripRequest.Driver.Contact,
                DateOfBirth = request.TripRequest.Driver.DateOfBirth,

            };

            var vehicle = new Vehicle
            {
                VehicleOwner = request.TripRequest.Vehicle.VehicleOwner,
                VehicleNumber = request.TripRequest.Vehicle.VehicleNumber,
                VehicleCapacity = request.TripRequest.Vehicle.VehicleCapacity,
                VehicleType = request.TripRequest.Vehicle.VehicleType,
                VehicleImagePath = request.TripRequest.Vehicle.VehicleImagePath,
                VehicleName = request.TripRequest.Vehicle.VehicleName,
                VehicleVolume = request.TripRequest.Vehicle.VehicleVolume
            };

            var consignment = new Consignment
            {
                PartyName = request.TripRequest.Consignment.PartyName,
                PartyContact = request.TripRequest.Consignment.PartyContact,
                LoadingPoint = request.TripRequest.Consignment.LoadingPoint,
                UnloadingPoint = request.TripRequest.Consignment.UnloadingPoint,
                GoodDescription = request.TripRequest.Consignment.GoodDescription,
                TotalWeight = request.TripRequest.Consignment.TotalWeight
            };

            var trip = new Trip
            {
                TripId = request.TripRequest.Trip.TripId
            };
            await _driversRepository.AddAsync(driver);
            await _driversRepository.SaveChangesAsync(cancellationToken);

            await _vehicleRepository.AddAsync(vehicle);
            await _vehicleRepository.SaveChangesAsync(cancellationToken);

            await _consignmentRepository.AddAsync(consignment);
            await _consignmentRepository.SaveChangesAsync(cancellationToken);

            await _tripRepository.AddAsync(trip);
            await _tripRepository.SaveChangesAsync(cancellationToken);

            var tripRequest = new TripRequest
                    {
                        Driver = driver,
                        Vehicle = vehicle,
                        Consignment = consignment,
                        Trip = trip,
                        RefuelStation = request.TripRequest.RefuelStation,
                        FuelReceiptPath = request.TripRequest.FuelReceiptPath,
                        ApprovalStatus = request.TripRequest.ApprovalStatus
                    };
                await _tripRequestRepository.AddAsync(tripRequest);
                await _tripRequestRepository.SaveChangesAsync(cancellationToken);

                _logger.LogInformation("{FunctionName}, triprequest is created successfully with data : {@RequestData}",
                nameof(TripRequestCommandHandler), JsonConvert.SerializeObject(request));

            return Results.Ok(new
            {
                Response = request,
                Message = "Trip request send."
            });
        }
        catch (Exception ex)
        {
            _logger.LogInformation(ex, "{FunctionNAme}, Failed to created triprequest with data : {@RequestData}",
                nameof(TripRequestCommandHandler), request);
            return Results.BadRequest(new
            {
                Message = "Failed to create tripRequest. Please try again."
            });
        }

    }
}
