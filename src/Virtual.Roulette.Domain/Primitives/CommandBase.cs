using MediatR;

namespace Virtual.Roulette.Domain.Primitives;

public class CommandBase<TResponse> : CommandBaseValidator, IRequest<TResponse>
{
}

public class CommandBase : CommandBaseValidator, IRequest
{
}