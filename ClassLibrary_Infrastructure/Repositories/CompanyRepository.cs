using ClassLibrary_Core.Entities;
using ClassLibrary_Core.Interfaces;
using ClassLibrary_Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ClassLibrary_Infrastructure.Repositories;

public class CompanyRepository : ICompanyRepository
{
    private readonly ILogger<CompanyRepository> _logger;
    private readonly AppDbContext _context;

    public CompanyRepository(ILogger<CompanyRepository> logger, AppDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<IEnumerable<Company>> GetAllCompaniesAsync()
    {
        try
        {
            _logger.LogInformation("[CompanyRepository] Fetching all companies from the database.");
            return await _context.companies.ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[CompanyRepository] An error occurred while fetching all companies.");
            throw;
        }
    }

    public async Task<Company?> GetRandomCompanyAsync()
    {
        try
        {
            _logger.LogInformation("[CompanyRepository] Fetching a random company from the database.");
            return await _context.companies.OrderBy(c => Guid.NewGuid()).FirstOrDefaultAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[CompanyRepository] An error occurred while fetching a random company.");
            throw;
        }
    }
}
