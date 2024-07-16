using System.Data;
using Npgsql;

namespace aspnet.webapi.Services;

public class NpgsqlConnectionFactory(string connectionString) : IDbConnectionFactory
{
    private readonly string _connectionString = connectionString;

    public async Task<IDbConnection> CreateConnectionAsync()
    {
        NpgsqlConnection conn = new(_connectionString);
        await conn.OpenAsync();
        return conn;
    }
}
