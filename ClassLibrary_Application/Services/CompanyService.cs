using ClassLibrary_Application.DTOs;
using ClassLibrary_Application.Mappers;
using ClassLibrary_Application.Models;
using ClassLibrary_Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace ClassLibrary_Application.Services;

public class CompanyService : ICompanyService
{
    private readonly ILogger<CompanyService> _logger;
    private readonly ICompanyRepository _companyRepository;

    public CompanyService(ILogger<CompanyService> logger, ICompanyRepository companyRepository)
    {
        _logger = logger;
        _companyRepository = companyRepository;
    }

    public async Task<ApiResponse<IEnumerable<CompanyDTO>>> GetAllCompaniesAsync()
    {
        try
        {
            _logger.LogInformation("[CompanyService] Fetching all companies");
            var companies = await _companyRepository.GetAllCompaniesAsync();

            _logger.LogInformation("[CompanyService] Mapped companies to DTOs");
            var companyDTOs = companies.Select(CompanyMapper.ToDto).ToList();

            _logger.LogInformation("[CompanyService] Returning successful response");
            return new ApiResponse<IEnumerable<CompanyDTO>>
            {   
                IsSuccess = true,
                StatusCode = 200,
                Data = companyDTOs,
                Message = "Companies fetched successfully"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[CompanyService] Error fetching companies");
            return new ApiResponse<IEnumerable<CompanyDTO>>
            {
                IsSuccess = false,
                StatusCode = 500,
                Message = $"An error occurred: {ex.Message}"
            };
        }
    }

    public async Task<ApiResponse<CompanyDTO>> GetRandomCompanyAsync()
    {
        try
        {
            _logger.LogInformation("[CompanyService] Fetching a random company");
            var company = await _companyRepository.GetRandomCompanyAsync();

            if (company == null)
            {
                _logger.LogWarning("[CompanyService] No company found");
                return new ApiResponse<CompanyDTO>
                {
                    IsSuccess = false,
                    StatusCode = 404,
                    Message = "No company found"
                };
            }

            _logger.LogInformation("[CompanyService] Mapped company to DTO");
            var companyDTO = CompanyMapper.ToDto(company);

            _logger.LogInformation("[CompanyService] Returning successful response");
            return new ApiResponse<CompanyDTO>
            {
                IsSuccess = true,
                StatusCode = 200,
                Data = companyDTO,
                Message = "Company fetched successfully"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[CompanyService] Error fetching random company");
            return new ApiResponse<CompanyDTO>
            {
                IsSuccess = false,
                StatusCode = 500,
                Message = $"An error occurred: {ex.Message}"
            };
        }
    }
}

