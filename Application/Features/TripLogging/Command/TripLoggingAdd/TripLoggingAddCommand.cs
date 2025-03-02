using Application.Interfaces.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http.Json;

namespace Application.Features.TripLogging.Command.TripLoggingAdd
{
    public record TripLoggingAddCommand(
        string LoadingPoint,
        string ReFilledFuelStation,
        string Weight,
        double Latitude,
        double Longitude,
        IFormFile ImageFile) : IRequest<IResult>;

    public class TripLoggingAddCommandHandler(
            ITripLoggingRepository _tripLoggingRepository,
            ILogger<TripLoggingAddCommandHandler> _logger,
            IWebHostEnvironment _webHostEnvironment,
            IHttpClientFactory _httpClientFactory) : IRequestHandler<TripLoggingAddCommand, IResult>
    {

        public async Task<IResult> Handle(TripLoggingAddCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("{FunctionName} with request: {@RequestData}",
                nameof(TripLoggingAddCommandHandler), request);

            // Handle image file upload
            string imagePath = null;
            if (request.ImageFile != null)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }
                var extension = Path.GetExtension(request.ImageFile.FileName);
                var randomFileName = Path.GetRandomFileName().Replace(".", "");
                var fileName = randomFileName + extension;
                imagePath = Path.Combine(uploadsFolder, fileName);

                using (var fileStream = new FileStream(imagePath, FileMode.Create))
                {
                    await request.ImageFile.CopyToAsync(fileStream);
                }
                imagePath = Path.Combine("/uploads", fileName);
                _logger.LogInformation("Image uploaded successfully: {@ImagePath}", imagePath);
            }

            Domain.Entities.TripLogging tripLogging = new()
            {
                LoadingPoint = request.LoadingPoint,
                ReFilledFuelStation = request.ReFilledFuelStation,
                Weight = request.Weight,
                Latitude = request.Latitude,
                Longitude = request.Longitude,
                ImagePath = imagePath,
            };

            await _tripLoggingRepository.AddAsync(tripLogging);
            await _tripLoggingRepository.SaveChangesAsync();

            if (tripLogging.Id > 0)
            {
                _logger.LogInformation("{FunctionName} Successfully added trip logging. Details: {@TripLoggingDetails}",
                    nameof(TripLoggingAddCommandHandler), JsonConvert.SerializeObject(tripLogging));

                // Publish TripLogging data to RabbitMQ
                var client = _httpClientFactory.CreateClient();
                var response = await client.PostAsJsonAsync("http://localhost:5108/api/rabbitmq/publish-trip-logging", new
                {
                    LoadingPoint = request.LoadingPoint,
                    ReFilledFuelStation = request.ReFilledFuelStation,
                    Weight = request.Weight,
                    Latitude = request.Latitude,
                    Longitude = request.Longitude,
                    ImagePath = imagePath,
                });

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("Failed to publish TripLogging message to RabbitMQ. Status Code: {StatusCode}", response.StatusCode);
                    return Results.StatusCode((int)response.StatusCode);
                }

                return Results.Ok(new
                {
                    Message = "Trip logging has been added.",
                    TripLogging = tripLogging
                });
            }
            else
            {
                _logger.LogWarning("{FunctionName} failed to add trip logging with request: {@RequestData}",
                    nameof(TripLoggingAddCommandHandler), request);

                return Results.BadRequest(new
                {
                    Message = "Failed to add trip logging. Please try again."
                });
            }
        }
    }
}
