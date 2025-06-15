using CreditManager.Application.Common.Models;
using CreditManager.Application.Contracts.Infrastructure;
using CreditManager.Application.Contracts.Persistence;
using CreditManager.Domain.Entities.Credit;
using MediatR;

namespace CreditManager.Application.Feature.CreditRequests.Commands.ApproveCreditRequest;

public class ApproveCreditRequestCommandHandler : IRequestHandler<ApproveCreditRequestCommand, Result<Unit>>
{
    private readonly IAsyncRepository<Guid, CreditRequest> _repository;
    private readonly ICurrentUserService _currentUserService;

    public ApproveCreditRequestCommandHandler(IAsyncRepository<Guid, CreditRequest> repository, ICurrentUserService currentUserService)
    {
        _repository = repository;
        _currentUserService = currentUserService;
    }

    public async Task<Result<Unit>> Handle(ApproveCreditRequestCommand request, CancellationToken cancellationToken)
    {
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

        creditRequest.Status = CreditRequestStatus.Approved;
        creditRequest.ApprovalDate = DateTime.UtcNow;
        creditRequest.ApprovedBy = currentUser.Id;
        creditRequest.LastModifiedBy = currentUser.Id.ToString();

        await _repository.UpdateAsync(creditRequest, cancellationToken);

        return Result<Unit>.Success(Unit.Value);
    }
} 