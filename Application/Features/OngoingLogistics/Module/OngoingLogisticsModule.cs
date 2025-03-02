using Application.Features.OngoingLogistics.Command.OngoingLogisticsAdd;
using Application.Features.OngoingLogistics.Command.OngoingLogisticsUpdate;
using Application.Features.OngoingLogistics.Query.GetAllOngoingLogistics;
using Application.Features.OngoingLogistics.Query.GetOngoingLogisticsById;
using Carter;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.OngoingLogistics.Module
{
    public class OngoingLogisticsModule : CarterModule
    {
        public OngoingLogisticsModule() : base("api/ongoing-logistics")
        {
            WithTags("Ongoing Logistics");
            IncludeInOpenApi();
        }

        public override void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("", async (IMediator mediator,  OngoingLogisticsAddCommand command) =>
            {
                return await mediator.Send(command);
            })
            .DisableAntiforgery();

            app.MapGet("", (IMediator mediator, int pageNumber = 1, int pageSize = 10) =>
            {
                return mediator.Send(new GetAllOngoingLogisticsQuery(pageNumber, pageSize));
            });

            app.MapPut("", (IMediator mediator, OngoingLogisticsUpdateCommand command) =>
            {
                return mediator.Send(command);
            });

            app.MapGet("/{id:int}", (int id, IMediator mediator) =>
            {
                return mediator.Send(new GetOngoingLogisticsByIdQuery(id));
            });
        }
    }
}
