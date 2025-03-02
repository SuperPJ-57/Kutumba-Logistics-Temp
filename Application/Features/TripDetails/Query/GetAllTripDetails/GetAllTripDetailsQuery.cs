using Application.Dto.TripDetails;
using Application.Interfaces.Repositories;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.TripDetails.Query.GetAllTripDetails
{
    public record GetAllTripDetailsQuery(int PageNumber, int PageSize) : IRequest<IResult>;

    public class GetAllTripDetailsQueryHandler(ITripDetailsRepository _tripDetailsRepository,
        ILogger<GetAllTripDetailsQueryHandler> _logger
        ) : IRequestHandler<GetAllTripDetailsQuery, IResult>
    {
        public async Task<IResult> Handle(GetAllTripDetailsQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("{FunctionName} received a request to retrieve all trip details.",
                nameof(GetAllTripDetailsQueryHandler));

            try
            {
                var totalCount = _tripDetailsRepository.Queryable.Count();

                var totalPages = (int)Math.Ceiling((double)totalCount / request.PageSize);

                var tripDetails = await _tripDetailsRepository
                    .GetAllAsNoTrackingAsync(request.PageNumber, request.PageSize);

                var tripDetailsDtos = tripDetails.Select(td => new TripDetailsDto
                {
                    Id = td.Id,
                    VehicleName = td.VehicleName,
                    Destination = $"{td.From} - {td.To}",
                    Latitude = td.Latitude,
                    Longitude = td.Longitude
                }).ToList();

                _logger.LogInformation("{FunctionName} successfully retrieved all trip details. Count: {@Count}. Result: {@TripDetailsDetails}",
                    nameof(GetAllTripDetailsQueryHandler), totalCount, JsonConvert.SerializeObject(tripDetailsDtos));

                return Results.Ok(new
                {
                    TripDetails = tripDetailsDtos,
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
                _logger.LogError(ex, "{FunctionName} error occurred while retrieving all trip details.",
                    nameof(GetAllTripDetailsQueryHandler));

                return Results.BadRequest(new
                {
                    Message = "An error occurred while retrieving trip details. Please try again later."
                });
            }
        }
    }
}
