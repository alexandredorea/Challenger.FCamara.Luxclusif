using Challenger.FCamara.Luxclusif.Application.Common.Results;
using MediatR;

namespace Challenger.FCamara.Luxclusif.Application.Common.CQS;

public interface ICommandHandler<in TCommand> : IRequestHandler<TCommand, Result>
where TCommand : ICommand
{
}

public interface ICommandHandler<in TCommand, TResponse> : IRequestHandler<TCommand, Result<TResponse>>
    where TCommand : ICommand<TResponse>
{
}