using Application.Interfaces.Repositories;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http.Json;

namespace Application.Features.TrackVehicles.Command.TrackVehiclesAdd
{
    public record TrackVehiclesAddCommand(
       string VehicleName,
       double Latitude,
       double Longitude) : IRequest<IResult>;

    public class TrackVehiclesAddCommandHandler(ITrackVehiclesRepository _trackVehiclesRepository,
        ILogger<TrackVehiclesAddCommandHandler> _logger,
        IHttpClientFactory _httpClientFactory) : IRequestHandler<TrackVehiclesAddCommand, IResult>
    {
        public async Task<IResult> Handle(TrackVehiclesAddCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("{FunctionName} with request: {@RequestData}",
                nameof(TrackVehiclesAddCommandHandler), request);

            Domain.Entities.TrackVehicles trackVehicle = new()
            {
                VehicleName = request.VehicleName,
                Latitude = request.Latitude,
                Longitude = request.Longitude,
            };

            await _trackVehiclesRepository.AddAsync(trackVehicle);
            await _trackVehiclesRepository.SaveChangesAsync();

            if (trackVehicle.Id > 0)
            {
                _logger.LogInformation("{FunctionName} Successfully added vehicle location. Details : {@TrackVehicleDetails}",
                        nameof(TrackVehiclesAddCommandHandler), JsonConvert.SerializeObject(trackVehicle));

                // Publish message to RabbitMQ
                var client = _httpClientFactory.CreateClient();
                var response = await client.PostAsJsonAsync("http://localhost:5108/api/rabbitmq/publish", request);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("Failed to publish message to RabbitMQ. Status Code: {StatusCode}", response.StatusCode);
                    return Results.StatusCode((int)response.StatusCode);
                }

                return Results.Ok(new
                {
                    Message = "Vehicle location has been added and published to RabbitMQ.",
                    TrackVehicle = request
                });
            }
            else
            {
                _logger.LogWarning("{FunctionName} failed to add vehicle location with request: {@RequestData}",
                         nameof(TrackVehiclesAddCommandHandler), request);

                return Results.BadRequest(new
                {
                    Message = "Failed to add vehicle location. Please try again."
                });
            }
        }
    }
}
