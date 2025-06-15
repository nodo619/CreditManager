using CreditManager.Application.Common.Models;
using CreditManager.Application.Pagination;
using MediatR;

namespace CreditManager.Application.Feature.CreditRequests.Queries.GetCreditRequestsForCustomer;

public record GetCreditRequestsForCustomerQuery : IQueryObject, IRequest<Result<PaginatedList<CreditRequestDto>>>
{
    public string? SortBy { get; set; }
    public string? SortDirection { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}