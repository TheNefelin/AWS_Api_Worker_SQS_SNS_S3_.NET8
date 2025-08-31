using ClassLibrary_Application.DTOs;
using ClassLibrary_Application.Models;
using ClassLibrary_Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace AWS_WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CompaniesController : ControllerBase
{
    private readonly ILogger<CompaniesController> _logger;
    private readonly ICompanyService _companyService;

    public CompaniesController(ILogger<CompaniesController> logger, ICompanyService companyService)
    {
        _logger = logger;
        _companyService = companyService;
    }

    [HttpGet("all")]
    public async Task<ActionResult<ApiResponse<CompanyDTO>>> GetAllCompanies()
    {
        _logger.LogInformation("[CompaniesController] GET /api/companies/all called");
        var response = await _companyService.GetAllCompaniesAsync();
        return StatusCode(response.StatusCode, response);
    }

    [HttpGet("random")]
    public async Task<ActionResult<ApiResponse<CompanyDTO>>> GetRandomCompany()
    {
        _logger.LogInformation("[CompaniesController] GET /api/companies/random called");
        var response = await _companyService.GetRandomCompanyAsync();
        return StatusCode(response.StatusCode, response);
    }
}
