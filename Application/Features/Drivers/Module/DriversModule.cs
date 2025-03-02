
using Application.Features.Drivers.Command.AddDrivers;
using Application.Features.Drivers.Command.DeleteDrivers;
using Application.Features.Drivers.Command.UpdateDrivers;
using Application.Features.Drivers.Queries.GetAll;
using Application.Features.Drivers.Queries.GetById;
using Carter;


namespace Application.Features.Drivers.Module;

public class DriversModule : CarterModule
{
    public DriversModule() : base("")
    {
        WithTags("Drivers");
        IncludeInOpenApi();
        //RequireAuthorization();
    }
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app = app.MapGroup("drivers");
        app.MapPost("", (ISender mediator, DriversCommand command) =>
        {
            return mediator.Send(command);
        });
        app.MapGet("", (IMediator mediator, int pageNumber = 1, int pageSize = 10) =>
        {
            return mediator.Send(new GetAllDriversQuery(pageNumber, pageSize));
        });
        app.MapGet("/{driverId:int}", (int driverId, ISender mediator) =>
        {
            var command = new GetDriversByIdQuery(driverId);
            return mediator.Send(command);
        });
        app.MapDelete("/{driverId:int}", (int driverId,ISender mediator) =>
        {
            return mediator.Send(new DriversDeleteCommand(driverId));
        });
        app.MapPut("", (ISender mediator, DriversUpdateCommand command) =>
        {
            return mediator.Send(command);
        });
        
    }
}
