using Application.Interfaces.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;


namespace Application.Features.Authentication.Queries.Login;

public class LoginQueryHandler : IRequestHandler<LoginQuery, IResult>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ITokenService _tokenService;
    private readonly ILogger<LoginQueryHandler> _logger;
    public LoginQueryHandler(
        UserManager<ApplicationUser> userManager,
        ITokenService tokenService,
        ILogger<LoginQueryHandler> logger)
    {
        _userManager = userManager;
        _tokenService = tokenService;
        _logger = logger;
    }
    public async Task<IResult> Handle(LoginQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("{FunctionName} with request: {@RequestData}",
                        nameof(LoginQueryHandler), request);

        try
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return Results.BadRequest(new { Message = "Invalid Credentials." });
            }

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, request.Password);
            if (!isPasswordValid)
            {
                return Results.BadRequest(new { Message = "Invalid Credentials." });
            }

            var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

            var accessToken = _tokenService.GenerateToken(claims);
            var refreshToken = _tokenService.GenerateRefreshToken(claims);
            var RefreshToken = refreshToken;
            var RefreshTokenExpiryTime = DateTime.Now.AddDays(7);
            await _userManager.UpdateAsync(user);

            return Results.Ok(new
            {
                Message = "Login successful.",
                UserId = user.Id,
                AccessToken = accessToken,
                RefreshToken = refreshToken
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "{FunctionName} error occurred while logging user account.",
                    nameof(LoginQueryHandler));

            return Results.BadRequest(new
            {
                Message = "An error occurred while logging accounts. Please try again later."
            });
        }

    }
}
