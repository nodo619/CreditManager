using Microsoft.Extensions.Configuration;
using System.Data;


namespace CreditManager.Persistence;

public class DapperContext
{
    private readonly string _connectionString;

    public DapperContext(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("CreditManager")!;
    }

    public IDbConnection CreateConnection()
        => new Npgsql.NpgsqlConnection(_connectionString);
}