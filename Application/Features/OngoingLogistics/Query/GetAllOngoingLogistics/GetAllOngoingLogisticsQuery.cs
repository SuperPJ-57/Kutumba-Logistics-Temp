using Application.Dto.OngoingLogistics;
using Application.Interfaces.Repositories;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.OngoingLogistics.Query.GetAllOngoingLogistics
{
    public record GetAllOngoingLogisticsQuery(int PageNumber, int PageSize) : IRequest<IResult>;

    public class GetAllOngoingLogisticsQueryHandler(
        IOngoingLogisticsRepository _ongoingLogisticsRepository,
        ILogger<GetAllOngoingLogisticsQueryHandler> _logger,
        IHttpClientFactory _httpClientFactory) : IRequestHandler<GetAllOngoingLogisticsQuery, IResult>
    {
        public async Task<IResult> Handle(GetAllOngoingLogisticsQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("{FunctionName} received a request to retrieve all ongoing logistics.",
                nameof(GetAllOngoingLogisticsQueryHandler));

            try
            {
                var totalCount = _ongoingLogisticsRepository.Queryable.Count();

                var totalPages = (int)Math.Ceiling((double)totalCount / request.PageSize);

                var ongoingLogistics = await _ongoingLogisticsRepository
                    .GetAllAsNoTrackingAsync(request.PageNumber, request.PageSize);

                var ongoingLogisticsDtos = ongoingLogistics.Select(ol => new OngoingLogisticsDto
                {
                    VehicleId = ol.VehicleId,
                    DriverId = ol.DriverId
                }).ToList();

                _logger.LogInformation("{FunctionName} successfully retrieved all ongoing logistics. Count: {@Count}. Result: {@OngoingLogisticsDetails}",
                    nameof(GetAllOngoingLogisticsQueryHandler), totalCount, JsonConvert.SerializeObject(ongoingLogisticsDtos));

                // Publish message to RabbitMQ

                //var client = _httpClientFactory.CreateClient();
                //var response = await client.PostAsJsonAsync("http://103.140.0.164:5108/api/rabbitmq/publish-", ongoingLogisticsDtos);

                //if (!response.IsSuccessStatusCode)
                //{
                //    _logger.LogWarning("Failed to publish message to RabbitMQ. Status Code: {StatusCode}", response.StatusCode);
                //    return Results.StatusCode((int)response.StatusCode);
                //}

                return Results.Ok(new
                {
                    OngoingLogistics = ongoingLogisticsDtos,
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
                _logger.LogError(ex, "{FunctionName} error occurred while retrieving all ongoing logistics.",
                    nameof(GetAllOngoingLogisticsQueryHandler));

                return Results.BadRequest(new
                {
                    Message = "An error occurred while retrieving ongoing logistics. Please try again later."
                });
            }
        }
    }
}
