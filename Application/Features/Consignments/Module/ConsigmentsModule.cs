

using Application.Features.Consignments.Command.AddConsignment;
using Application.Features.Consignments.Command.DeleteConsignment;
using Application.Features.Consignments.Command.UpdateConsignment;
using Application.Features.Consignments.Queries.GetAll;
using Application.Features.Consignments.Queries.GetById;
using Carter;

namespace Application.Features.ConsignmentDetails.Module;

public class ConsignmentDetailsModule : CarterModule
{
    public ConsignmentDetailsModule() : base("")
    {
        WithTags("ConsignmentDetails");
        IncludeInOpenApi();
        //RequireAuthorization();
    }
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app = app.MapGroup("consignmentdetails");
        app.MapPost("", (ISender mediator, ConsignmentsCommand command) =>
        {
            return mediator.Send(command);
        });
        app.MapDelete("/{id:int}", (int id, ISender mediator) =>
        {
            return mediator.Send(new ConsignmentsDeleteCommand(id));
        });
        app.MapPut("", (ISender mediator, ConsignmentsUpdateCommand command) =>
        {
            return mediator.Send(command); 
        });
        app.MapGet("", (ISender mediator, int PageNumber = 1, int PageSize = 10) =>
        {
            return mediator.Send(new GetAllConsignmentsQuery(PageNumber,PageSize));
        });
        app.MapGet("/{id:int}", (ISender mediator, int id) =>
        {
            return mediator.Send(new GetConsignmentsByIdQuery(id));
        });
    }
}
