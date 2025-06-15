using CreditManager.Application.Contracts.Infrastructure;
using CreditManager.Application.Contracts.Persistence;
using CreditManager.Application.Messages;
using CreditManager.Domain.Entities.Credit;
using MassTransit;
using MediatR;

namespace CreditManager.Application.Feature.CreditRequests.Commands.UpdateCreditRequest;

public class UpdateCreditRequestCommandHandler : IRequestHandler<UpdateCreditRequestCommand, Guid>
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IAsyncRepository<Guid, CreditRequest> _repository;

    public UpdateCreditRequestCommandHandler(ICurrentUserService currentUserService, IAsyncRepository<Guid, CreditRequest> repository)
    {
        _currentUserService = currentUserService;
        _repository = repository;
    }

    public async Task<Guid> Handle(UpdateCreditRequestCommand request, CancellationToken cancellationToken)
    {
        var currentUser = await _currentUserService.GetCurrentUserAsync(cancellationToken);

        if (currentUser is null)
        {
            throw new ArgumentNullException(nameof(currentUser));
        }

        var existingRecord = await _repository.GetByIdAsync(request.Id, cancellationToken);

        if (existingRecord is null)
        {
            throw new KeyNotFoundException();
        }

        bool modified = ApplyUpdatesIfChanged(existingRecord, request);

        if (modified)
        {
            await _repository.UpdateAsync(existingRecord, cancellationToken);
        }

        return request.Id;
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