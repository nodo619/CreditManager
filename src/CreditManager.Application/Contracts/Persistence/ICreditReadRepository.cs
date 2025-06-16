using CreditManager.Application.Feature.CreditRequests.Queries.GetCreditRequests;
using CreditManager.Application.Pagination;
using CreditManager.Domain.Entities.Credit;

namespace CreditManager.Application.Contracts.Persistence;

public interface ICreditReadRepository
{
    public Task<CreditRequest?> GetCreditByIdAsync(Guid id, CancellationToken cancellationToken);

    public Task<PaginatedList<CreditRequest>> GetCreditsForUserAsync(
        Guid userId, 
        IQueryObject queryObject,
        CancellationToken cancellationToken);

    public Task<PaginatedList<CreditRequestWithUserModel>> GetCreditsWithSpecificStatusesAsync(
        int[] includedStatuses,
        IQueryObject queryObject,
        CancellationToken cancellationToken);
}