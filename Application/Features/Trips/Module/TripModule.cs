using Application.Features.Trips.Command.AddTrip;
using Application.Features.Trips.Command.DeleteTrip;
using Application.Features.Trips.Command.UpdateTrip;
using Application.Features.Trips.Queries.GetAll;
using Application.Features.Trips.Queries.GetById;
using Carter;

namespace Application.Features.Trips.Module;

public class TripModule : CarterModule
{
    public TripModule() : base("")
    {
        WithTags("Trips");
        IncludeInOpenApi();
        //RequireAuthorization();
    }
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app = app.MapGroup("trip");
        app.MapPost("", (ISender mediator, TripCommand command) =>
        {
            return mediator.Send(command);
        });
        app.MapPut("", (ISender mediator, TripUpdateCommand command) =>
        {
            return mediator.Send(command);
        });
        app.MapDelete("/{id:int}", (int id, ISender mediator) =>
        {
            return mediator.Send(new TripDeleteCommand(id));
        });
        app.MapGet("", (ISender mediator, int PageNumber = 1, int PageSize = 10) =>
        {
            return mediator.Send(new GetAllTripQuery(PageNumber, PageSize));
        });
        app.MapGet("/{id:int}", (int id, ISender mediator) =>
        {
            var command = new GetTripByIdQuery(id);
            return mediator.Send(command);
        });
    }
}
