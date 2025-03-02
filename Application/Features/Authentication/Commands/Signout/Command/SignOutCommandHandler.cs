
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Application.Features.Authentication.Commands.Signout.Command;

public class SignOutCommandHandler : IRequestHandler<SignOutCommand, IResult>
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly ILogger<SignOutCommandHandler> _logger;
    public SignOutCommandHandler(SignInManager<ApplicationUser> signInManager, ILogger<SignOutCommandHandler> logger)
    {
        _signInManager = signInManager;
        _logger = logger;
    }


    public async Task<IResult> Handle(SignOutCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("{FunctionName} with request: {@RequestData}",
                nameof(SignOutCommandHandler), request);
        try
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User successfully signed out.");
            return Results.Ok(new
            {
                Message = "You have been SignOut."
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while signing out the user.");

            return Results.BadRequest(new { Message = "An error occurred while signing out." });
        }
    }
}
