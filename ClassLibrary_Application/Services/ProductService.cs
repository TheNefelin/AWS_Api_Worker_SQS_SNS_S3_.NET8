using ClassLibrary_Application.DTOs;
using ClassLibrary_Application.Mappers;
using ClassLibrary_Application.Models;
using ClassLibrary_Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace ClassLibrary_Application.Services;

public class ProductService : IProductService
{
    private readonly ILogger<CompanyService> _logger;
    private readonly IProductRepository _productRepository;

    public ProductService(ILogger<CompanyService> logger, IProductRepository productRepository)
    {
        _logger = logger;
        _productRepository = productRepository;
    }

    public async Task<ApiResponse<IEnumerable<ProductDTO>>> GetAllProductsAsync()
    {
        try
        {
            _logger.LogInformation("[ProductService] Fetching all products");
            var products = await _productRepository.GetAllProductsAsync();

            _logger.LogInformation("[ProductService] Mapped products to DTOs");
            var productDTOs = products.Select(ProductMapper.ToDto).ToList();

            _logger.LogInformation("[ProductService] Returning successful response");
            return new ApiResponse<IEnumerable<ProductDTO>>
            {
                IsSuccess = true,
                StatusCode = 200,
                Message = "Products fetched successfully",
                Data = productDTOs
            };
        }
        catch (Exception ex) 
        {
            _logger.LogError(ex, "[ProductService] Error fetching products");
            return new ApiResponse<IEnumerable<ProductDTO>>
            {
                IsSuccess = true,
                StatusCode = 200,
                Message = $"An error occurred: {ex.Message}"
            };
        }
    }

    public async Task<ApiResponse<IEnumerable<ProductDTO>>> GetRandomProductsAsync(int amount)
    {
        try
        {
            _logger.LogInformation("[ProductService] Fetching all products");
            var products = await _productRepository.GetRandomProductsAsync(amount);

            _logger.LogInformation("[ProductService] Mapped products to DTOs");
            var productDTOs = products.Select(ProductMapper.ToDto).ToList();

            _logger.LogInformation("[ProductService] Returning successful response");
            return new ApiResponse<IEnumerable<ProductDTO>>
            {
                IsSuccess = true,
                StatusCode = 200,
                Data = productDTOs,
                Message = "Company fetched successfully"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[ProductService] Error fetching products");
            return new ApiResponse<IEnumerable<ProductDTO>>
            {
                IsSuccess = false,
                StatusCode = 500,
                Message = $"An error occurred: {ex.Message}"
            };
        }
    }
}
