using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Virtual.Roulette.Application.Contracts.Services.SpinServices;
using Virtual.Roulette.Domain.Entities.Spins;

namespace Virtual.Roulette.Api.Controllers;

/// <summary>
/// Endpoints for retrieving spin history.
/// </summary>
[Route("v{version:apiVersion}/spin")]
[Authorize]
public class SpinController(ISpinService spinService) : ApiControllerBase
{
    /// <summary>
    /// Gets the spin history for the authenticated user.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A list of the user's past spins.</returns>
    /// <response code="200">Returns a list of the user's spins.</response>
    /// <response code="401">If the user is not authenticated.</response>
    [HttpGet]
    [ProducesResponseType(typeof(List<Spin>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetUserSpins(CancellationToken cancellationToken)
    {
        var spins = await spinService.GetSpinsByUserAsync(UserModel!.UserId, cancellationToken);
        return Ok(spins);
    }
}