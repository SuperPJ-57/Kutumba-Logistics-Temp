using Application.Dto.TripDetails;
using Application.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Application.Features.TripDetails.Query.GetTripDetailsById
{
    public record GetTripDetailsByIdQuery(int Id) : IRequest<IResult>;

    public class GetTripDetailsByIdQueryHandler(
        ITripDetailsRepository _tripDetailsRepository,
        ILogger<GetTripDetailsByIdQueryHandler> _logger
        ) : IRequestHandler<GetTripDetailsByIdQuery, IResult>
    {
        public async Task<IResult> Handle(GetTripDetailsByIdQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("{FunctionName} received request for trip detail ID: {@RequestId}",
                nameof(GetTripDetailsByIdQueryHandler), request.Id);

            try
            {
                var tripDetail = await _tripDetailsRepository
                    .Queryable
                    .Where(x => x.Id == request.Id)
                    .Select(x => new TripDetailsDto { 
                        Id = x.Id,
                        VehicleName = x.VehicleName,
                        Destination = $"{x.From} - {x.To}",
                        Latitude = x.Latitude,
                        Longitude = x.Longitude
                        
                    })
                    .FirstOrDefaultAsync(cancellationToken);

                if (tripDetail == null)
                {
                    _logger.LogWarning("{FunctionName} - Trip detail not found for ID: {Id}",
                        nameof(GetTripDetailsByIdQueryHandler), request.Id);

                    return Results.NotFound(new
                    {
                        Message = "Trip detail data not found"
                    });
                }

                _logger.LogInformation("{FunctionName} successfully retrieved trip detailg: {@TripLogDetails}",
                    nameof(GetTripDetailsByIdQueryHandler), JsonConvert.SerializeObject(tripDetail));

                return Results.Ok(new
                {
                    TripLog = tripDetail
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{FunctionName} error for ID {Id}: {ErrorMessage}",
                    nameof(GetTripDetailsByIdQueryHandler), request.Id, ex.Message);

                return Results.BadRequest(new
                {
                    Message = "Error retrieving trip detail data"
                });
            }
        }
    }
}
