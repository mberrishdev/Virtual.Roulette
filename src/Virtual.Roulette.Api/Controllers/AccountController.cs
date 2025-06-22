using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Virtual.Roulette.Application.Contracts.Services.AccountServices;
using Virtual.Roulette.Application.Contracts.Services.AccountServices.Models;

namespace Virtual.Roulette.Api.Controllers;

/// <summary>
/// User accounts.
/// </summary>
[Route("v{version:apiVersion}/account")]
[Authorize]
public class AccountController(IAccountService accountService) : ApiControllerBase
{
    /// <summary>
    /// Gets the account details for the authenticated user.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The user's account details.</returns>
    /// <response code="200">Returns the user's account information.</response>
    /// <response code="404">If the user's account is not found.</response>
    [HttpGet]
    [ProducesResponseType(typeof(AccountModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AccountModel>> GetBalance(CancellationToken cancellationToken)
    {
        return Ok(await accountService.GetAccount(UserModel!.UserId, cancellationToken));
    }
}