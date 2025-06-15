
using CreditManager.Application.Contracts.Persistence;
using CreditManager.Domain.Entities.Credit;
using CreditManager.Domain.Entities.Identity;
using Dapper;
using System.Threading;

namespace CreditManager.Persistence.Repositories;

public class CreditReadRepository : ICreditReadRepository
{
    private readonly DapperContext _context;

    public CreditReadRepository(DapperContext context)
    {
        _context = context;
    }

    public async Task<CreditRequest?> GetCreditByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var sql = """
                  SELECT "Id", "CustomerId", "Amount", "CurrencyCode", "Status",
                  "Comments", "ApprovalDate", "ApprovedBy", "CreditType", "PeriodYears", "PeriodMonths", "PeriodDays", "RequestDate"
                  FROM creditmanager."CreditRequests"
                  WHERE "Id" = @Id;
                  """;

        using var connection = _context.CreateConnection();

        var command = new CommandDefinition(sql, new {Id = id},
            cancellationToken: cancellationToken);

        return await connection.QueryFirstOrDefaultAsync<CreditRequest>(command);
    }

    public async Task<List<CreditRequest>> GetCreditsForUserAsync(Guid userId, CancellationToken cancellationToken)
    {
        var sql = """
                  SELECT "Id", "CustomerId", "Amount", "CurrencyCode", "Status",
                         "Comments", "ApprovalDate", "ApprovedBy", "CreditType",
                         "PeriodYears", "PeriodMonths", "PeriodDays", "RequestDate"
                  FROM creditmanager."CreditRequests"
                  WHERE "CustomerId" = @CustomerId;
                  """;
        using var connection = _context.CreateConnection();

        var command = new CommandDefinition(sql, new{ CustomerId = userId },
            cancellationToken: cancellationToken);

        var items = await connection.QueryAsync<CreditRequest>(command);
        return items.ToList();
    }

    public async Task<List<CreditRequest>> GetCreditsWithSpecificStatusesAsync(int[] includedStatuses, CancellationToken cancellationToken)
    {
        var sql = """
                  SELECT "Id", "CustomerId", "Amount", "CurrencyCode", "Status",
                         "Comments", "ApprovalDate", "ApprovedBy", "CreditType",
                         "PeriodYears", "PeriodMonths", "PeriodDays", "RequestDate"
                  FROM creditmanager."CreditRequests"
                  WHERE "Status" = ANY(@IncludedStatuses);
                  """;

        using var connection = _context.CreateConnection();

        var command = new CommandDefinition(sql, new { IncludedStatuses = includedStatuses },
            cancellationToken: cancellationToken);

        var items = await connection.QueryAsync<CreditRequest>(command);
        return items.ToList();

    }
}