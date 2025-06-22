using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Virtual.Roulette.Application.Contracts.Services.JackpotServices;

namespace Virtual.Roulette.Api.Controllers;

[Route("v{version:apiVersion}/jackpot")]
[Authorize]
public class JackpotController(IJackpotService jackpotService) : ApiControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(jackpotService.GetJackpot());
    }
}