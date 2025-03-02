using Application.Features.TrackVehicles.Command.TrackVehiclesAdd;
using Application.Features.TrackVehicles.Command.TrackVehiclesUpdate;
using Application.Features.TrackVehicles.Query.GetAllTrackVehicles;
using Application.Features.TrackVehicles.Query.GetTrackVehiclesById;
using Carter;

namespace Application.Features.TrackVehicles.Module
{
    public class TrackVehiclesModule : CarterModule
    {
        public TrackVehiclesModule() : base("api/track-vehicles")
        {
            WithTags("Track Vehicles");
            IncludeInOpenApi();
        }

        public override void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("", (IMediator mediator, TrackVehiclesAddCommand command) =>
            {
                return mediator.Send(command);
            })
            .DisableAntiforgery();

            app.MapGet("", (IMediator mediator, int pageNumber = 1, int pageSize = 10) =>
            {
                return mediator.Send(new GetAllTrackVehiclesQuery(pageNumber, pageSize));
            });

            app.MapPut("", (IMediator mediator, TrackVehiclesUpdateCommand command) =>
            {
                return mediator.Send(command);
            });

            app.MapGet("/{id:int}", (int id, IMediator mediator) =>
            {
                return mediator.Send(new GetTrackVehiclesByIdQuery(id));
            });
        }
    }
}
