using Application.Dto.TrackVehicles;
using Application.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Application.Features.TrackVehicles.Query.GetTrackVehiclesById
{
    public record GetTrackVehiclesByIdQuery(int Id) : IRequest<IResult>;

    public class GetTrackVehiclesByIdQueryHandler(
        ITrackVehiclesRepository _trackVehiclesRepository,
        ILogger<GetTrackVehiclesByIdQueryHandler> _logger
        ) : IRequestHandler<GetTrackVehiclesByIdQuery, IResult>
    {
        public async Task<IResult> Handle(GetTrackVehiclesByIdQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("{FunctionName} received request for vehicle ID: {@RequestId}",
                nameof(GetTrackVehiclesByIdQueryHandler), request.Id);

            try
            {
                var vehicle = await _trackVehiclesRepository
                    .Queryable
                    .Where(x => x.Id == request.Id)
                    .Select(x => new TrackVehiclesDto
                    {
                        Id = x.Id,
                        VehicleName = x.VehicleName,
                        Latitude = x.Latitude,
                        Longitude = x.Longitude
                    })
                    .FirstOrDefaultAsync(cancellationToken);

                if (vehicle == null)
                {
                    _logger.LogWarning("{FunctionName} - Vehicle not found for ID: {Id}",
                        nameof(GetTrackVehiclesByIdQueryHandler), request.Id);

                    return Results.NotFound(new
                    {
                        Message = "Vehicle tracking data not found"
                    });
                }

                _logger.LogInformation("{FunctionName} successfully retrieved vehicle: {@VehicleDetails}",
                    nameof(GetTrackVehiclesByIdQueryHandler), JsonConvert.SerializeObject(vehicle));

                return Results.Ok(new
                {
                    Vehicle = vehicle
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{FunctionName} error for ID {Id}: {ErrorMessage}",
                    nameof(GetTrackVehiclesByIdQueryHandler), request.Id, ex.Message);

                return Results.BadRequest(new
                {
                    Message = "Error retrieving vehicle tracking data"
                });
            }
        }
    }
}
