using Challenger.FCamara.Luxclusif.Application.Common.Results;
using MediatR;

namespace Challenger.FCamara.Luxclusif.Application.Common.CQS;

public interface IQuery : IRequest<Result>
{
}

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
}