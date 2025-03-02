using Application.Features.TripRequest.Command.TripRequestAdd;
using Application.Features.TripRequest.Command.TripRequestUpdate;
using Application.Features.TripRequest.Query.GetAllTripRequest;
using Application.Features.TripRequest.Query.GetTripRequestById;
using Carter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.TripRequest.Module
{
    public class TripRequestModule : CarterModule
    {
        public TripRequestModule() : base("api")
        {
            WithTags("TripRequest");
            IncludeInOpenApi();
            // RequireAuthorization();
        }

        public override void AddRoutes(IEndpointRouteBuilder app)
        {
            app = app.MapGroup("trip-request");

            app.MapPost("", (IMediator mediator, TripRequestAddCommand command) =>
            {
                return mediator.Send(command);
            })
            .DisableAntiforgery();

            app.MapGet("", (IMediator mediator, int pageNumber = 1, int pageSize = 10) =>
            {
                return mediator.Send(new GetAllTripRequestQuery(pageNumber, pageSize));
            });

            app.MapGet("/{id:int}", (int id, IMediator mediator) =>
            {
                return mediator.Send(new GetTripRequestByIdQuery(id));
            });

            app.MapPut("", (IMediator mediator, TripRequestUpdateCommand command) =>
            {
                return mediator.Send(command);
            });
        }
    }
}
