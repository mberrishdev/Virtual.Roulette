using Microsoft.AspNetCore.Mvc;
using Virtual.Roulette.Application.Contracts.Services.AuthService.Models;
using Virtual.Roulette.Application.Contracts.Services.AuthServices;
using Virtual.Roulette.Application.Models;
using Virtual.Roulette.Application.Contracts.Services.AuthServices.Models;

namespace Virtual.Roulette.Api.Controllers;

[ApiVersion("1.0")]
[Route("v{version:apiVersion}/auth")]
public class AuthController(IAuthService authService) : ApiControllerBase
{
    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> Login([FromBody] AuthRequest request,
        CancellationToken cancellationToken)
    {
        var loginResponse = await authService.LoginAsync(request, cancellationToken);

        return Ok(loginResponse);
    }

    [HttpPost("refresh")]
    public async Task<ActionResult<LoginResponse>> Refresh([FromBody] RefreshRequest request,
        CancellationToken cancellationToken)
    {
        var response = await authService.RefreshAsync(request.RefreshToken, cancellationToken);
        return Ok(response);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request, CancellationToken cancellationToken)
    {
        await authService.RegisterAsync(request, cancellationToken);
        return Ok();
    }
}