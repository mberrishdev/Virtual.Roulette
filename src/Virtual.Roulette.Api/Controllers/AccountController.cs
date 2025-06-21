using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Virtual.Roulette.Application.Contracts.Services.AccountServices;

namespace Virtual.Roulette.Api.Controllers;

[Route("v{version:apiVersion}/account")]
[Authorize]
public class AccountController(IAccountService accountService) : ApiControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetBalance(CancellationToken cancellationToken)
    {
        return Ok(await accountService.GetAccount(UserModel!.UserId, cancellationToken));
    }
}