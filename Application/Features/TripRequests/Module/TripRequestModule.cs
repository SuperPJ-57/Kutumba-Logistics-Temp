using Application.Features.TripRequests.Command.AddTripRequest;
using Application.Features.TripRequests.Command.DeleteTripRequest;
using Application.Features.TripRequests.Command.UpdateTripRequest;
using Application.Features.TripRequests.Queries.GetAll;
using Application.Features.TripRequests.Queries.GetById;

using Carter;

namespace Application.Features.TripRequests.Module;

public class TripRequestModule : CarterModule
{
    public TripRequestModule() : base("")
    {
        WithTags("TripRequest");
        IncludeInOpenApi();
        //RequireAuthorization();
    }
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app = app.MapGroup("tripRequest");
        app.MapPost("", (ISender mediator, TripRequestCommand command) =>
        {
            return mediator.Send(command);
        });
        app.MapGet("/{Id:int}", (int id, ISender mediator) =>
        {
            return mediator.Send(new GetTripRequestByIdQuery(id));
        });
        app.MapGet("", (ISender mediator, int pageNumber = 1, int pageSize = 10) =>
        {
            return mediator.Send(new GetAllTripRequestQuery(pageNumber, pageSize));    
        });
        app.MapDelete("/{Id:int}", (int id, ISender mediator) =>
        {
            return mediator.Send(new TripRequestDeleteCommand(id));
        });
        app.MapPut("", (ISender mediator, TripRequestUpdateCommand command) =>
        {
            return mediator.Send(command);
        });
    }
}
