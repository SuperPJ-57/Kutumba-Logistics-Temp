using Application.Dto.TripRequest;
using Application.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Application.Features.TripRequest.Query.GetTripRequestById
{
    public record GetTripRequestByIdQuery(int Id) : IRequest<IResult>;

    public class GetTripRequestByIdQueryHandler(
        ITripRequestRepository _tripRequestRepository,
        ILogger<GetTripRequestByIdQueryHandler> _logger
        ) : IRequestHandler<GetTripRequestByIdQuery, IResult>
    {
        public async Task<IResult> Handle(GetTripRequestByIdQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("{FunctionName} received request for trip request ID: {@RequestId}",
                nameof(GetTripRequestByIdQueryHandler), request.Id);

            try
            {
                var tripRequest = await _tripRequestRepository
                    .Queryable
                    .Where(x => x.Id == request.Id)
                    .Select(x => new TripRequestDto
                    {
                        Id = x.Id,
                        VehicleName = x.VehicleName,
                        Destination = $"{x.From} - {x.To}",
                        Date = x.Date,
                        Time = x.Time

                    })
                    .FirstOrDefaultAsync(cancellationToken);

                if (tripRequest == null)
                {
                    _logger.LogWarning("{FunctionName} - Trip request not found for ID: {Id}",
                        nameof(GetTripRequestByIdQueryHandler), request.Id);

                    return Results.NotFound(new
                    {
                        Message = "Trip request data not found"
                    });
                }

                _logger.LogInformation("{FunctionName} successfully retrieved trip requestg: {@TripLogRequest}",
                    nameof(GetTripRequestByIdQueryHandler), JsonConvert.SerializeObject(tripRequest));

                return Results.Ok(new
                {
                    TripLog = tripRequest
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{FunctionName} error for ID {Id}: {ErrorMessage}",
                    nameof(GetTripRequestByIdQueryHandler), request.Id, ex.Message);

                return Results.BadRequest(new
                {
                    Message = "Error retrieving trip request data"
                });
            }
        }
    }
}
