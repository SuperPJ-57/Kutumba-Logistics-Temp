using Application.Interfaces.Repositories;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.OngoingLogistics.Command.OngoingLogisticsAdd
{
    public record OngoingLogisticsAddCommand(
        int VehicleId,
        int DriverId) : IRequest<IResult>;
    public class OngoingLogisticsAddCommandHandler : IRequestHandler<OngoingLogisticsAddCommand, IResult>
    {
        private readonly IOngoingLogisticsRepository _ongoingLogisticsRepository;
        private readonly ILogger<OngoingLogisticsAddCommandHandler> _logger;
        private readonly IHttpClientFactory _httpClientFactory;

        public OngoingLogisticsAddCommandHandler(
            IOngoingLogisticsRepository ongoingLogisticsRepository,
            ILogger<OngoingLogisticsAddCommandHandler> logger,
            IHttpClientFactory httpClientFactory)
        {
            _ongoingLogisticsRepository = ongoingLogisticsRepository;
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IResult> Handle(OngoingLogisticsAddCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("{FunctionName} with request: {@RequestData}",
                nameof(OngoingLogisticsAddCommandHandler), request);

            Domain.Entities.OngoingLogistics ongoingLogistics = new()
            {
                VehicleId = request.VehicleId,
                DriverId = request.DriverId
            };
            

            await _ongoingLogisticsRepository.AddAsync(ongoingLogistics);
            await _ongoingLogisticsRepository.SaveChangesAsync();

            if (ongoingLogistics.Id > 0)
            {
                _logger.LogInformation("{FunctionName} Successfully added ongoing logistics. Details : {@TrackVehicleDetails}",
                        nameof(OngoingLogisticsAddCommandHandler), JsonConvert.SerializeObject(ongoingLogistics));

                // Publish message to RabbitMQ
                var client = _httpClientFactory.CreateClient();
                var response = await client.PostAsJsonAsync("http://103.140.0.164:5108/api/rabbitmq/publish", request);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("Failed to publish message to RabbitMQ. Status Code: {StatusCode}", response.StatusCode);
                    return Results.StatusCode((int)response.StatusCode);
                }

                return Results.Ok(new
                {
                    Message = "Ongoing Logistics has been added and published to RabbitMQ.",
                    TrackVehicle = request
                });
            }
            else
            {
                _logger.LogWarning("{FunctionName} failed to add ongoing logistics with request: {@RequestData}",
                         nameof(OngoingLogisticsAddCommandHandler), request);

                return Results.BadRequest(new
                {
                    Message = "Failed to add ongoing logistics. Please try again."
                });
            }
        }
    }
}
