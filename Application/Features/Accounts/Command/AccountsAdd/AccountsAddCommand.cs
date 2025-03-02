//using Application.Interfaces.Repositories;
//using Microsoft.Extensions.Logging;
//using Newtonsoft.Json;

//namespace Application.Features.Accounts.Command.AccountsAdd
//{
//    public record AccountsAddCommand(
//        int SubLedgerId,
//        string AccountNumber,
//        string Name,
//        string Address,
//        string ContactInfo,
//        string Email,
//        string PANNumber,
//        string BackUpContact,
//        decimal? CreditLimit,
//        DateTime? CreditDays,
//        int CompanyInfoId,
//        bool IsTaxable,
//        int UnitId) : IRequest<IResult>;

//    public class AccountsAddCommandHandler(IAccountsRepository _accountsRepository,
//        ILogger<AccountsAddCommandHandler> _logger
//        ) : IRequestHandler<AccountsAddCommand, IResult>
//    {
//        public async Task<IResult> Handle(AccountsAddCommand request, CancellationToken cancellationToken)
//        {
//            _logger.LogInformation("{FunctionName} with request: {@RequestData}", 
//                nameof(AccountsAddCommandHandler), request);

//            Domain.TenantEntities.Accounts account = new()
//            {
//                SubLedgerId = request.SubLedgerId,
//                AccountNumber = request.AccountNumber,
//                Name = request.Name,
//                Address = request.Address,
//                ContactInfo = request.ContactInfo,
//                Email = request.Email,
//                PANNumber = request.PANNumber,
//                BackUpContact = request.BackUpContact,
//                CreditLimit = request.CreditLimit,
//                CreditDays = request.CreditDays,
//                CompanyInfoId = request.CompanyInfoId,
//            };

//            await _accountsRepository.AddAsync(account);
//            await _accountsRepository.SaveChangesAsync();

//            if (account.Id > 0)
//            {
//                _logger.LogInformation("{FunctionName} Successfully added account. Details : {@AccountDetails}",
//                        nameof(AccountsAddCommandHandler),JsonConvert.SerializeObject(account));

//                return Results.Ok(new 
//                { 
//                    Message = "Account has been added.", 
//                    Account = request 
//                });
//            }
//            else
//            {
//                _logger.LogWarning("{FunctionName} failed to add account with request: {@RequestData}",
//                         nameof(AccountsAddCommandHandler), request);

//                return Results.BadRequest(new 
//                { 
//                    Message = "Failed to add account. Please try again." 
//                });

//            }
//        }
//    }
//}
