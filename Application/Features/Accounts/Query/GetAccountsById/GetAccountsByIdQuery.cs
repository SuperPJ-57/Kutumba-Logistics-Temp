//using Application.Dto.Account;
//using Application.Interfaces.Repositories;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Logging;
//using Newtonsoft.Json;

//namespace Application.Features.Accounts.Query.GetAccountsById
//{
//    public record GetAccountsByIdQuery(int Id) : IRequest<IResult>;

//    public class GetAccountsByIdQueryHandler(IAccountsRepository _accountsRepository,
//        ILogger<GetAccountsByIdQueryHandler> _logger
//        ) : IRequestHandler<GetAccountsByIdQuery, IResult>
//    {
//        public async Task<IResult> Handle(GetAccountsByIdQuery request, CancellationToken cancellationToken)
//        {
//            _logger.LogInformation("{FunctionName} received a request to retrieve accounts. Id: {@RequestId}", 
//                nameof(GetAccountsByIdQueryHandler), request);

//            try
//            {

//                var account = await _accountsRepository
//                    .Queryable
//                    .Where(a => a.Id == request.Id)
//                    .Select(a => new AccountDto
//                    {
//                        Id = a.Id,
//                        SubLedgerId = a.SubLedgerId,
//                        AccountNumber = a.AccountNumber,
//                        Name = a.Name,
//                        Address = a.Address,
//                        ContactInfo = a.ContactInfo,
//                        Email = a.Email,
//                        PANNumber = a.PANNumber,
//                        BackUpContact = a.BackUpContact,
//                        CreditLimit = a.CreditLimit,
//                        CreditDays = a.CreditDays,
//                        CompanyInfoId = a.CompanyInfoId,
//                    })
//                    .FirstOrDefaultAsync(cancellationToken);

//                if (account == null)
//                {
//                    _logger.LogWarning("{FunctionName} failed. No accounts found with Id: {@RequestId}",
//                        nameof(GetAccountsByIdQueryHandler), request);

//                    return Results.NotFound(new 
//                    { 
//                        Message = "Account not found."
//                    });
//                }

//                _logger.LogInformation("{FunctionName} successfully retrieved accounts. Details: {@AccountDetails}",
//                        nameof(GetAccountsByIdQueryHandler), JsonConvert.SerializeObject(account));

//                return Results.Ok(new 
//                { 
//                    Account = account
//                });
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "{FunctionName} error occurred while retrieving accounts. Id: {@RequestId}",
//                    nameof(GetAccountsByIdQueryHandler), request);

//                return Results.BadRequest(new
//                {
//                    Message = "An error occurred while retrieving accounts. Please try again later."
//                });
//            }
//        }
//    }
//}
