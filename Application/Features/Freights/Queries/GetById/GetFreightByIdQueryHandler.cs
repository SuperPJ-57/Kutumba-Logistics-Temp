
using Application.Dto.Drivers;
using Application.Dto.Freights;
using Application.Dto.TripRequests;
using Application.Dto.Vehicles;
using Application.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Features.Freights.Queries.GetById;

public class GetFreightByIdQueryHandler : IRequestHandler<GetFreightByIdQuery, IResult>
{
    private readonly IFreightRepository _freightRepository;
    private readonly ILogger<GetFreightByIdQueryHandler> _logger;

    public GetFreightByIdQueryHandler(IFreightRepository freightRepository, ILogger<GetFreightByIdQueryHandler> logger)
    {
        _freightRepository = freightRepository;
        _logger = logger;
    }

    public async Task<IResult> Handle(GetFreightByIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("{FunctionName}, received request with Fireght Id: {@RequestData}",
            nameof(GetFreightByIdQueryHandler), request);
        try
        {
            var freightDetails = await _freightRepository.Queryable.AsNoTracking()
                //.Where(f => f.FreightId == request.FreightId)
                .OrderBy(f => f.FreightId)
                .Select(f => new FreightDto
                {
                    FreightId = f.FreightId,
                    AdvancedPaid = f.AdvancedPaid,
                    RemainingAmount = f.RemainingAmount,
                    FreightRate = f.FreightRate,
                    PaymentMode = f.PaymentMode,
                    TripRequestDto = f.TripRequest.Select(tr => new TripRequestDto
                    {
                        Id = tr.Id,
                        DriverDto = tr.Driver !=null ? new DriverDto
                        {
                            DriverId = tr.Driver.DriverId,
                            FirstName = tr.Driver.FirstName,
                            LastName = tr.Driver.LastName,
                            Contact = tr.Driver.Contact,
                            LicenseNumber = tr.Driver.LicenseNumber,
                            ProfileImagePath = tr.Driver.ProfileImagePath,
                            Status = tr.Driver.Status,
                            DateOfBirth = tr.Driver.DateOfBirth,
                            TripRequestDto = tr.Driver.TripRequest.Select(tr => new TripRequestDto
                            {
                                VehicleDto = tr.Vehicle != null ? new VehicleDto
                                {
                                    VehicleId = tr.Vehicle.VehicleId,
                                    VehicleCapacity = tr.Vehicle.VehicleCapacity,
                                    VehicleImagePath = tr.Vehicle.VehicleImagePath,
                                    VehicleName = tr.Vehicle.VehicleName,
                                    VehicleNumber = tr.Vehicle.VehicleNumber,
                                    VehicleOwner = tr.Vehicle.VehicleOwner,
                                    VehicleType = tr.Vehicle.VehicleType,
                                    VehicleVolume = tr.Vehicle.VehicleVolume,
                                    Status = tr.Vehicle.Status,
                                }: null,
                            }).ToList(),
                        }: null,
                        
                    }).ToList(),
                }).FirstOrDefaultAsync(cancellationToken);
            _logger.LogInformation("{FunctionName}, successfully fetch freights Id. {@RequestData}",
                nameof(GetFreightByIdQueryHandler), request);
            if (freightDetails is null)
            {
                _logger.LogWarning("{FunctionName}, failed fetch freights Id. {@RequestData}",
                nameof(GetFreightByIdQueryHandler), request);
                return Results.NotFound(new
                {
                    Message = "Freight Details not found."
                });
            }
            return Results.Ok(new
            {
                Message = "Freight Details",
                Freight = freightDetails
            });
        }
        catch (Exception ex)
        {
            _logger.LogInformation(ex, "{FunctionName}, failed fetch freights Id. {@RequestData}",
                nameof(GetFreightByIdQueryHandler), request);
            return Results.BadRequest(new
            {
                Message = "Failed to Fetch Freight with Id. Please try again."
            });
        }
    }
}
