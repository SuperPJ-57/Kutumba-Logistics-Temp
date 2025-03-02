//using Application.Dto.Account;
//using Application.Interfaces.Repositories;
//using Microsoft.Extensions.Logging;
//using Newtonsoft.Json;

//namespace Application.Features.Accounts.Query.GetAllAccounts
//{
//    public record GetAllAccountsQuery(int PageNumber, int PageSize) : IRequest<IResult>;

//    public class GetAllAccountsQueryHandler(IAccountsRepository _accountsRepository,
//        ILogger<GetAllAccountsQueryHandler> _logger
//        ) : IRequestHandler<GetAllAccountsQuery, IResult>
//    {
//        public async Task<IResult> Handle(GetAllAccountsQuery request, CancellationToken cancellationToken)
//        {
//            _logger.LogInformation("{FunctionName} received a request to retrieve all accounts.",
//                nameof(GetAllAccountsQueryHandler));

//            try
//            {

//                var totalCount = _accountsRepository.Queryable.Count();

//                var totalPages = (int)Math.Ceiling((double)totalCount / request.PageSize);

//                var accounts = await _accountsRepository
//                    .GetAllAsNoTrackingAsync(request.PageNumber, request.PageSize);

//                var accountDtos = accounts.Select(a => new AccountDto
//                {
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
//                }).ToList();

//                _logger.LogInformation("{FunctionName} successfully retrieved all accounts. Count: {@Count}. Result: {@AccountsDetails}",
//                    nameof(GetAllAccountsQueryHandler), totalCount, JsonConvert.SerializeObject(accountDtos));

//                return Results.Ok(new
//                {
//                    Accounts = accountDtos,
//                    meta = new
//                    {
//                        PageNumber = request.PageNumber,
//                        PageSize = request.PageSize,
//                        TotalCount = totalCount,
//                        TotalPages = totalPages
//                    }
//                });
//            }

//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "{FunctionName} error occurred while retrieving  all the accounts.", 
//                    nameof(GetAllAccountsQueryHandler));

//                return Results.BadRequest(new
//                {
//                    Message = "An error occurred while retrieving accounts. Please try again later."
//                });         
//            }     
//        }
//    }
//}
