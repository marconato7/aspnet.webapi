using System.Data;
using Dapper;

namespace aspnet.webapi.Services;

public class ProductsRepository(IDbConnectionFactory dbConnectionFactory)
{
    private readonly IDbConnectionFactory _dbConnectionFactory = dbConnectionFactory;

    public async Task CreateAsync(Product product)
    {
        using IDbConnection connection = await _dbConnectionFactory.CreateConnectionAsync();

        string query = @"
            INSERT INTO products (id, name, category, sub_category)
            VALUES (@Id, @Name, @Category, @SubCategory);";

        var numberOfAffectedRows = await connection.ExecuteAsync(query, product);
        if (numberOfAffectedRows <= 0)
        {
            throw new Exception();
        }
    }

    public async Task<Product> GetByIdAsync(Guid productId)
    {
        using IDbConnection connection = await _dbConnectionFactory.CreateConnectionAsync();

        string query = @"
            SELECT id, name, category, sub_category AS SubCategory
            FROM products
            WHERE id = @Id;";

        var product = await connection.QueryFirstOrDefaultAsync<Product>(query, new { productId }) ?? throw new Exception();

        return product;
    }
}
