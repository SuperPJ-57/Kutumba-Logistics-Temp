//using Application.Features.Accounts.Command.AccountsAdd;
//using Application.Features.Accounts.Command.AccountsDelete;
//using Application.Features.Accounts.Command.AccountsUpdate;
//using Application.Features.Accounts.Query.GetAccountsById;
//using Application.Features.Accounts.Query.GetAllAccounts;
//using Carter;

//namespace Application.Features.Accounts.Module
//{

//    public class AccountsModule : CarterModule
//    {
//        public AccountsModule() : base("")
//        {
//            WithTags("Accounts");
//            IncludeInOpenApi();
//            RequireAuthorization();
//        }

//        public override void AddRoutes(IEndpointRouteBuilder app)
//        {
//            app = app.MapGroup("accounts");

//            app.MapPost("", (IMediator mediator, AccountsAddCommand command) =>
//            {
//                return mediator.Send(command);
//            });

//            app.MapPut("", (IMediator mediator, AccountsUpdateCommand command) =>
//            {
//                return mediator.Send(command);
//            });

//            app.MapDelete("/{id:int}", (int id, IMediator mediator) =>
//            {
//                return mediator.Send(new AccountsDeleteCommand(id));
//            });

//            app.MapGet("", ( IMediator mediator, int pageNumber = 1, int pageSize = 10) =>
//            {
//                return mediator.Send(new GetAllAccountsQuery(pageNumber,pageSize));
//            });

//            app.MapGet("/{id:int}", (int id, IMediator mediator) =>
//            {
//                return mediator.Send(new GetAccountsByIdQuery(id));
//            });
//        }
//    }
//}
    
