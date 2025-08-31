using ClassLibrary_Application.DTOs;
using ClassLibrary_Application.Models;

namespace ClassLibrary_Application.Services;

public interface ICompanyService
{
    public Task<ApiResponse<IEnumerable<CompanyDTO>>> GetAllCompaniesAsync();
    public Task<ApiResponse<CompanyDTO>> GetRandomCompanyAsync();
}
