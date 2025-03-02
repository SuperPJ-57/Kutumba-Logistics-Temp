using Application.Features.Vehicles.Command.AddVehicles;
using Application.Features.Vehicles.Command.DeleteVehicles;
using Application.Features.Vehicles.Command.UpdateVehicles;
using Application.Features.Vehicles.Queries.GetAllVehicle;
using Application.Features.Vehicles.Queries.GetVehicleById;
using Carter;

namespace Application.Features.Vehicles.Module;

public class VehiclesModule : CarterModule
{
    public VehiclesModule() : base("")
    {
        WithTags("Vehicles");
        IncludeInOpenApi();
        //RequireAuthorization();
    }

    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app = app.MapGroup("vehicles");
        app.MapPost("", (ISender mediator, VehiclesCommand command) =>
        {
            return mediator.Send(command);
        });
        app.MapDelete("/{id:int}", (int id, ISender mediator) =>
        {
            return mediator.Send(new DeleteVehicleCommand(id));
        });
        app.MapPut("", (ISender mediator, VehiclesUpdateCommand command) =>
        {
            return mediator.Send(command);
        });
        app.MapGet("", (IMediator mediator, int pageNumber = 1, int pageSize = 10) =>
        {
            return mediator.Send(new GetAllVehiclesQuery(pageNumber, pageSize));
        });
        app.MapGet("/{id:int}", (int id, ISender mediator) =>
        {
            return mediator.Send(new GetVehicleByIdQuery(id));
        });

    }
}
