using CreditManager.Application.Common.Models;
using CreditManager.Application.Contracts.Persistence;
using CreditManager.Application.Pagination;
using CreditManager.Domain.Entities.Credit;
using MediatR;

namespace CreditManager.Application.Feature.CreditRequests.Queries.GetCreditRequests;

public class GetCreditRequestsQueryHandler : IRequestHandler<GetCreditRequestsQuery, Result<PaginatedList<CreditRequestDto>>>
{
    private readonly ICreditReadRepository _repository;

    public GetCreditRequestsQueryHandler(ICreditReadRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<PaginatedList<CreditRequestDto>>> Handle(GetCreditRequestsQuery request, CancellationToken cancellationToken)
    {
        var neededStatuses = new[]
        {
            (int)CreditRequestStatus.Sent,
            (int)CreditRequestStatus.Approved,
            (int)CreditRequestStatus.Rejected,
            (int)CreditRequestStatus.Cancelled
        };

        var creditRequests = await _repository.GetCreditsWithSpecificStatusesAsync(neededStatuses, request, cancellationToken);

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