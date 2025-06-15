using CreditManager.Application.Contracts.Infrastructure;
using CreditManager.Application.Contracts.Persistence;
using CreditManager.Domain.Entities.Credit;
using MediatR;

namespace CreditManager.Application.Feature.CreditRequests.Queries.GetCreditRequestsForCustomer;

public class GetCreditRequestsForCustomerQueryHandler : IRequestHandler<GetCreditRequestsForCustomerQuery, IEnumerable<CreditRequestDto>>
{
    private readonly ICreditReadRepository _repository;
    private readonly ICurrentUserService _currentUserService;

    public GetCreditRequestsForCustomerQueryHandler(ICreditReadRepository repository, ICurrentUserService currentUserService)
    {
        _repository = repository;
        _currentUserService = currentUserService;
    }

    public async Task<IEnumerable<CreditRequestDto>> Handle(GetCreditRequestsForCustomerQuery request, CancellationToken cancellationToken)
    {
        var currentUser = await _currentUserService.GetCurrentUserAsync(cancellationToken);

        if (currentUser is null)
        {
            throw new ArgumentNullException(nameof(currentUser));
        }

        var creditRequests = await _repository.GetCreditsForUserAsync(currentUser.Id, cancellationToken);

        var dtoList = creditRequests?.Select(c => new CreditRequestDto
        {
            Id = c.Id,
            CustomerId = c.CustomerId,
            Amount = c.Amount,
            CurrencyCode = c.CurrencyCode,
            RequestDate = c.RequestDate,
            PeriodYears = c.PeriodYears,
            PeriodMonths = c.PeriodMonths,
            PeriodDays = c.PeriodDays,
            CreditType = c.CreditType,
            Status = c.Status,
            Comments = c.Comments,
            ApprovalDate = c.ApprovalDate,
            ApprovedBy = c.ApprovedBy
        });

        return dtoList ?? new List<CreditRequestDto>();
    }
} 