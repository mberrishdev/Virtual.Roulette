using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;
using Virtual.Roulette.Api.Swagger.Examples;
using Virtual.Roulette.Application.Contracts.Services.BetServices;
using Virtual.Roulette.Application.Contracts.Services.BetServices.Models;

namespace Virtual.Roulette.Api.Controllers;

/// <summary>
/// Endpoints for placing bets.
/// </summary>
[Route("v{version:apiVersion}/bet")]
[Authorize]
public class BetController(IBetService betService, IHttpContextAccessor httpContextAccessor)
    : ApiControllerBase
{
    /// <summary>
    /// Places a bet for the authenticated user.
    /// </summary>
    /// <param name="request">The bet request details.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The result of the bet, including the winning number and updated balance.</returns>
    /// <response code="200">Returns the result of the bet.</response>
    /// <response code="400">If the request is invalid.</response>
    /// <response code="401">If the user is not authenticated.</response>
    [HttpPost]
    [ProducesResponseType(typeof(BetResultResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [SwaggerRequestExample(typeof(BetRequest), typeof(BetRequestExample))]
    public async Task<ActionResult<BetResultResponse>> PlaceBet([FromBody] BetRequest request,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var ip = httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString() ?? "unknown";

        var result = await betService.PlaceBetAsync(UserModel!.UserId, request.BetJson, ip, cancellationToken);

        return Ok(result);
    }
}