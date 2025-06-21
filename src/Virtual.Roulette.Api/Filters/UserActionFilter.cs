using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Filters;
using Virtual.Roulette.Api.Controllers;
using Virtual.Roulette.Api.Models;

namespace Virtual.Roulette.Api.Filters;

public class UserActionFilter : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (context.Controller is ApiControllerBase c)
        {
            _ = int.TryParse(c.User?.FindFirstValue(ClaimTypes.NameIdentifier), out var userId);
            var userModel = new UserModel
            {
                UserId = userId
            };

            c.UserModel = userModel;
        }

        base.OnActionExecuting(context);
    }
}