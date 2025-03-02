using Application.Dto.User;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;


namespace Application.Features.Authentication.Queries.UsersById;

public class GetUserByIdQueryHandler 
    :IRequestHandler<GetUsersByIdQuery, IResult>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<GetUserByIdQueryHandler> _logger;
    private readonly IMapper _mapper;
    public GetUserByIdQueryHandler(
        UserManager<ApplicationUser> userManager,
        ILogger<GetUserByIdQueryHandler> logger,
        IMapper mapper)
    {
        _userManager = userManager;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<IResult> Handle(GetUsersByIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("{FunctionName} received a request to retrieve users. Id: {@RequestId}",
                nameof(GetUserByIdQueryHandler), request);
        try
        {
            var user = await _userManager.FindByIdAsync(request.Id.ToString());
            if (user is null)
            {
                //need logger
                return Results.NotFound(new
                {
                    Message = "User not found."
                });
            }
            var userDto = _mapper.Map<UserDto>(user);
            return Results.Ok(new
            {
                Message = "User details.",
                userdata = userDto
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "{FunctionName} error occurred while retrieving users. Id: {@RequestId}",
                    nameof(GetUserByIdQueryHandler), request);
            return Results.BadRequest(new
            {
                Message = "An error occurred while retrieving users. Please try again later."
            });
        }
    }
}
