using System.Data;

namespace aspnet.webapi.Services;

public interface IDbConnectionFactory
{
    Task<IDbConnection> CreateConnectionAsync();
}
