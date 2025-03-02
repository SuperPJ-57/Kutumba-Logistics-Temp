using Application.Dto.TripLogging;
using Application.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Application.Features.TripLogging.Query.GetTripLoggingById
{
    public record GetTripLoggingByIdQuery(int Id) : IRequest<IResult>;

    public class GetTripLoggingByIdQueryHandler(
        ITripLoggingRepository _tripLoggingRepository,
        ILogger<GetTripLoggingByIdQueryHandler> _logger
        ) : IRequestHandler<GetTripLoggingByIdQuery, IResult>
    {
        public async Task<IResult> Handle(GetTripLoggingByIdQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("{FunctionName} received request for trip log ID: {@RequestId}",
                nameof(GetTripLoggingByIdQueryHandler), request.Id);

            try
            {
                var tripLog = await _tripLoggingRepository
                    .Queryable
                    .Where(x => x.Id == request.Id)
                    .Select(x => new TripLoggingDto(
                        x.Id,
                        x.LoadingPoint,
                        x.ReFilledFuelStation,
                        x.Weight,
                        x.Latitude,
                        x.Longitude,
                        x.ImagePath))
                    .FirstOrDefaultAsync(cancellationToken);

                if (tripLog == null)
                {
                    _logger.LogWarning("{FunctionName} - Trip log not found for ID: {Id}",
                        nameof(GetTripLoggingByIdQueryHandler), request.Id);

                    return Results.NotFound(new
                    {
                        Message = "Trip log data not found"
                    });
                }

                _logger.LogInformation("{FunctionName} successfully retrieved trip log: {@TripLogDetails}",
                    nameof(GetTripLoggingByIdQueryHandler), JsonConvert.SerializeObject(tripLog));

                return Results.Ok(new
                {
                    TripLog = tripLog
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{FunctionName} error for ID {Id}: {ErrorMessage}",
                    nameof(GetTripLoggingByIdQueryHandler), request.Id, ex.Message);

                return Results.BadRequest(new
                {
                    Message = "Error retrieving trip log data"
                });
            }
        }
    }
}
