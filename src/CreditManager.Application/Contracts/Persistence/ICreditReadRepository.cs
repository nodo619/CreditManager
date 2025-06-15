using CreditManager.Domain.Entities.Credit;

namespace CreditManager.Application.Contracts.Persistence;

public interface ICreditReadRepository
{
    public Task<CreditRequest?> GetCreditByIdAsync(Guid id, CancellationToken cancellationToken);

    public Task<List<CreditRequest>?> GetCreditsForUserAsync(Guid userId, CancellationToken cancellationToken);

    public Task<List<CreditRequest>> GetCreditsWithSpecificStatusesAsync(int[] includedStatuses,
        CancellationToken cancellationToken);
}