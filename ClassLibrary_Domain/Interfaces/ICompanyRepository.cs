using ClassLibrary_Domain.Entities;

namespace ClassLibrary_Domain.Interfaces;

public interface ICompanyRepository
{
    Task<IEnumerable<Company>> GetAllCompaniesAsync();
    Task<Company?> GetRandomCompanyAsync();
}
