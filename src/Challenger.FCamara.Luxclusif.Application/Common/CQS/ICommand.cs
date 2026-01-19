using Challenger.FCamara.Luxclusif.Application.Common.Results;
using MediatR;

namespace Challenger.FCamara.Luxclusif.Application.Common.CQS;

public interface ICommand : IRequest<Result>
{
}

public interface ICommand<TResponse> : IRequest<Result<TResponse>>
{
}