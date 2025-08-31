using ClassLibrary_Application.DTOs;
using ClassLibrary_Domain.Entities;

namespace ClassLibrary_Application.Mappers;

public class CompanyMapper
{
    public static CompanyDTO ToDto(Company entity) => new CompanyDTO
    {
        Company_id = entity.Company_id,
        Name = entity.Name,
        Email = entity.Email,
        Img = entity.Img
    };
}
