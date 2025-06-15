
using CreditManager.Application.Contracts.Persistence;
using CreditManager.Domain.Entities.Credit;
using Dapper;

namespace CreditManager.Persistence.Repositories;

public class CreditReadRepository : ICreditReadRepository
{
    private readonly DapperContext _context;

    public CreditReadRepository(DapperContext context)
    {
        _context = context;
    }

    public async Task<CreditRequest?> GetCreditByIdAsync(Guid id)
    {
        const string sql = "SELECT \"Id\", \"CustomerId\", \"Amount\", \"CurrencyCode\", \"Status\", \"Comments\", \"ApprovalDate\", \"ApprovedBy\", \"CreditType\", \"Period\", \"RequestDate\" FROM creditmanager.\"CreditRequests\" WHERE \"Id\" = @Id";
        
        using var connection = _context.CreateConnection();

        return await connection.QueryFirstOrDefaultAsync<CreditRequest>(sql, new { Id = id });
    }
}