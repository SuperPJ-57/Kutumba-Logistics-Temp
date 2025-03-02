using Application.Dto.OngoingLogistics;
using Application.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http.Json;

namespace Application.Features.OngoingLogistics.Query.GetOngoingLogisticsById
{
    public record GetOngoingLogisticsByIdQuery(int Id) : IRequest<IResult>;

    public class GetOngoingLogisticsByIdQueryHandler(
        IOngoingLogisticsRepository _ongoingLogisticsRepository,
        ILogger<GetOngoingLogisticsByIdQueryHandler> _logger,
        IHttpClientFactory _httpClientFactory) : IRequestHandler<GetOngoingLogisticsByIdQuery, IResult>
    {
        public async Task<IResult> Handle(GetOngoingLogisticsByIdQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("{FunctionName} received request for ongoing logistics ID: {@RequestId}",
                nameof(GetOngoingLogisticsByIdQueryHandler), request.Id);

            try
            {
                var ongoingLogistics = await _ongoingLogisticsRepository
                    .Queryable
                    .Where(x => x.Id == request.Id)
                    .Select(x => new OngoingLogisticsDto
                    {
                        VehicleId = x.VehicleId,
                        DriverId = x.DriverId
                    }).FirstOrDefaultAsync(cancellationToken);

                if (ongoingLogistics == null)
                {
                    _logger.LogWarning("{FunctionName} - Ongoing Logistics not found for ID: {Id}",
                        nameof(GetOngoingLogisticsByIdQueryHandler), request.Id);

                    return Results.NotFound(new
                    {
                        Message = "Ongoing Logistics data not found"
                    });
                }

                _logger.LogInformation("{FunctionName} successfully retrieved ongoing logistics: {@OngoingLogisticsDetails}",
                    nameof(GetOngoingLogisticsByIdQueryHandler), JsonConvert.SerializeObject(ongoingLogistics));

                // Publish message to RabbitMQ
                var client = _httpClientFactory.CreateClient();
                var response = await client.PostAsJsonAsync("http://103.140.0.164:5108/api/rabbitmq/publish", ongoingLogistics);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("Failed to publish message to RabbitMQ. Status Code: {StatusCode}", response.StatusCode);
                    return Results.StatusCode((int)response.StatusCode);
                }

                return Results.Ok(new
                {
                    OngoingLogistics = ongoingLogistics,
                    Message = "Ongoing Logistics data retrieved and published to RabbitMQ."
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{FunctionName} error for ID {Id}: {ErrorMessage}",
                    nameof(GetOngoingLogisticsByIdQueryHandler), request.Id, ex.Message);

                return Results.BadRequest(new
                {
                    Message = "Error retrieving ongoing logistics data"
                });
            }
        }
    }
}
