using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Application.Features.Authentication.Commands.Register;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, IResult>
{
    private readonly ILogger<RegisterCommandHandler> _logger;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public RegisterCommandHandler(UserManager<ApplicationUser> userManager, ILogger<RegisterCommandHandler> logger)
    {
        _userManager = userManager;
        _logger = logger;
    }

    public async Task<IResult> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("{FunctionName} with request: {@RequestData}",
                nameof(RegisterCommandHandler), request);
        try
        {
            
            var userExist = await _userManager.FindByEmailAsync(request.Email);
            if (userExist is not null)
            {
                _logger.LogWarning("{FunctionName} failed to create: {@RequestData}",
                nameof(RegisterCommandHandler), request);

                return Results.BadRequest(new
                {
                    Message = "Email already exists."
                });
            }

            var user = new ApplicationUser
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                UserName = request.Email,
            };

            var response = await _userManager.CreateAsync(user, request.Password);
            _logger.LogInformation("{FunctionName} successfully to create: {@RequestData}",
                nameof(RegisterCommandHandler), request);

            return Results.Ok(new
            {
                Message = "Account has been created,"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,"{FunctionName} failed to add account with request: {@RequestData}",
                         nameof(RegisterCommandHandler), request);
            return Results.BadRequest(new
            {
                Message = "Failed to Register. Please try again."
            });
        }

    }
}

