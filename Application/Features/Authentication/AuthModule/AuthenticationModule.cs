using Application.Features.Authentication.Commands.Register;
using Application.Features.Authentication.Commands.Signout.Command;
using Application.Features.Authentication.Commands.UsersDelete;
using Application.Features.Authentication.Commands.UsersUpdate;
using Application.Features.Authentication.Queries.AllUsers;
using Application.Features.Authentication.Queries.Login;
using Application.Features.Authentication.Queries.UsersById;
using Carter;



namespace Application.Features.Authentication.AuthModule;

public class AuthenticationModule : CarterModule
{
    public AuthenticationModule() : base("")
    {
        WithTags("Authentication");
        IncludeInOpenApi();
        //RequireAuthorization();
    }
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app = app.MapGroup("auth");

        app.MapPost("/register", (ISender mediator, RegisterCommand command) => {
            return mediator.Send(command);
        });

        app.MapPost("/login", (ISender mediator, LoginQuery query) =>
        {
            return mediator.Send(query);
        });

        app.MapGet("users", (ISender mediator, int PageNumber = 1, int PageSize = 10) =>
        {
            return mediator.Send(new GetAllUsersQuery(PageNumber, PageSize));
        });

        app.MapGet("users/{id:guid}", (Guid id, ISender mediator) =>
        {
            return mediator.Send(new GetUsersByIdQuery(id));
        });
        
        app.MapPut("users/{id:guid}", async (Guid id,ISender mediator, UsersUpdateCommand command) =>
        {
            return await mediator.Send(command);
        });

        app.MapDelete("users/{id:guid}", async (Guid id, ISender mediator) =>
        {
            var command = new UsersDeleteCommand(id);
            return  await mediator.Send(command);
        });
        app.MapPost("signout", async (ISender mediator, SignOutCommand command) =>
        {  
            return await mediator.Send(command); 
        });
    }
}
