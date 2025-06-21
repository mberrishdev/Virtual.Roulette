using Microsoft.AspNetCore.Mvc;
using MediatR;
using Virtual.Roulette.Api.Filters;
using Virtual.Roulette.Api.Models;

namespace Virtual.Roulette.Api.Controllers;

/// <summary>
/// Base Controller
/// </summary>
[ApiController]
[Produces("application/json")]
[UserActionFilter]
public class ApiControllerBase : ControllerBase
{
    /// <summary>
    /// IMediator
    /// </summary>
    protected readonly IMediator? Mediator;

    /// <summary>
    /// UserModel
    /// </summary>
    public UserModel? UserModel { get; set; } = null;
    
    /// <summary>
    /// ApiControllerBase Constructor
    /// </summary>
    /// <param name="mediator"></param>
    public ApiControllerBase(IMediator mediator)
    {
        Mediator = mediator;
    }

    public ApiControllerBase()
    {
    }
}