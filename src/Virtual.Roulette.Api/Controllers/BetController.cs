using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Virtual.Roulette.Application.Contracts.Services.BetServices;
using Virtual.Roulette.Application.Contracts.Services.BetServices.Models;

namespace Virtual.Roulette.Api.Controllers;

[Route("v{version:apiVersion}/bet")]
[Authorize]
public class BetController(IBetService betService, IHttpContextAccessor httpContextAccessor)
    : ApiControllerBase
{
    [HttpPost]
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