using Application.Dto.TransportationOrder;
using Application.Interfaces.Repositories;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;


namespace Application.Features.TransportationOrder.Query.GetAllTransportationOrder
{
    public record GetAllTransportationOrderQuery(int PageNumber, int PageSize) : IRequest<IResult>;

    public class GetAllTransportationOrderQueryHandler(ITransportationOrderRepository _transportationOrderRepository,
        ILogger<GetAllTransportationOrderQueryHandler> _logger
        ) : IRequestHandler<GetAllTransportationOrderQuery, IResult>
    {
        public async Task<IResult> Handle(GetAllTransportationOrderQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("{FunctionName} received a request to retrieve all transportation orders.",
                nameof(GetAllTransportationOrderQueryHandler));

            try
            {
                var totalCount = _transportationOrderRepository.Queryable.Count();

                var totalPages = (int)Math.Ceiling((double)totalCount / request.PageSize);

                var transportationOrders = await _transportationOrderRepository
                    .GetAllAsNoTrackingAsync(request.PageNumber, request.PageSize);

                var transportationOrderDtos = transportationOrders.Select(to => new TransportationOrderDto
                {
                    Id = to.Id,
                    AssignVehicle = to.AssignVehicle,
                    ConsignmentPriority = to.ConsignmentPriority,
                    LoadingPoint = to.LoadingPoint,
                    UnloadingPoint = to.UnloadingPoint,
                    ClientName = to.ClientName,
                    ClientContact = to.ClientContact,
                    StartDate = to.StartDate,
                    DeliveryDate = to.DeliveryDate,
                    DriverAllowance = to.DriverAllowance,
                    FreightRate = to.FreightRate,
                    MaintenanceFee = to.MaintenanceFee,
                    TripAllowance = to.TripAllowance,
                    Custom1 = to.Custom1,
                    Custom2 = to.Custom2,
                    Custom3 = to.Custom3,
                    DocumentPath = to.DocumentPath
                }).ToList();

                _logger.LogInformation("{FunctionName} successfully retrieved all transportation orders. Count: {@Count}. Result: {@TransportationOrdersDetails}",
                    nameof(GetAllTransportationOrderQueryHandler), totalCount, JsonConvert.SerializeObject(transportationOrderDtos));

                return Results.Ok(new
                {
                    TransportationOrders = transportationOrderDtos,
                    Meta = new
                    {
                        PageNumber = request.PageNumber,
                        PageSize = request.PageSize,
                        TotalCount = totalCount,
                        TotalPages = totalPages
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{FunctionName} error occurred while retrieving all transportation orders.",
                    nameof(GetAllTransportationOrderQueryHandler));

                return Results.BadRequest(new
                {
                    Message = "An error occurred while retrieving transportation orders. Please try again later."
                });
            }
        }
    }
}
