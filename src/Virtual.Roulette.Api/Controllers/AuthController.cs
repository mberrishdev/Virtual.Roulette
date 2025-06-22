using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;
using Virtual.Roulette.Api.Swagger.Examples;
using Virtual.Roulette.Application.Contracts.Services.AuthService.Models;
using Virtual.Roulette.Application.Contracts.Services.AuthServices;
using Virtual.Roulette.Application.Models;
using Virtual.Roulette.Application.Contracts.Services.AuthServices.Models;

namespace Virtual.Roulette.Api.Controllers;

/// <summary>
/// Authentication.
/// </summary>
[ApiVersion("1.0")]
[Route("v{version:apiVersion}/auth")]
public class AuthController(IAuthService authService) : ApiControllerBase
{
    /// <summary>
    /// Login and receive a JWT token.
    /// </summary>
    /// <param name="request">Login credentials.</param>
    /// <param name="cancellationToken"></param>
    /// <returns>JWT token and user info.</returns>
    /// <response code="200">Returns the JWT token and user info.</response>
    /// <response code="401">If credentials are invalid.</response>
    [HttpPost("login")]
    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [SwaggerRequestExample(typeof(AuthRequest), typeof(AuthRequestExample))]
    public async Task<ActionResult<AuthResponse>> Login([FromBody] AuthRequest request,
        CancellationToken cancellationToken)
    {
        var loginResponse = await authService.LoginAsync(request, cancellationToken);

        return Ok(loginResponse);
    }

    /// <summary>
    /// Refreshes the access token using a valid refresh token.
    /// </summary>
    /// <param name="request">The request containing the refresh token.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>Returns a new <see cref="AuthResponse"/> with updated tokens if the refresh token is valid and not expired.</returns>
    /// <response code="200">Returns the new access and refresh tokens.</response>
    [HttpPost("refresh")]
    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<AuthResponse>> Refresh([FromBody] RefreshRequest request,
        CancellationToken cancellationToken)
    {
        var response = await authService.RefreshAsync(request.RefreshToken, cancellationToken);
        return Ok(response);
    }

    /// <summary>
    /// Register a new user.
    /// </summary>
    /// <param name="request">Registration data.</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Success or error.</returns>
    /// <response code="200">User registered successfully.</response>
    /// <response code="409">If username already exists.</response>
    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [SwaggerRequestExample(typeof(RegisterRequest), typeof(Api.Swagger.Examples.RegisterRequestExample))]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request, CancellationToken cancellationToken)
    {
        await authService.RegisterAsync(request, cancellationToken);
        return Ok();
    }
}