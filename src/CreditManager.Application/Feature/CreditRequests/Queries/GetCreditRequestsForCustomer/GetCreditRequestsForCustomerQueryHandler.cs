using CreditManager.Application.Common.Models;
using CreditManager.Application.Contracts.Infrastructure;
using CreditManager.Application.Contracts.Persistence;
using CreditManager.Application.Pagination;
using CreditManager.Domain.Entities.Credit;
using MediatR;

namespace CreditManager.Application.Feature.CreditRequests.Queries.GetCreditRequestsForCustomer;

public class GetCreditRequestsForCustomerQueryHandler : IRequestHandler<GetCreditRequestsForCustomerQuery, Result<PaginatedList<CreditRequestDto>>>
{
    private readonly ICreditReadRepository _repository;
    private readonly ICurrentUserService _currentUserService;

    public GetCreditRequestsForCustomerQueryHandler(ICreditReadRepository repository, ICurrentUserService currentUserService)
    {
        _repository = repository;
        _currentUserService = currentUserService;
    }

    public async Task<Result<PaginatedList<CreditRequestDto>>> Handle(GetCreditRequestsForCustomerQuery request, CancellationToken cancellationToken)
    {
        var currentUser = await _currentUserService.GetCurrentUserAsync(cancellationToken);

        if (currentUser is null)
        {
            return Result<PaginatedList<CreditRequestDto>>.Failure("Current user not found");
        }

        var creditRequests = await _repository.GetCreditsForUserAsync(currentUser.Id, request, cancellationToken);

        var dtoList = creditRequests.Items.Select(c => new CreditRequestDto
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
        }).ToList();

        return Result<PaginatedList<CreditRequestDto>>.Success(
            new PaginatedList<CreditRequestDto>(dtoList, creditRequests.TotalCount));
    }
} 