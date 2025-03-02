
using Application.Dto.Drivers;
using Application.Dto.TripRequests;
using Application.Dto.Vehicles;
using Application.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;


namespace Application.Features.Vehicles.Queries.GetAllVehicle;

public class GetAllVehiclesQueryHandler : IRequestHandler<GetAllVehiclesQuery, IResult>
{
    private readonly IVehicleRepository _vehiclesRepository;
    private readonly ILogger<GetAllVehiclesQueryHandler> _logger;

    public GetAllVehiclesQueryHandler(IVehicleRepository vehicleRepository, ILogger<GetAllVehiclesQueryHandler>logger)
    {
        _vehiclesRepository = vehicleRepository;
        _logger = logger;
    }

    public async Task<IResult> Handle(GetAllVehiclesQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("{FunctionName}, received a request to retrieve all vehicles.",
                nameof(GetAllVehiclesQueryHandler));
        try
        {
            var totalCount = _vehiclesRepository.Queryable.Count();

            var totalPages = (int)Math.Ceiling((double)totalCount / request.PageSize);

            var vehicles = await _vehiclesRepository.Queryable
                .Include(v => v.TripRequest)
                    .ThenInclude(tr => tr.Driver)
                .OrderBy(v => v.VehicleNumber)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync(cancellationToken);
            var vehicleDto = vehicles.Select(v => new VehicleDto
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
                    DriverDto = tr.Driver != null? new DriverDto
                    {
                        DriverId = tr.Driver.DriverId,
                        FirstName = tr.Driver.FirstName,
                        LastName = tr.Driver.LastName,
                        Contact = tr.Driver.Contact,
                        LicenseNumber = tr.Driver.LicenseNumber,
                        ProfileImagePath = tr.Driver.ProfileImagePath,
                        DateOfBirth = tr.Driver.DateOfBirth,
                    }:null,
                }).ToList()
            }).ToList();


            _logger.LogInformation("{FunctionName} successfully retrieved all vehicles. Count: {@Count}",
            nameof(GetAllVehiclesQueryHandler), totalCount);

            return Results.Ok(new
            {
                response = vehicleDto,
                meta = new
                {
                    PageNumber = request.PageNumber,
                    PageSize = request.PageSize,
                    TotalCount = totalCount,
                    TotalPages = totalPages
                }
            });
        }

        catch (Exception ex)
        {
            _logger.LogError(ex, "{FunctionName} error occurred while retrieving  all the vehicles.",
                    nameof(GetAllVehiclesQueryHandler));
            return Results.BadRequest(new
            {
                Message = "An error occurred while retrieving vehicles. Please try again later."
            });
        }
    }
}
