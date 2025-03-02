using Application.Dto.User;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Application.Features.Authentication.Commands.UsersUpdate;

public class UsersUpdateCommandHandler : IRequestHandler<UsersUpdateCommand, IResult>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<UsersUpdateCommandHandler> _logger;
    private readonly IMapper _mapper;
    public UsersUpdateCommandHandler(UserManager<ApplicationUser> userManager, ILogger<UsersUpdateCommandHandler> logger, IMapper mapper)
    {
        _userManager = userManager;
        _logger = logger;
        _mapper = mapper;
    }
    public async Task<IResult> Handle(UsersUpdateCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("{FunctionName} received a request for {@RequestData}",
                nameof(UsersUpdateCommandHandler), request);
        try
        {
            var existingUser = await _userManager.FindByIdAsync(request.Id.ToString());
            if (existingUser is null)
            {
                _logger.LogWarning("{FunctionName} failed. Users with ID: {@AccountsId} not found.",
                        nameof(UsersUpdateCommandHandler), request.Id);

                return Results.NotFound(new
                {
                    Message = "User not Found."
                });
            }

            existingUser.FirstName = request.FirstName;
            existingUser.LastName = request.LastName;
            existingUser.Email = request.Email;

            await _userManager.UpdateAsync(existingUser);
            var updateUser = _mapper.Map<UserDto>(existingUser);
            _logger.LogInformation("{FunctionName} successfully updated accounts. Details: {@AccountDetails}",
                    nameof(UsersUpdateCommandHandler), JsonConvert.SerializeObject(existingUser));
            return Results.Ok(new
            {
                response = updateUser
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "{FunctionName} failed to update users with request: {@RequestData}",
                    nameof(UsersUpdateCommandHandler), request);
            return Results.BadRequest(new
            {
                Message = "Failed to update user. Please try again."
            });
        }
    }
}
