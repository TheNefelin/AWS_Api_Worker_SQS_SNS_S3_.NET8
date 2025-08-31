using ClassLibrary_Application.DTOs;
using ClassLibrary_Domain.Entities;

namespace ClassLibrary_Application.Mappers;

public class ProductMapper
{
    public static ProductDTO ToDto(Product entity) => new ProductDTO
    {
        Product_id = entity.Product_id,
        Name = entity.Name,
        Description = entity.Description,
        Price = entity.Price
    };
}
