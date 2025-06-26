using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Virtual.Roulette.Application.Contracts.Services.JackpotServices;
using Virtual.Roulette.Application.Contracts.Services.JackpotServices.Models;
using Virtual.Roulette.Application.Hubs;

namespace Virtual.Roulette.Api.Controllers;

/// <summary>
/// Endpoints for retrieving jackpot information.
/// </summary>
[Route("v{version:apiVersion}/jackpot")]
[Authorize]
public class JackpotController(IJackpotService jackpotService) : ApiControllerBase
{
    /// <summary>
    /// Gets the current jackpot information.
    /// </summary>
    /// <returns>The current jackpot details.</returns>
    /// <response code="200">Returns the current jackpot information.</response>
    [HttpGet]
    [ProducesResponseType(typeof(JackpotModel), StatusCodes.Status200OK)]
    public ActionResult<JackpotModel> Get(CancellationToken cancellationToken )
    {
        return Ok(jackpotService.GetJackpotAmountAsync(cancellationToken));
    }
}