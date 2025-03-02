using Application.Dto.TripLogging;
using Application.Interfaces.Repositories;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Application.Features.TripLogging.Query.GetAllTripLogging
{
    public record GetAllTripLoggingQuery(int PageNumber, int PageSize) : IRequest<IResult>;

    public class GetAllTripLoggingQueryHandler(ITripLoggingRepository _tripLoggingRepository,
        ILogger<GetAllTripLoggingQueryHandler> _logger
        ) : IRequestHandler<GetAllTripLoggingQuery, IResult>
    {
        public async Task<IResult> Handle(GetAllTripLoggingQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("{FunctionName} received a request to retrieve all trip logs.",
                nameof(GetAllTripLoggingQueryHandler));

            try
            {
                var totalCount = _tripLoggingRepository.Queryable.Count();
                var totalPages = (int)Math.Ceiling((double)totalCount / request.PageSize);

                var tripLogs = await _tripLoggingRepository.GetAllAsNoTrackingAsync(request.PageNumber, request.PageSize);

                var tripLogDtos = tripLogs.Select(tl => new TripLoggingDto(
                    tl.Id, 
                    tl.LoadingPoint,
                    tl.ReFilledFuelStation,
                    tl.Weight,
                    tl.Latitude,
                    tl.Longitude,
                    tl.ImagePath)).ToList();

                _logger.LogInformation("{FunctionName} successfully retrieved all trip logs. Count: {@Count}. Result: {@TripLogsDetails}",
                    nameof(GetAllTripLoggingQueryHandler), totalCount, JsonConvert.SerializeObject(tripLogDtos));

                return Results.Ok(new
                {
                    TripLogs = tripLogDtos,
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
                _logger.LogError(ex, "{FunctionName} error occurred while retrieving trip logs.",
                    nameof(GetAllTripLoggingQueryHandler));

                return Results.BadRequest(new
                {
                    Message = "An error occurred while retrieving trip logs. Please try again later."
                });
            }
        }
    }
}
