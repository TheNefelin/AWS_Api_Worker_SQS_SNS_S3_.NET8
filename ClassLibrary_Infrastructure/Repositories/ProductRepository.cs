using ClassLibrary_Domain.Entities;
using ClassLibrary_Domain.Interfaces;
using ClassLibrary_Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ClassLibrary_Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly ILogger<ProductRepository> _logger;
    private readonly AppDbContext _context;

    public ProductRepository(ILogger<ProductRepository> logger, AppDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<IEnumerable<Product>> GetAllProductsAsync()
    {
        try
        {
            _logger.LogInformation("[ProductRepository] Fetching all products from the database.");
            return await _context.products.ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[ProductRepository] An error occurred while fetching all products.");
            throw;
        }
    }

    public async Task<IEnumerable<Product>> GetRandomProductsAsync(int amount)
    {
        try
        {
            _logger.LogInformation("[ProductRepository] Fetching random products from the database.");
            return await _context.products.OrderBy(p => Guid.NewGuid()).Take(amount).ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[ProductRepository] An error occurred while fetching random products.");  
            throw;
        }
    }
}
