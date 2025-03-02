using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Application.Features.Authentication.Commands.UsersDelete;

public class UsersDeleteCommandHandler : IRequestHandler<UsersDeleteCommand, IResult>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<UsersDeleteCommandHandler> _logger;
    public UsersDeleteCommandHandler(UserManager<ApplicationUser> userManager, ILogger<UsersDeleteCommandHandler> logger)
    {
        _userManager = userManager;
        _logger = logger;
    }
    public async Task<IResult> Handle(UsersDeleteCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "{FunctionName} received request: {@RequestData}",
            nameof(UsersDeleteCommandHandler),
            request);

        try
        {
            var existingUser = await _userManager.FindByIdAsync(request.Id.ToString());
            if (existingUser is null)
            {
                return Results.NotFound(new
                {
                    Message = "User Not Found."
                });
            }
            await _userManager.DeleteAsync(existingUser);

            _logger.LogInformation("{FunctionName} successfully deleted account with ID: {@UserID}",
                    nameof(UsersDeleteCommandHandler), existingUser.Id);

            return Results.Ok(new
            {
                Message = "User is deleted successfully."
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "{FunctionName} failed to delete accounts with request: {@RequestData}",
                    nameof(UsersDeleteCommandHandler), request);
            return Results.BadRequest(new
            {
                Message = "Failed to delete account. Please try again."
            });

        }

    }
}

