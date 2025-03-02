using Application.Interfaces.Repositories;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http.Json;

namespace Application.Features.TrackVehicles.Command.TrackVehiclesUpdate
{
    public record TrackVehiclesUpdateCommand(
       int Id,
       string VehicleName,
       double Latitude,
       double Longitude) : IRequest<IResult>;

    public class TrackVehiclesUpdateCommandHandler : IRequestHandler<TrackVehiclesUpdateCommand, IResult>
    {
        private readonly ITrackVehiclesRepository _trackVehiclesRepository;
        private readonly ILogger<TrackVehiclesUpdateCommandHandler> _logger;
        private readonly IHttpClientFactory _httpClientFactory;

        public TrackVehiclesUpdateCommandHandler(
            ITrackVehiclesRepository trackVehiclesRepository,
            ILogger<TrackVehiclesUpdateCommandHandler> logger,
            IHttpClientFactory httpClientFactory)
        {
            _trackVehiclesRepository = trackVehiclesRepository;
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IResult> Handle(TrackVehiclesUpdateCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("{FunctionName} with request: {@RequestData}",
                nameof(TrackVehiclesUpdateCommandHandler), request);

            var trackVehicle = await _trackVehiclesRepository.GetByIdAsync(request.Id);
            if (trackVehicle == null)
            {
                _logger.LogWarning("{FunctionName} - Vehicle not found for Id: {@TrackVehiclesId}",
                    nameof(TrackVehiclesUpdateCommandHandler), request.Id);
                return Results.NotFound(new { Message = "Vehicle not found." });
            }

            trackVehicle.VehicleName = request.VehicleName;
            trackVehicle.Latitude = request.Latitude;
            trackVehicle.Longitude = request.Longitude;

            await _trackVehiclesRepository.SaveChangesAsync();

            _logger.LogInformation("{FunctionName} Successfully updated vehicle location. Details: {@TrackVehicleDetails}",
                        nameof(TrackVehiclesUpdateCommandHandler), JsonConvert.SerializeObject(trackVehicle));

            var client = _httpClientFactory.CreateClient();
            var response = await client.PostAsJsonAsync("http://localhost:5108/api/rabbitmq/publish-update", request);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("Failed to publish update message to RabbitMQ. Status Code: {StatusCode}", response.StatusCode);
                return Results.StatusCode((int)response.StatusCode);
            }

            return Results.Ok(new
            {
                Message = "Vehicle location has been updated and published to RabbitMQ.",
                TrackVehicle = request
            });
        }
    }
}
