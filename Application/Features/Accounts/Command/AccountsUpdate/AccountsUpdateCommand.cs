//using Application.Interfaces.Repositories;
//using Microsoft.Extensions.Logging;
//using Newtonsoft.Json;

//namespace Application.Features.Accounts.Command.AccountsUpdate
//{
//    public record AccountsUpdateCommand(
//        int Id,
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

//    public class AccountsUpdateCommandHandler(IAccountsRepository _accountsRepository,
//        ILogger<AccountsUpdateCommandHandler> _logger
//        ) : IRequestHandler<AccountsUpdateCommand, IResult>
//    {
//        public async Task<IResult> Handle(AccountsUpdateCommand request, CancellationToken cancellationToken)
//        {
//            _logger.LogInformation("{FunctionName} received a request for {@RequestData}", 
//                nameof(AccountsUpdateCommandHandler), request);

//            try
//            {
//                var account = await _accountsRepository.GetByIdAsync(request.Id);
//                if (account == null)
//                {
//                    _logger.LogWarning("{FunctionName} failed. Accounts with ID: {@AccountsId} not found.",
//                        nameof(AccountsUpdateCommandHandler), request.Id);

//                    return Results.NotFound(new 
//                    { 
//                        Message = "Account not found." 
//                    });
//                }

//                account.SubLedgerId = request.SubLedgerId;
//                account.AccountNumber = request.AccountNumber;
//                account.Name = request.Name;
//                account.Address = request.Address;
//                account.ContactInfo = request.ContactInfo;
//                account.Email = request.Email;
//                account.PANNumber = request.PANNumber;
//                account.BackUpContact = request.BackUpContact;
//                account.CreditLimit = request.CreditLimit;
//                account.CreditDays = request.CreditDays;
//                account.CompanyInfoId = request.CompanyInfoId;

//                await _accountsRepository.UpdateAsync(account);
//                await _accountsRepository.SaveChangesAsync();

//                _logger.LogInformation("{FunctionName} successfully updated accounts. Details: {@AccountDetails}",
//                    nameof(AccountsUpdateCommandHandler), JsonConvert.SerializeObject(account));

//                return Results.Ok(new 
//                {
//                    Message = "Account has been updated.", 
//                    Account = request 
//                });
//            }

//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "{FunctionName} failed to update accounts with request: {@RequestData}",
//                    nameof(AccountsUpdateCommandHandler), request);
//                return Results.BadRequest(new 
//                { 
//                    Message = "Failed to update account. Please try again." 
//                });
//            }
//        }
//    }
//}
