using Application.Features.Freights.Command.AddFreights;
using Application.Features.Freights.Command.DeleteFreights;
using Application.Features.Freights.Command.UpdateFreights;
using Application.Features.Freights.Queries.GetAll;
using Application.Features.Freights.Queries.GetById;
using Carter;

namespace Application.Features.Freights.Module;

public class FreightsModule : CarterModule
{
    public FreightsModule() : base("")
    {
        WithTags("Freight");
        IncludeInOpenApi();
        //RequireAuthorization();
    }

    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app = app.MapGroup("freights");
        app.MapPost("", (ISender mediator, FreightCommand command) =>
        {
            return mediator.Send(command);
        });
        app.MapDelete("/{freightId:int}", (int freightId, ISender mediator) =>
        {
            return mediator.Send(new FreightDeleteCommand(freightId));
        });
        app.MapPut("", (ISender medaitor, FreightUpdateCommand command) =>
        {
            return medaitor.Send(command);
        });
        app.MapGet("/{freightId:int}", (int freightId, ISender mediator) => 
        {
            return mediator.Send(new GetFreightByIdQuery(freightId));
        });
        app.MapGet("", (ISender mediator, int PageNumber=1, int PageSize=10) => 
        {
            return mediator.Send(new GetAllFreightQuery(PageNumber, PageSize));
        });
    }
}
