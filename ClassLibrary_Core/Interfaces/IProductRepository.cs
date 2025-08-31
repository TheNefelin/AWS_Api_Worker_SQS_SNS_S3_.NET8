using ClassLibrary_Core.Entities;

namespace ClassLibrary_Core.Interfaces;

public interface IProductRepository
{
    Task<IEnumerable<Product>> GetAllProductsAsync();
    Task<IEnumerable<Product>> GetRandomProductsAsync(int amount);
}
