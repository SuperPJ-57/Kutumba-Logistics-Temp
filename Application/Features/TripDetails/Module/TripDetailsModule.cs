using Application.Features.TripDetails.Command.TripDetailsAdd;
using Application.Features.TripDetails.Command.TripDetailsUpdate;
using Application.Features.TripDetails.Query.GetAllTripDetails;
using Application.Features.TripDetails.Query.GetTripDetailsById;
using Carter;

namespace Application.Features.TripDetails.Module
{
    public class TripDetailsModule : CarterModule
    {
        public TripDetailsModule() : base("api")
        {
            WithTags("TripDetails");
            IncludeInOpenApi();
            // RequireAuthorization();
        }

        public override void AddRoutes(IEndpointRouteBuilder app)
        {
            app = app.MapGroup("trip-details");

            app.MapPost("", (IMediator mediator, TripDetailsAddCommand command) =>
            {
                return mediator.Send(command);
            })
            .DisableAntiforgery();

            app.MapGet("", (IMediator mediator, int pageNumber = 1, int pageSize = 10) =>
            {
                return mediator.Send(new GetAllTripDetailsQuery(pageNumber, pageSize));
            });

            app.MapGet("/{id:int}", (int id, IMediator mediator) =>
            {
                return mediator.Send(new GetTripDetailsByIdQuery(id));
            });

            app.MapPut("", (IMediator mediator, TripDetailsUpdateCommand command) =>
            {
                return mediator.Send(command);
            });
        }
    }
}
