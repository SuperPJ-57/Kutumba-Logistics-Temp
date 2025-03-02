using Application.Interfaces.Repositories;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http.Json;

namespace Application.Features.TripDetails.Command.TripDetailsAdd
{
    public record TripDetailsAddCommand(
       string VehicleName,
       string From,
       string To,
       double Latitude,
       double Longitude) : IRequest<IResult>;

    public class TripDetailsAddCommandHandler(ITripDetailsRepository _tripDetailsRepository,
        ILogger<TripDetailsAddCommandHandler> _logger,
        IHttpClientFactory _httpClientFactory) : IRequestHandler<TripDetailsAddCommand, IResult>
    {
        public async Task<IResult> Handle(TripDetailsAddCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("{FunctionName} with request: {@RequestData}",
                nameof(TripDetailsAddCommandHandler), request);

            Domain.Entities.TripDetails tripDetails = new()
            {
                VehicleName = request.VehicleName,
                From = request.From,
                To = request.To,
                Latitude = request.Latitude,
                Longitude = request.Longitude,
            };

            await _tripDetailsRepository.AddAsync(tripDetails);
            await _tripDetailsRepository.SaveChangesAsync();

            if (tripDetails.Id > 0)
            {
                _logger.LogInformation("{FunctionName} Successfully added Trip Details. Details : {@TripDetails}",
                        nameof(TripDetailsAddCommandHandler), JsonConvert.SerializeObject(tripDetails));

                /*
                // Publish message to RabbitMQ
                var client = _httpClientFactory.CreateClient();
                var response = await client.PostAsJsonAsync("http://localhost:5108/api/rabbitmq/publish", tripDetails);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("Failed to publish message to RabbitMQ. Status Code: {StatusCode}", response.StatusCode);
                    return Results.StatusCode((int)response.StatusCode);
                }
                */

                return Results.Ok(new
                {
                    Message = "Trip Details has been added successfully.",
                    TripDetails = request
                });
            }
            else
            {
                _logger.LogWarning("{FunctionName} failed to add Trip Details with request: {@RequestData}",
                         nameof(TripDetailsAddCommandHandler), request);

                return Results.BadRequest(new
                {
                    Message = "Failed to add Trip Details. Please try again."
                });
            }
        }
    }
}
