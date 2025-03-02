using Application.Dto.TripDetails;
using Application.Interfaces.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.TripDetails.Command.TripDetailsUpdate
{
    public record TripDetailsUpdateCommand(
        int Id,
        string VehicleName,
        string From,
        string To,
        double Latitude,
        double Longitude) : IRequest<IResult>;

    public class TripDetailsUpdateCommandHandler(
    ITripDetailsRepository _tripDetailsRepository,
    ILogger<TripDetailsUpdateCommandHandler> _logger,
    IWebHostEnvironment _webHostEnvironment,
    IHttpClientFactory _httpClientFactory) : IRequestHandler<TripDetailsUpdateCommand, IResult>
    {
        public async Task<IResult> Handle(TripDetailsUpdateCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("{FunctionName} with request: {@RequestData}",
                nameof(TripDetailsUpdateCommandHandler), request);

            // Fetch existing trip logging entry
            var tripDetail = await _tripDetailsRepository.GetByIdAsync(request.Id);
            if (tripDetail == null)
            {
                _logger.LogWarning("TripDetails entry with ID {Id} not found", request.Id);
                return Results.NotFound(new { Message = "TripDetails entry not found." });
            }

            

            // Update entity
            tripDetail.VehicleName = request.VehicleName;
            tripDetail.From = request.From;
            tripDetail.To = request.To;
            tripDetail.Latitude = request.Latitude;
            tripDetail.Longitude = request.Longitude;
            

            await _tripDetailsRepository.UpdateAsync(tripDetail);
            await _tripDetailsRepository.SaveChangesAsync();

            // Publish update to RabbitMQ
            //var client = _httpClientFactory.CreateClient();
            //var response = await client.PostAsJsonAsync(
            //    "http://localhost:5108/api/rabbitmq/publish-trip-details-update",
            //    new TripDetailsDto
            //    {
            //        Id = tripDetail.Id,
            //        VehicleName = tripDetail.VehicleName,
            //        Destination = $"{tripDetail.From} - {tripDetail.To}",
            //        Latitude = tripDetail.Latitude,
            //        Longitude = tripDetail.Longitude
            //    }
            //);

            //if (!response.IsSuccessStatusCode)
            //{
            //    _logger.LogWarning("Failed to publish TripDetails update. Status: {StatusCode}", response.StatusCode);
            //    return Results.StatusCode((int)response.StatusCode);
            //}

           

            return Results.Ok(new
            {
                Message = "TripDetails updated successfully",
                TripDetails = tripDetail
            });
        }
    }
}
