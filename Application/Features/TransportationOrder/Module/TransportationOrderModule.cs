
using Application.Features.TrackVehicles.Command.TrackVehiclesAdd;
using Application.Features.TransportationOrder.Command.TransportationOrderAdd;
using Application.Features.TransportationOrder.Query.GetAllTransportationOrder;
using Carter;
using Microsoft.AspNetCore.Mvc;


namespace Application.Features.TransportationOrder.Module
{
    public class TransportationOrderModule : CarterModule
    {
        public TransportationOrderModule() : base("api")
        {
            WithTags("TransportationOrders");
            IncludeInOpenApi();
            // RequireAuthorization();
        }

        public override void AddRoutes(IEndpointRouteBuilder app)
        {
            app = app.MapGroup("transportation-orders");

            app.MapPost("", (IMediator mediator,[FromForm] TransportationOrderAddCommand command) =>
            {
                return mediator.Send(command);
            })
            .DisableAntiforgery();

            app.MapGet("", (IMediator mediator, int pageNumber = 1, int pageSize = 10) =>
            {
                return mediator.Send(new GetAllTransportationOrderQuery(pageNumber, pageSize));
            });
        }
    }


}
