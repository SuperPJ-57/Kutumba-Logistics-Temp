using Application.Dto.TripLogging;
using Application.Interfaces.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;

namespace Application.Features.TripLogging.Command.TripLoggingUpdate
{
    public record TripLoggingUpdateCommand(
        int Id,
        string LoadingPoint,
        string ReFilledFuelStation,
        string Weight,
        double Latitude,
        double Longitude,
        IFormFile? ImageFile) : IRequest<IResult>;

    public class TripLoggingUpdateCommandHandler(
    ITripLoggingRepository _tripLoggingRepository,
    ILogger<TripLoggingUpdateCommandHandler> _logger,
    IWebHostEnvironment _webHostEnvironment,
    IHttpClientFactory _httpClientFactory) : IRequestHandler<TripLoggingUpdateCommand, IResult>
    {
        public async Task<IResult> Handle(TripLoggingUpdateCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("{FunctionName} with request: {@RequestData}",
                nameof(TripLoggingUpdateCommandHandler), request);

            // Fetch existing trip logging entry
            var existingTrip = await _tripLoggingRepository.GetByIdAsync(request.Id);
            if (existingTrip == null)
            {
                _logger.LogWarning("TripLogging entry with ID {Id} not found", request.Id);
                return Results.NotFound(new { Message = "TripLogging entry not found." });
            }

            // Handle image update
            string imagePath = existingTrip.ImagePath;
            if (request.ImageFile != null)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
                if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);

                var extension = Path.GetExtension(request.ImageFile.FileName);
                var randomFileName = Path.GetRandomFileName().Replace(".", "") + extension;
                var newImagePath = Path.Combine(uploadsFolder, randomFileName);

                using (var fileStream = new FileStream(newImagePath, FileMode.Create))
                {
                    await request.ImageFile.CopyToAsync(fileStream);
                }

                // Delete old image if it exists
                if (!string.IsNullOrEmpty(imagePath))
                {
                    var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, imagePath.TrimStart('/'));
                    if (File.Exists(oldImagePath)) File.Delete(oldImagePath);
                }

                imagePath = $"/uploads/{randomFileName}";
                _logger.LogInformation("Updated image path: {ImagePath}", imagePath);
            }

            // Update entity
            existingTrip.LoadingPoint = request.LoadingPoint;
            existingTrip.ReFilledFuelStation = request.ReFilledFuelStation;
            existingTrip.Weight = request.Weight;
            existingTrip.Latitude = request.Latitude;
            existingTrip.Longitude = request.Longitude;
            existingTrip.ImagePath = imagePath;

            await _tripLoggingRepository.UpdateAsync(existingTrip);
            await _tripLoggingRepository.SaveChangesAsync();

            // Publish update to RabbitMQ
            var client = _httpClientFactory.CreateClient();
            var response = await client.PostAsJsonAsync(
                "http://localhost:5108/api/rabbitmq/publish-trip-logging-update",
                new TripLoggingDto(
                    existingTrip.Id,
                    existingTrip.LoadingPoint,
                    existingTrip.ReFilledFuelStation,
                    existingTrip.Weight,
                    existingTrip.Latitude,
                    existingTrip.Longitude,
                    existingTrip.ImagePath
                )
            );

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("Failed to publish TripLogging update. Status: {StatusCode}", response.StatusCode);
                return Results.StatusCode((int)response.StatusCode);
            }

            return Results.Ok(new
            {
                Message = "TripLogging updated successfully",
                TripLogging = existingTrip
            });
        }
    }
}
