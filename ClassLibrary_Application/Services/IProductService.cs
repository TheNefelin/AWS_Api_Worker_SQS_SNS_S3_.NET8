using ClassLibrary_Application.DTOs;
using ClassLibrary_Application.Models;

namespace ClassLibrary_Application.Services;

public interface IProductService
{
    public Task<ApiResponse<IEnumerable<ProductDTO>>> GetAllProductsAsync();
    public Task<ApiResponse<IEnumerable<ProductDTO>>> GetRandomProductsAsync(int amount);
}
