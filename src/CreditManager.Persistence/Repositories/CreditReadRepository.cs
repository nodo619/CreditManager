using CreditManager.Application.Contracts.Persistence;
using CreditManager.Application.Pagination;
using CreditManager.Domain.Entities.Credit;
using CreditManager.Domain.Entities.Identity;
using Dapper;
using System.Threading;

namespace CreditManager.Persistence.Repositories;

public class CreditReadRepository : ICreditReadRepository
{
    private readonly DapperContext _context;

    private const string Columns = """
                                   "Id", "CustomerId", "Amount", "CurrencyCode", "Status",
                                   "Comments", "ApprovalDate", "ApprovedBy", "CreditType",
                                   "PeriodYears", "PeriodMonths", "PeriodDays", "RequestDate"
                                   """;

    private static readonly HashSet<string> AllowedSortColumns = new(StringComparer.OrdinalIgnoreCase)
    {
        "Id", "CustomerId", "Amount", "CurrencyCode", "Status",
        "ApprovalDate", "CreditType", "RequestDate"
    };

    public CreditReadRepository(DapperContext context)
    {
        _context = context;
    }

    public async Task<CreditRequest?> GetCreditByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var sql = $"""
                  SELECT {Columns}
                  FROM creditmanager."CreditRequests"
                  WHERE "Id" = @Id;
                  """;

        using var connection = _context.CreateConnection();

        var command = new CommandDefinition(sql, new { Id = id }, cancellationToken: cancellationToken);
        return await connection.QueryFirstOrDefaultAsync<CreditRequest>(command);
    }

    public async Task<PaginatedList<CreditRequest>> GetCreditsForUserAsync(
        Guid userId,
        IQueryObject queryObject,
        CancellationToken cancellationToken)
    {
        var baseSql = $"""
                       SELECT {Columns}
                       FROM creditmanager."CreditRequests"
                       WHERE "CustomerId" = @CustomerId
                       """;

        using var connection = _context.CreateConnection();

        // Get total count
        var countSql = $"SELECT COUNT(*) FROM ({baseSql}) AS count_query";
        var countCommand = new CommandDefinition(countSql, new { CustomerId = userId }, cancellationToken: cancellationToken);
        var totalCount = await connection.ExecuteScalarAsync<int>(countCommand);

        // Build final SQL
        var finalSql = baseSql;

        if (!string.IsNullOrEmpty(queryObject.SortBy) &&
            AllowedSortColumns.Contains(queryObject.SortBy) &&
            (queryObject.SortDirection?.ToLower() is "asc" or "desc"))
        {
            finalSql += $""" ORDER BY "{queryObject.SortBy}" {queryObject.SortDirection!.ToUpper()} """;
        }

        finalSql += " LIMIT @Limit OFFSET @Offset";

        var offset = (queryObject.PageNumber - 1) * queryObject.PageSize;
        var command = new CommandDefinition(finalSql, new { CustomerId = userId, Limit = queryObject.PageSize, Offset = offset }, cancellationToken: cancellationToken);
        var items = await connection.QueryAsync<CreditRequest>(command);

        return new PaginatedList<CreditRequest>(items.ToList(), totalCount);
    }

    public async Task<PaginatedList<CreditRequest>> GetCreditsWithSpecificStatusesAsync(
        int[] includedStatuses,
        IQueryObject queryObject,
        CancellationToken cancellationToken)
    {
        var baseSql = $"""
                        SELECT {Columns}
                        FROM creditmanager."CreditRequests"
                        WHERE "Status" = ANY(@IncludedStatuses)
                        """;

        using var connection = _context.CreateConnection();

        // Get total count
        var countSql = $"SELECT COUNT(*) FROM ({baseSql}) AS count_query";
        var countCommand = new CommandDefinition(countSql, new { IncludedStatuses = includedStatuses }, cancellationToken: cancellationToken);
        var totalCount = await connection.ExecuteScalarAsync<int>(countCommand);

        // Build final SQL
        var finalSql = baseSql;

        if (!string.IsNullOrEmpty(queryObject.SortBy) &&
            AllowedSortColumns.Contains(queryObject.SortBy) &&
            (queryObject.SortDirection?.ToLower() is "asc" or "desc"))
        {
            finalSql += $""" ORDER BY "{queryObject.SortBy}" {queryObject.SortDirection!.ToUpper()} """;
        }

        finalSql += " LIMIT @Limit OFFSET @Offset";

        var offset = (queryObject.PageNumber - 1) * queryObject.PageSize;
        var command = new CommandDefinition(finalSql, new { IncludedStatuses = includedStatuses, Limit = queryObject.PageSize, Offset = offset }, cancellationToken: cancellationToken);
        var items = await connection.QueryAsync<CreditRequest>(command);

        return new PaginatedList<CreditRequest>(items.ToList(), totalCount);
    }
}