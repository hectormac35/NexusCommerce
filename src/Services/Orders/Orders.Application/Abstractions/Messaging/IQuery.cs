using MediatR;
using Orders.Application.Common.Results;

namespace Orders.Application.Abstractions.Messaging;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
}
