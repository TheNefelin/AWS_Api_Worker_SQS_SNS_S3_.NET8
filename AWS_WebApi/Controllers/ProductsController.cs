using ClassLibrary_Application.DTOs;
using ClassLibrary_Application.Models;
using ClassLibrary_Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace AWS_WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly ILogger<CompaniesController> _logger;
    private readonly IProductService _productService;

    public ProductsController(ILogger<CompaniesController> logger, IProductService productService)
    {
        _logger = logger;
        _productService = productService;
    }

    [HttpGet("all")]
    public async Task<ActionResult<ApiResponse<IEnumerable<ProductDTO>>>> GetAllCompanies()
    {
        _logger.LogInformation("[ProductsController] GET /api/products/all called");
        var response = await _productService.GetAllProductsAsync();
        return StatusCode(response.StatusCode, response);
    }

    [HttpGet("random")]
    public async Task<ActionResult<ApiResponse<IEnumerable<ProductDTO>>>> GetRandomCompany(int amount)
    {
        _logger.LogInformation("[CompaniesController] GET /api/products/random called");
        var response = await _productService.GetRandomProductsAsync(amount);
        return StatusCode(response.StatusCode, response);
    }
}
