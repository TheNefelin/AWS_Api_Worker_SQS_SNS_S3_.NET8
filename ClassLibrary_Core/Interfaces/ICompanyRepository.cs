using ClassLibrary_Core.Entities;

namespace ClassLibrary_Core.Interfaces;

public interface ICompanyRepository
{
    Task<IEnumerable<Company>> GetAllCompaniesAsync();
    Task<Company?> GetRandomCompanyAsync();
}
