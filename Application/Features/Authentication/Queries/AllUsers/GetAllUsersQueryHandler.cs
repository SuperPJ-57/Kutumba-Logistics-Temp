using Application.Dto.User;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Features.Authentication.Queries.AllUsers
{
    public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, IResult>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<GetAllUsersQueryHandler> _logger;
        private readonly IMapper _mapper;

        public GetAllUsersQueryHandler(UserManager<ApplicationUser> userManager, ILogger<GetAllUsersQueryHandler> logger, IMapper mapper)
        {
            _userManager = userManager;
            _logger = logger;
            _mapper = mapper;
        }
        public async Task<IResult> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("{FunctionName} received a request to retrieve all users.",
                            nameof(GetAllUsersQueryHandler));
            await Task.CompletedTask;
            try
            {
                var totalCount = _userManager.Users.Count();
                var totalPages = (int)Math.Ceiling((double)totalCount / request.PageSize);
                var userData = await _userManager.Users.AsNoTracking()
                                              .OrderBy(u => u.Id)
                                              .Skip((request.PageNumber - 1) * request.PageSize)
                                              .Take(request.PageSize)
                                              .ToListAsync(cancellationToken);
                var userDto = _mapper.Map<List<UserDto>>(userData);

                if (!userData.Any())
                {
                    return Results.NotFound(new
                    {
                        Message = "Not Found."
                    });
                }
                return Results.Ok(new
                {
                    response = userDto,
                    meta = new
                    {
                        PageNumber = request.PageNumber,
                        PageSize = request.PageSize,
                        TotalCount = totalCount,
                        TotalPages = totalPages
                    }
                });
            
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "{FunctionName} error occurred while retrieving  all the accounts.",
                    nameof(GetAllUsersQueryHandler));
                return Results.BadRequest(new
                {
                    Message = "An error occurred while retrieving users. Please try again later."
                });
            }
        }
    }
}
