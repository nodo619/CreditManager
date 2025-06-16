using CreditManager.Application.Common.Models;
using CreditManager.Application.Contracts.Infrastructure;
using CreditManager.Application.Contracts.Persistence;
using CreditManager.Domain.Entities.Credit;
using MediatR;

namespace CreditManager.Application.Feature.CreditRequests.Commands.CreateCreditRequest;

public class CreateCreditRequestCommandHandler : IRequestHandler<CreateCreditRequestCommand, Result<Guid>>
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IAsyncRepository<Guid, CreditRequest> _repository;

    public CreateCreditRequestCommandHandler(ICurrentUserService currentUserService, IAsyncRepository<Guid, CreditRequest> repository)
    {
        _currentUserService = currentUserService;
        _repository = repository;
    }

    public async Task<Result<Guid>> Handle(CreateCreditRequestCommand request, CancellationToken cancellationToken)
    {
        var currentUser = await _currentUserService.GetCurrentUserAsync(cancellationToken);

        if (currentUser is null)
        {
            return Result<Guid>.Failure("Current user not found");
        }

        var creditRequest = new CreditRequest
        {
            CustomerId = currentUser.Id,
            Amount = request.Amount,
            CreditType = (CreditType)request.CreditType,
            PeriodYears = request.PeriodYears,
            PeriodMonths = request.PeriodMonths,
            PeriodDays = request.PeriodDays,
            CurrencyCode = request.CurrencyCode,
            Status = CreditRequestStatus.Pending,
            Comments = request.Comments,
            CreatedById = currentUser.Id
        };

        await _repository.AddAsync(creditRequest, cancellationToken);

        return Result<Guid>.Success(creditRequest.Id);
    }
}