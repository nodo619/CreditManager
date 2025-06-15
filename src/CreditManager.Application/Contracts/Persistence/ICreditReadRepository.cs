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

    public Task<PaginatedList<CreditRequest>> GetCreditsWithSpecificStatusesAsync(
        int[] includedStatuses,
        IQueryObject queryObject,
        CancellationToken cancellationToken);
}