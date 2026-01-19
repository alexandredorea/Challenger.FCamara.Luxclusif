using Challenger.FCamara.Luxclusif.Application.Common.Results;
using MediatR;

namespace Challenger.FCamara.Luxclusif.Application.Common.CQS;

public interface IQueryHandler<in TQuery> : IRequestHandler<TQuery, Result>
    where TQuery : IQuery
{
}

public interface IQueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>>
    where TQuery : IQuery<TResponse>
{
}