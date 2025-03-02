using Application.Interfaces.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;

namespace Application.Features.TripRequest.Command.TripRequestUpdate
{
    public record TripRequestUpdateCommand(
        int Id,
        string VehicleName,
        string From,
        string To,
        DateTime Date,
        string Time) : IRequest<IResult>;

    public class TripRequestUpdateCommandHandler(
    ITripRequestRepository _tripRequestRepository,
    ILogger<TripRequestUpdateCommandHandler> _logger,
    IWebHostEnvironment _webHostEnvironment,
    IHttpClientFactory _httpClientFactory) : IRequestHandler<TripRequestUpdateCommand, IResult>
    {
        public async Task<IResult> Handle(TripRequestUpdateCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("{FunctionName} with request: {@RequestData}",
                nameof(TripRequestUpdateCommandHandler), request);

            // Fetch existing trip logging entry
            var tripRequest = await _tripRequestRepository.GetByIdAsync(request.Id);
            if (tripRequest == null)
            {
                _logger.LogWarning("TripRequest entry with ID {Id} not found", request.Id);
                return Results.NotFound(new { Message = "TripRequest entry not found." });
            }



            // Update entity
            tripRequest.VehicleName = request.VehicleName;
            tripRequest.From = request.From;
            tripRequest.To = request.To;
            tripRequest.Date = request.Date;
            tripRequest.Time = request.Time;


            await _tripRequestRepository.UpdateAsync(tripRequest);
            await _tripRequestRepository.SaveChangesAsync();

            // Publish update to RabbitMQ
            //var client = _httpClientFactory.CreateClient();
            //var response = await client.PostAsJsonAsync(
            //    "http://localhost:5108/api/rabbitmq/publish-trip-details-update",
            //    new TripRequestDto
            //    {
            //        Id = tripRequest.Id,
            //        VehicleName = tripRequest.VehicleName,
            //        Destination = $"{tripRequest.From} - {tripRequest.To}",
            //        Date = tripRequest.Date,
            //        Time = tripRequest.Time
            //    }
            //);

            //if (!response.IsSuccessStatusCode)
            //{
            //    _logger.LogWarning("Failed to publish TripRequest update. Status: {StatusCode}", response.StatusCode);
            //    return Results.StatusCode((int)response.StatusCode);
            //}



            return Results.Ok(new
            {
                Message = "TripRequest updated successfully",
                TripRequest = tripRequest
            });
        }
    }
}
