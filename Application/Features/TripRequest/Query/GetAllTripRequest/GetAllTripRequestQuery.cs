using Application.Dto.TripRequest;
using Application.Interfaces.Repositories;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.TripRequest.Query.GetAllTripRequest
{
    public record GetAllTripRequestQuery(int PageNumber, int PageSize) : IRequest<IResult>;

    public class GetAllTripRequestQueryHandler(ITripRequestRepository _tripRequestRepository,
        ILogger<GetAllTripRequestQueryHandler> _logger
        ) : IRequestHandler<GetAllTripRequestQuery, IResult>
    {
        public async Task<IResult> Handle(GetAllTripRequestQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("{FunctionName} received a request to retrieve all trip request.",
                nameof(GetAllTripRequestQueryHandler));

            try
            {
                var totalCount = _tripRequestRepository.Queryable.Count();

                var totalPages = (int)Math.Ceiling((double)totalCount / request.PageSize);

                var tripRequest = await _tripRequestRepository
                    .GetAllAsNoTrackingAsync(request.PageNumber, request.PageSize);

                var tripRequestDtos = tripRequest.Select(tr => new TripRequestDto
                {
                    Id = tr.Id,
                    VehicleName = tr.VehicleName,
                    Destination = $"{tr.From} - {tr.To}",
                    Date = tr.Date,
                    Time = tr.Time
                }).ToList();

                _logger.LogInformation("{FunctionName} successfully retrieved all trip request. Count: {@Count}. Result: {@TripRRequest}",
                    nameof(GetAllTripRequestQueryHandler), totalCount, JsonConvert.SerializeObject(tripRequestDtos));

                return Results.Ok(new
                {
                    TripRequest = tripRequestDtos,
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
                _logger.LogError(ex, "{FunctionName} error occurred while retrieving all trip request.",
                    nameof(GetAllTripRequestQueryHandler));

                return Results.BadRequest(new
                {
                    Message = "An error occurred while retrieving trip request. Please try again later."
                });
            }
        }
    }
}
