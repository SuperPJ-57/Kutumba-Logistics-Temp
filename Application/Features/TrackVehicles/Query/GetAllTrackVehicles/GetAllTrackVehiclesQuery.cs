using Application.Dto.TrackVehicles;
using Application.Interfaces.Repositories;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Application.Features.TrackVehicles.Query.GetAllTrackVehicles
{
    public record GetAllTrackVehiclesQuery(int PageNumber, int PageSize) : IRequest<IResult>;

    public class GetAllTrackVehiclesQueryHandler(ITrackVehiclesRepository _trackVehiclesRepository,
        ILogger<GetAllTrackVehiclesQueryHandler> _logger
        ) : IRequestHandler<GetAllTrackVehiclesQuery, IResult>
    {
        public async Task<IResult> Handle(GetAllTrackVehiclesQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("{FunctionName} received a request to retrieve all tracked vehicles.",
                nameof(GetAllTrackVehiclesQueryHandler));

            try
            {
                var totalCount = _trackVehiclesRepository.Queryable.Count();

                var totalPages = (int)Math.Ceiling((double)totalCount / request.PageSize);

                var trackedVehicles = await _trackVehiclesRepository
                    .GetAllAsNoTrackingAsync(request.PageNumber, request.PageSize);

                var trackedVehicleDtos = trackedVehicles.Select(tv => new TrackVehiclesDto
                {
                    Id = tv.Id,
                    VehicleName = tv.VehicleName,
                    Latitude = tv.Latitude,
                    Longitude = tv.Longitude

                }).ToList();

                _logger.LogInformation("{FunctionName} successfully retrieved all tracked vehicles. Count: {@Count}. Result: {@TrackedVehiclesDetails}",
                    nameof(GetAllTrackVehiclesQueryHandler), totalCount, JsonConvert.SerializeObject(trackedVehicleDtos));

                return Results.Ok(new
                {
                    TrackedVehicles = trackedVehicleDtos,
                    Meta = new
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
                _logger.LogError(ex, "{FunctionName} error occurred while retrieving all tracked vehicles.",
                    nameof(GetAllTrackVehiclesQueryHandler));

                return Results.BadRequest(new
                {
                    Message = "An error occurred while retrieving tracked vehicles. Please try again later."
                });
            }
        }
    }
}
