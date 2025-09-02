namespace ClassLibrary_Infrastructure.Models;

public class InvoiceData
{
    public required string CompanyName { get; set; }
    public required string CompanyEmail { get; set; }
    public Stream? ComanyImgStream { get; set; }
    public required string Email { get; set; }
    public List<InvoiceProduct> InvoiceProductList { get; set; } = new();
}
