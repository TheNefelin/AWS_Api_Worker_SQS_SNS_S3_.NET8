using ClassLibrary_Domain.Entities;

namespace ClassLibrary_Domain.Interfaces;

public interface IProductRepository
{
    Task<IEnumerable<Product>> GetAllProductsAsync();
    Task<IEnumerable<Product>> GetRandomProductsAsync(int amount);
}
