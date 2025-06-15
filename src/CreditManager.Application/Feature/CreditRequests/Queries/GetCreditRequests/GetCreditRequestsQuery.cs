using CreditManager.Application.Common.Models;
using CreditManager.Application.Pagination;
using MediatR;

namespace CreditManager.Application.Feature.CreditRequests.Queries.GetCreditRequests;

public record GetCreditRequestsQuery : IRequest<Result<PaginatedList<CreditRequestDto>>>, IQueryObject
{
    public string? SortBy { get; set; }
    public string? SortDirection { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
} 