using Application.Interfaces.Repositories;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.OngoingLogistics.Command.OngoingLogisticsUpdate
{
    public record OngoingLogisticsUpdateCommand(
       int Id,
       int VehicleId,
       int DriverId) : IRequest<IResult>;

    public class OngoingLogisticsUpdateCommandHandler : IRequestHandler<OngoingLogisticsUpdateCommand, IResult>
    {
        private readonly IOngoingLogisticsRepository _ongoingLogisticsRepository;
        private readonly ILogger<OngoingLogisticsUpdateCommandHandler> _logger;
        private readonly IHttpClientFactory _httpClientFactory;

        public OngoingLogisticsUpdateCommandHandler(
            IOngoingLogisticsRepository ongoingLogisticsRepository,
            ILogger<OngoingLogisticsUpdateCommandHandler> logger,
            IHttpClientFactory httpClientFactory)
        {
            _ongoingLogisticsRepository = ongoingLogisticsRepository;
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IResult> Handle(OngoingLogisticsUpdateCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("{FunctionName} with request: {@RequestData}",
                nameof(OngoingLogisticsUpdateCommandHandler), request);

            var ongoingLogistics = await _ongoingLogisticsRepository.GetByIdAsync(request.Id);
            if (ongoingLogistics == null)
            {
                _logger.LogWarning("{FunctionName} - Ongoing Logistics not found for Id: {@OngoingLogisticsId}",
                    nameof(OngoingLogisticsUpdateCommandHandler), request.Id);
                return Results.NotFound(new { Message = "Ongoing Logistics not found." });
            }

            ongoingLogistics.VehicleId = request.VehicleId;
            ongoingLogistics.DriverId = request.DriverId;

            await _ongoingLogisticsRepository.SaveChangesAsync();

            _logger.LogInformation("{FunctionName} Successfully updated ongoing logistics. Details: {@OngoingLogisticsDetails}",
                        nameof(OngoingLogisticsUpdateCommandHandler), JsonConvert.SerializeObject(ongoingLogistics));

            var client = _httpClientFactory.CreateClient();
            var response = await client.PostAsJsonAsync("http://103.140.0.164:5108/api/rabbitmq/publish-update", request);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("Failed to publish update message to RabbitMQ. Status Code: {StatusCode}", response.StatusCode);
                return Results.StatusCode((int)response.StatusCode);
            }

            return Results.Ok(new
            {
                Message = "Ongoing logistics has been updated and published to RabbitMQ.",
                OngoingLogistics = request
            });
        }
    }
}
