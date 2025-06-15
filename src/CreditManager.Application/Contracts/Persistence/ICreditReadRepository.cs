using CreditManager.Domain.Entities.Credit;

namespace CreditManager.Application.Contracts.Persistence;

public interface ICreditReadRepository
{
    Task<CreditRequest?> GetCreditByIdAsync(Guid id);
}