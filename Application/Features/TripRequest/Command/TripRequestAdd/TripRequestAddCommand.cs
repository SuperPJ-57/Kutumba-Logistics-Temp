using Application.Interfaces.Repositories;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Application.Features.TripRequest.Command.TripRequestAdd
{
    public record TripRequestAddCommand(
       string VehicleName,
       string From,
       string To,
       DateTime Date,
       string Time) : IRequest<IResult>;

    public class TripRequestAddCommandHandler(ITripRequestRepository _tripRequestRepository,
        ILogger<TripRequestAddCommandHandler> _logger,
        IHttpClientFactory _httpClientFactory) : IRequestHandler<TripRequestAddCommand, IResult>
    {
        public async Task<IResult> Handle(TripRequestAddCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("{FunctionName} with request: {@RequestData}",
                nameof(TripRequestAddCommandHandler), request);

            Domain.Entities.TripRequest tripRequest = new()
            {
                VehicleName = request.VehicleName,
                From = request.From,
                To = request.To,
                Date = request.Date,
                Time = request.Time,
            };

            await _tripRequestRepository.AddAsync(tripRequest);
            await _tripRequestRepository.SaveChangesAsync();

            if (tripRequest.Id > 0)
            {
                _logger.LogInformation("{FunctionName} Successfully added Trip Request. Request : {@TripRequest}",
                        nameof(TripRequestAddCommandHandler), JsonConvert.SerializeObject(tripRequest));

                /*
                // Publish message to RabbitMQ
                var client = _httpClientFactory.CreateClient();
                var response = await client.PostAsJsonAsync("http://localhost:5108/api/rabbitmq/publish", tripRequest);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("Failed to publish message to RabbitMQ. Status Code: {StatusCode}", response.StatusCode);
                    return Results.StatusCode((int)response.StatusCode);
                }
                */

                return Results.Ok(new
                {
                    Message = "Trip Request has been added successfully.",
                    TripRequest = request
                });
            }
            else
            {
                _logger.LogWarning("{FunctionName} failed to add Trip Request with request: {@RequestData}",
                         nameof(TripRequestAddCommandHandler), request);

                return Results.BadRequest(new
                {
                    Message = "Failed to add Trip Request. Please try again."
                });
            }
        }
    }
}
