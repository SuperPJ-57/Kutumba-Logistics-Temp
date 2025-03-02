using Application.Interfaces.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Application.Features.TransportationOrder.Command.TransportationOrderAdd
{
    public record TransportationOrderAddCommand(
        string AssignVehicle,
        string ConsignmentPriority,
        decimal FreightRate,
        string LoadingPoint,
        string UnloadingPoint,
        string ClientName,
        string ClientContact,
        DateTime? StartDate,
        DateTime? DeliveryDate,
        decimal TripAllowance,
        decimal MaintenanceFee,
        decimal DriverAllowance,
        string Custom1,
        string Custom2,
        string Custom3,
        IFormFile DocumentFile) : IRequest<IResult>;
    public class TransportationOrderAddCommandHandler : IRequestHandler<TransportationOrderAddCommand, IResult>
    {
        private readonly ITransportationOrderRepository _transportationOrderRepository;
        private readonly ILogger<TransportationOrderAddCommandHandler> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public TransportationOrderAddCommandHandler(
            ITransportationOrderRepository transportationOrderRepository,
            ILogger<TransportationOrderAddCommandHandler> logger,
            IWebHostEnvironment webHostEnvironment)
        {
            _transportationOrderRepository = transportationOrderRepository;
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IResult> Handle(TransportationOrderAddCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("{FunctionName} with request: {@RequestData}",
                nameof(TransportationOrderAddCommandHandler), request);

            // Handle file upload
            string documentPath = null;
            if (request.DocumentFile != null)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }
                var extension = Path.GetExtension(request.DocumentFile.FileName);
                var randomFileName = Path.GetRandomFileName().Replace(".", "");
                var fileName = randomFileName + extension;
                documentPath = Path.Combine(uploadsFolder, fileName);

                using (var fileStream = new FileStream(documentPath, FileMode.Create))
                {
                    await request.DocumentFile.CopyToAsync(fileStream);
                }
                documentPath = Path.Combine("uploads", fileName);
                _logger.LogInformation("File uploaded successfully: {DocumentPath}", documentPath);
            }

            Domain.Entities.TransportationOrder transportationOrder = new()
            {
                AssignVehicle = request.AssignVehicle,
                ConsignmentPriority = request.ConsignmentPriority,
                FreightRate = request.FreightRate,
                LoadingPoint = request.LoadingPoint,
                UnloadingPoint = request.UnloadingPoint,
                ClientName = request.ClientName,
                ClientContact = request.ClientContact,
                StartDate = request.StartDate,
                DeliveryDate = request.DeliveryDate,
                TripAllowance = request.TripAllowance,
                MaintenanceFee = request.MaintenanceFee,
                DriverAllowance = request.DriverAllowance,
                Custom1 = request.Custom1,
                Custom2 = request.Custom2,
                Custom3 = request.Custom3,
                DocumentPath = documentPath,
            };

            await _transportationOrderRepository.AddAsync(transportationOrder);
            await _transportationOrderRepository.SaveChangesAsync();

            if (transportationOrder.Id > 0)
            {
                _logger.LogInformation("{FunctionName} Successfully added transportation order. Details: {@TransportationOrderDetails}",
                    nameof(TransportationOrderAddCommandHandler), JsonConvert.SerializeObject(transportationOrder));

                return Results.Ok(new
                {
                    Message = "Transportation order has been added.",
                    TransportationOrder = transportationOrder
                });
            }
            else
            {
                _logger.LogWarning("{FunctionName} failed to add transportation order with request: {@RequestData}",
                    nameof(TransportationOrderAddCommandHandler), request);

                return Results.BadRequest(new
                {
                    Message = "Failed to add transportation order. Please try again."
                });
            }
        }

    }
}
