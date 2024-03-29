using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using MediatR;
using SharedKernel;
using System.Transactions;

namespace Application.Behaviors;

public sealed class UnitOfWorkBehavior<TRequest, TResponse>(IUnitOfWork unitOfWork)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly IUnitOfWork unitOfWork = unitOfWork;

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        using TransactionScope transactionScope = new(TransactionScopeAsyncFlowOption.Enabled);

        TResponse response = await next();

        if (response is Result { IsSuccess: true })
        {
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }

        transactionScope.Complete();

        return response;
    }
}
