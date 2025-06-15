using CreditManager.Application.Common.Models;
using CreditManager.Application.Contracts.Infrastructure;
using CreditManager.Application.Contracts.Persistence;
using CreditManager.Domain.Entities.Credit;
using MediatR;

namespace CreditManager.Application.Feature.CreditRequests.Commands.UpdateCreditRequest;

public class UpdateCreditRequestCommandHandler : IRequestHandler<UpdateCreditRequestCommand, Result<Unit>>
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IAsyncRepository<Guid, CreditRequest> _repository;

    public UpdateCreditRequestCommandHandler(ICurrentUserService currentUserService, IAsyncRepository<Guid, CreditRequest> repository)
    {
        _currentUserService = currentUserService;
        _repository = repository;
    }

    public async Task<Result<Unit>> Handle(UpdateCreditRequestCommand request, CancellationToken cancellationToken)
    {
        var currentUser = await _currentUserService.GetCurrentUserAsync(cancellationToken);

        if (currentUser is null)
        {
            return Result<Unit>.Failure("Current user not found");
        }

        var existingRecord = await _repository.GetByIdAsync(request.Id, cancellationToken);

        if (existingRecord is null)
        {
            return Result<Unit>.Failure("Credit request not found");
        }

        bool modified = ApplyUpdatesIfChanged(existingRecord, request);

        if (modified)
        {
            existingRecord.LastModifiedBy = currentUser.Id.ToString();
            await _repository.UpdateAsync(existingRecord, cancellationToken);
        }

        return Result<Unit>.Success(Unit.Value);
    }

    public bool ApplyUpdatesIfChanged(CreditRequest entity, UpdateCreditRequestCommand request)
    {
        bool modified = false;

        if (entity.Amount != request.Amount)
        {
            entity.Amount = request.Amount;
            modified = true;
        }

        if (entity.CurrencyCode != request.CurrencyCode)
        {
            entity.CurrencyCode = request.CurrencyCode;
            modified = true;
        }

        if (entity.Comments != request.Comments)
        {
            entity.Comments = request.Comments;
            modified = true;
        }

        if ((int)entity.CreditType != request.CreditType)
        {
            entity.CreditType = (CreditType)request.CreditType;
            modified = true;
        }

        if (entity.PeriodYears != request.PeriodYears)
        {
            entity.PeriodYears = request.PeriodYears;
            modified = true;
        }

        if (entity.PeriodMonths != request.PeriodMonths)
        {
            entity.PeriodMonths = request.PeriodMonths;
            modified = true;
        }

        if (entity.PeriodDays != request.PeriodDays)
        {
            entity.PeriodDays = request.PeriodDays;
            modified = true;
        }

        return modified;
    }
}