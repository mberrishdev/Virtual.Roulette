using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Virtual.Roulette.Application.Contracts.Services.SpinServices;

namespace Virtual.Roulette.Api.Controllers;

[Route("v{version:apiVersion}/spin")]
[Authorize]
public class SpinController(ISpinService spinService) : ApiControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetUserSpins(CancellationToken cancellationToken)
    {
        var spins = await spinService.GetSpinsByUserAsync(UserModel!.UserId, cancellationToken);
        return Ok(spins);
    }
}