//using Application.Features.DamageLosses.Command.DamageLossesDelete;
//using Application.Interfaces.Repositories;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Logging;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Application.Features.Accounts.Command.AccountsDelete
//{
//    public record AccountsDeleteCommand(int Id) : IRequest<IResult>;

//    public class AccountsDeleteCommandHandler(IAccountsRepository _accountsRepository,
//        ILogger<AccountsDeleteCommandHandler> _logger
//        ) : IRequestHandler<AccountsDeleteCommand, IResult>
//    {
//        public async Task<IResult> Handle(AccountsDeleteCommand request, CancellationToken cancellationToken)
//        {
//            _logger.LogInformation("{FunctionName} received request: {@RequestData}",nameof(AccountsDeleteCommandHandler), request);

//            try
//            {
//                var account = await _accountsRepository.Queryable.FirstOrDefaultAsync(a => a.Id == request.Id, cancellationToken);
//                if (account == null)
//                {
//                    _logger.LogWarning("{FunctionName} failed. Account with ID: {@RequestId} not found.",
//                        nameof(AccountsDeleteCommandHandler), request.Id);
                    
//                    return Results.NotFound(new 
//                    { 
//                        Message = "Account not found."
//                    });
//                }

//                await _accountsRepository.RemoveAsync(account);
//                await _accountsRepository.SaveChangesAsync();

//                _logger.LogInformation("{FunctionName} successfully deleted account with ID: {@AccountID}",
//                    nameof(AccountsDeleteCommandHandler), account.Id);

//                return Results.Ok(new 
//                { 
//                    Message = "Account has been deleted."
//                });
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "{FunctionName} failed to delete accounts with request: {@RequestData}",
//                    nameof(DamageLossesDeleteCommandHandler), request);
//                return Results.BadRequest(new 
//                { 
//                    Message = "Failed to delete account. Please try again."
//                });
//            }
//        }
//    }
//}
