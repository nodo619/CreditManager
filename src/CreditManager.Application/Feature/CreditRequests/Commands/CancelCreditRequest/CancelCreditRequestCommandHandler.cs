using CreditManager.Application.Common.Models;
using CreditManager.Application.Contracts.Infrastructure;
using CreditManager.Application.Contracts.Persistence;
using CreditManager.Domain.Entities.Credit;
using MediatR;

namespace CreditManager.Application.Feature.CreditRequests.Commands.CancelCreditRequest;

public class CancelCreditRequestCommandHandler : IRequestHandler<CancelCreditRequestCommand, Result<Unit>>
{
    private readonly IAsyncRepository<Guid, CreditRequest> _repository;
    private readonly ICurrentUserService _currentUserService;

    public CancelCreditRequestCommandHandler(IAsyncRepository<Guid, CreditRequest> repository, ICurrentUserService currentUserService)
    {
        _repository = repository;
        _currentUserService = currentUserService;
    }

    public async Task<Result<Unit>> Handle(CancelCreditRequestCommand request, CancellationToken cancellationToken)
    {
        var s = 0;
        var m = 1 / s;

        var creditRequest = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (creditRequest == null)
        {
            return Result<Unit>.Failure("Credit request not found");
        }

        var currentUser = await _currentUserService.GetCurrentUserAsync(cancellationToken);

        if (currentUser is null)
        {
            return Result<Unit>.Failure("User not found");
        }

        creditRequest.Status = CreditRequestStatus.Cancelled;
        creditRequest.LastModifiedBy = currentUser.Id.ToString();

        await _repository.UpdateAsync(creditRequest, cancellationToken);

        return Result<Unit>.Success(Unit.Value);
    }
} 