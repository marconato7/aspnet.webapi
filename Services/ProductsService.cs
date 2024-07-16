namespace aspnet.webapi.Services;

public class ProductsService(ProductsRepository productsRepository)
{
    private readonly ProductsRepository _productsRepository = productsRepository;

    public async Task CreateAsync(Product product)
    {
        await _productsRepository.CreateAsync(product);
    }

    public async Task<Product?> GetByIdAsync(Guid productId)
    {
        var result = await _productsRepository.GetByIdAsync(productId);
        return result;
    }
}
