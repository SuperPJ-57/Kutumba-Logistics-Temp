
using Application.Features.TripLogging.Command.TripLoggingAdd;
using Application.Features.TripLogging.Command.TripLoggingUpdate;
using Application.Features.TripLogging.Query.GetAllTripLogging;
using Application.Features.TripLogging.Query.GetTripLoggingById;
using Carter;
using Microsoft.AspNetCore.Mvc;

namespace Application.Features.TripLogging.Module
{
    public class TripLoggingModule : CarterModule
    {
        public TripLoggingModule() : base("api")
        {
            WithTags("Trip Logging");
            IncludeInOpenApi();
        }

        public override void AddRoutes(IEndpointRouteBuilder app)
        {
            app = app.MapGroup("trip-logging");

            app.MapPost("", (IMediator mediator,  TripLoggingAddCommand command) =>
            {
                return mediator.Send(command);
            })
            .DisableAntiforgery();

            app.MapPut("", (IMediator mediator, TripLoggingUpdateCommand command) =>
            {
                return mediator.Send(command);
            })
            .DisableAntiforgery();

            app.MapGet("", (IMediator mediator, int pageNumber = 1, int pageSize = 10) =>
            {
                return mediator.Send(new GetAllTripLoggingQuery(pageNumber, pageSize));
            });

            app.MapGet("/{id:int}", (int id, IMediator mediator) =>
            {
                return mediator.Send(new GetTripLoggingByIdQuery(id));
            });

        }
    }
}
